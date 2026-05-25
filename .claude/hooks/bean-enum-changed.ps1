# Claude Code PostToolUse hook
# 监测 Bean/Enum 改动，提示运行对应的 IL 代码生成/同步 skill。
# 通过 stderr 输出 + exit 2 让 Claude 看到提示但不阻塞用户操作。

$ErrorActionPreference = 'Stop'

try {
    $raw = [Console]::In.ReadToEnd()
    if ([string]::IsNullOrWhiteSpace($raw)) { exit 0 }
    $payload = $raw | ConvertFrom-Json
} catch {
    exit 0
}

$filePath = $null
if ($payload.tool_input -and $payload.tool_input.file_path) {
    $filePath = [string]$payload.tool_input.file_path
}
if (-not $filePath) { exit 0 }

$normalized = $filePath -replace '\\', '/'

$messages = @()

# Bean 改动：Cfg* / Db* / *Bean.cs
if ($normalized -match 'Assets/Scripts/Bean/.+\.cs$' -and $normalized -notmatch 'Partial\.cs$') {
    $messages += "Bean 主文件改动: $filePath"
    $messages += "  -> 如果新增了字段，请确认 GameData 下对应 Excel/JSON 已同步（/il-excel-sync）。"
    $messages += "  -> 如果新增的是 Cfg Bean，确认对应 Manager 的 InitData() 已注册。"
}

# Enum 改动
if ($normalized -match 'Assets/Scripts/Enums/.+\.cs$') {
    $messages += "Enum 改动: $filePath"
    $messages += "  -> 如果该枚举用于 Excel 数据类型映射，请确认 Excel 数值与枚举一致。"
    $messages += "  -> 如果枚举值用于序列化存档，新值请追加在末尾，不要插入或删除。"
}

if ($messages.Count -gt 0) {
    [Console]::Error.WriteLine("[IL hook]")
    foreach ($m in $messages) { [Console]::Error.WriteLine($m) }
    exit 2
}

exit 0
