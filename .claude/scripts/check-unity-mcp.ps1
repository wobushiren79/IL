<#
.SYNOPSIS
  Detects Unity MCP connection status and auto-starts the HTTP server if needed.

.DESCRIPTION
  Checks whether:
  1. The Unity MCP HTTP server is accepting connections (TCP probe)
  2. Unity Editor is running
  If the HTTP server is not reachable, attempts to auto-start it via uvx.
  Outputs a systemMessage JSON reminder when MCP is not connected.

.PARAMETER Port
  HTTP server port to probe. Defaults to 8080.

.PARAMETER TimeoutMs
  TCP connection timeout in milliseconds. Defaults to 1500.

.PARAMETER CheckOnly
  Only check status, do not attempt to start the server.

.EXAMPLE
  .claude/scripts/check-unity-mcp.ps1
  # Auto-detect and start if needed

.EXAMPLE
  .claude/scripts/check-unity-mcp.ps1 -CheckOnly
  # Only report status without starting
#>

param(
    [int]$Port = 8080,
    [int]$TimeoutMs = 1500,
    [switch]$CheckOnly
)

$ErrorActionPreference = "Stop"

# ------------------------------------------------------------------
# Helpers
# ------------------------------------------------------------------

function Test-TcpPort {
    param([string]$HostName = "127.0.0.1", [int]$Port, [int]$TimeoutMs = 1000)
    try {
        $client = New-Object System.Net.Sockets.TcpClient
        $connect = $client.BeginConnect($HostName, $Port, $null, $null)
        $wait = $connect.AsyncWaitHandle.WaitOne($TimeoutMs, $false)
        if ($wait -and $client.Connected) {
            $client.Close()
            return $true
        }
        try { $client.Close() } catch {}
    } catch {}
    return $false
}

function Find-UvxPath {
    $candidates = @("uvx", "uvx.exe")
    foreach ($c in $candidates) {
        $found = Get-Command $c -ErrorAction SilentlyContinue
        if ($found) { return $found.Source }
    }
    $commonPaths = @(
        "$env:LOCALAPPDATA\Programs\uv\uvx.exe"
        "$env:PROGRAMFILES\uv\uvx.exe"
        "$env:USERPROFILE\.cargo\bin\uvx.exe"
        "$env:USERPROFILE\.local\bin\uvx.exe"
        "$env:LOCALAPPDATA\Microsoft\WinGet\Links\uvx.exe"
    )
    foreach ($p in $commonPaths) {
        if (Test-Path $p) { return $p }
    }
    return $null
}

function Test-UnityEditorRunning {
    $unity = Get-Process | Where-Object {
        ($_.ProcessName -eq "Unity" -or $_.ProcessName -eq "Unity.exe") -and
        $_.MainWindowTitle -and $_.MainWindowTitle -notmatch "Unity Hub"
    } | Select-Object -First 1
    return ($null -ne $unity)
}

function Start-McpHttpServer {
    param([string]$UvxPath, [int]$Port)

    $cmd = "`"$UvxPath`" --from mcpforunityserver mcp-for-unity --transport http --http-url http://127.0.0.1:$Port --project-scoped-tools"

    $scriptsDir = Join-Path $PSScriptRoot ".." ".." "Library" "MCPForUnity" "TerminalScripts"
    New-Item -ItemType Directory -Force -Path $scriptsDir | Out-Null
    $scriptPath = Join-Path $scriptsDir "mcp-auto-start.cmd"

    $content = "@echo off`r`n" +
               "title MCP For Unity Server`r`n" +
               "echo Starting MCP HTTP server on port $Port...`r`n" +
               "$cmd`r`n" +
               "echo.`r`n" +
               "echo Server exited. Press any key to close.`r`n" +
               "pause >nul`r`n"
    [System.IO.File]::WriteAllText($scriptPath, $content)

    $startInfo = New-Object System.Diagnostics.ProcessStartInfo
    $startInfo.FileName = "cmd.exe"
    $startInfo.Arguments = "/c start `"MCP Server`" cmd.exe /k `"$scriptPath`""
    $startInfo.UseShellExecute = $false
    $startInfo.CreateNoWindow = $true

    [System.Diagnostics.Process]::Start($startInfo) | Out-Null
    return $true
}

# ------------------------------------------------------------------
# Main
# ------------------------------------------------------------------

$httpRunning = Test-TcpPort -Port $Port -TimeoutMs $TimeoutMs
$unityRunning = Test-UnityEditorRunning

# Silent when everything is healthy
if ($httpRunning) {
    exit 0
}

# Build status report
$parts = @()
$parts += "Unity MCP HTTP server is NOT running on port $Port."

if (-not $unityRunning) {
    $parts += "Unity Editor does not appear to be running."
}

# Attempt auto-start unless CheckOnly
$started = $false
$startError = $null
if (-not $CheckOnly) {
    $uvx = Find-UvxPath
    if ($uvx) {
        try {
            Start-McpHttpServer -UvxPath $uvx -Port $Port
            $started = $true
            $parts += "Auto-started the MCP HTTP server in a new terminal window."
        } catch {
            $startError = $_.Exception.Message
            $parts += "Auto-start failed: $startError"
        }
    } else {
        $startError = "uvx not found in PATH or common install locations."
        $parts += $startError
    }
}

# If we just started it, give it a moment then re-check
if ($started) {
    Start-Sleep -Seconds 2
    $httpRunning = Test-TcpPort -Port $Port -TimeoutMs 2000
    if ($httpRunning) {
        $parts += "Server is now accepting connections."
    } else {
        $parts += "Server starting, may need a few more seconds."
    }
}

$parts += "Next steps: open Unity Editor > Window > MCP For Unity > click 'Start Session' if session is not active."

$message = ($parts -join " ")
# Escape for JSON
$message = $message -replace '"', '\"' -replace "\r\n", " " -replace "\n", " "

Write-Output "{`"systemMessage`":`"$message`"}"
exit 0
