# 通用 Unity MCP 调用辅助（JSON-RPC over HTTP，streamable HTTP 传输）
# 用法：invoke-unity-mcp.ps1 -Body '<json-rpc body>' [-Port 8080] [-Reinit]
# 行为：
#   1. 自动完成 initialize 握手（Mcp-Session-Id 缓存到 $env:TEMP，后续调用复用）
#   2. 自动发送 notifications/initialized
#   3. 解析 SSE(text/event-stream) 响应，提取 data: 行输出为 JSON 文本
#   4. session 失效(4xx)时自动重新握手重试一次；-Reinit 强制重握手
# 注意：本脚本含中文注释，必须保存为 UTF-8 with BOM（Windows PowerShell 5.1 按 ANSI 误读无 BOM 中文）
param(
    [string]$Body,
    [string]$BodyBase64,
    [int]$Port = 8080,
    [switch]$Reinit
)

# BodyBase64 优先：Windows PowerShell 5.1 的 -File 参数解析会剥离内嵌双引号，JSON body 一律走 base64 传入
if (-not [string]::IsNullOrEmpty($BodyBase64)) {
    $Body = [Text.Encoding]::UTF8.GetString([Convert]::FromBase64String($BodyBase64))
}
if ([string]::IsNullOrEmpty($Body)) { throw "需要 -Body 或 -BodyBase64 参数" }

$ErrorActionPreference = 'Stop'
$uri = "http://127.0.0.1:$Port/mcp"
$sessionFile = Join-Path $env:TEMP "unity_mcp_session_$Port.txt"

function Initialize-McpSession {
    # 握手：initialize -> 从响应头取 Mcp-Session-Id -> notifications/initialized
    $initBody = '{"jsonrpc":"2.0","id":0,"method":"initialize","params":{"protocolVersion":"2024-11-05","capabilities":{},"clientInfo":{"name":"claude-code","version":"1.0"}}}'
    $headers = @{ "Accept" = "application/json, text/event-stream"; "Content-Type" = "application/json" }
    $resp = Invoke-WebRequest -Uri $uri -Method POST -Body ([Text.Encoding]::UTF8.GetBytes($initBody)) -Headers $headers -UseBasicParsing
    $sessionId = $resp.Headers["Mcp-Session-Id"]
    if ([string]::IsNullOrEmpty($sessionId)) { throw "initialize 未返回 Mcp-Session-Id" }
    [IO.File]::WriteAllText($sessionFile, $sessionId, (New-Object Text.UTF8Encoding $false))
    $notifyHeaders = @{ "Accept" = "application/json, text/event-stream"; "Content-Type" = "application/json"; "Mcp-Session-Id" = $sessionId }
    $notifyBody = '{"jsonrpc":"2.0","method":"notifications/initialized","params":{}}'
    Invoke-WebRequest -Uri $uri -Method POST -Body ([Text.Encoding]::UTF8.GetBytes($notifyBody)) -Headers $notifyHeaders -UseBasicParsing | Out-Null
    return $sessionId
}

function Get-McpSessionId {
    if ($Reinit) { return Initialize-McpSession }
    if (Test-Path $sessionFile) {
        $cached = [IO.File]::ReadAllText($sessionFile).Trim()
        if (-not [string]::IsNullOrEmpty($cached)) { return $cached }
    }
    return Initialize-McpSession
}

function Send-McpRequest([string]$sessionId, [string]$jsonBody) {
    # 发送 JSON-RPC 请求并解析 SSE 响应；返回 data 载荷文本
    $headers = @{ "Accept" = "application/json, text/event-stream"; "Content-Type" = "application/json"; "Mcp-Session-Id" = $sessionId }
    # Body 必须 UTF8 字节化：PS5.1 的 Invoke-WebRequest 对 string Body 默认按 ISO-8859-1 编码，中文路径会变问号
    $resp = Invoke-WebRequest -Uri $uri -Method POST -Body ([Text.Encoding]::UTF8.GetBytes($jsonBody)) -Headers $headers -UseBasicParsing
    $content = $resp.Content
    if ($null -eq $content) { return "" }
    # SSE 格式：提取所有 data: 行拼接；非 SSE 则原样返回
    if ($content -match '(?m)^data:') {
        $lines = $content -split "`n" | Where-Object { $_ -match '^data:' } | ForEach-Object { $_.Substring(5).Trim() }
        return ($lines -join "`n")
    }
    return $content
}

$sessionId = Get-McpSessionId
try {
    $result = Send-McpRequest $sessionId $Body
}
catch {
    # session 失效（服务器重启等）：清缓存重握手后重试一次
    $statusCode = $null
    if ($_.Exception.Response) { $statusCode = [int]$_.Exception.Response.StatusCode }
    if ($statusCode -ge 400 -and $statusCode -lt 500) {
        Remove-Item $sessionFile -Force -ErrorAction SilentlyContinue
        $sessionId = Initialize-McpSession
        $result = Send-McpRequest $sessionId $Body
    }
    else { throw }
}
Write-Output $result
