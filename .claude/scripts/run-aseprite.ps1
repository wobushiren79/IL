[CmdletBinding()]
param(
    [switch]$WhichOnly,
    [Parameter(ValueFromRemainingArguments = $true)]
    [object[]]$ForwardedArgs
)

$ErrorActionPreference = "Stop"

function Add-AsepriteCandidate {
    param(
        [System.Collections.Generic.List[string]]$Candidates,
        [string]$Path
    )

    if (-not [string]::IsNullOrWhiteSpace($Path)) {
        $Candidates.Add([Environment]::ExpandEnvironmentVariables($Path))
    }
}

function Get-AsepriteExecutable {
    $candidates = [System.Collections.Generic.List[string]]::new()

    Add-AsepriteCandidate -Candidates $candidates -Path $env:ASEPRITE_EXE

    $commands = Get-Command -Name "aseprite.exe", "aseprite" -ErrorAction SilentlyContinue
    foreach ($command in $commands) {
        Add-AsepriteCandidate -Candidates $candidates -Path $command.Source
    }

    $uninstallPaths = @(
        "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\*",
        "HKLM:\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\*",
        "HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\*"
    )

    foreach ($uninstallPath in $uninstallPaths) {
        $entries = Get-ItemProperty -Path $uninstallPath -ErrorAction SilentlyContinue |
            Where-Object { $_.DisplayName -like "Aseprite*" }
        foreach ($entry in $entries) {
            if ($entry.InstallLocation) {
                Add-AsepriteCandidate -Candidates $candidates -Path (Join-Path $entry.InstallLocation "Aseprite.exe")
            }
            if ($entry.DisplayIcon) {
                $icon = ([string]$entry.DisplayIcon).Trim('"')
                if ($icon -match "^(.*Aseprite\.exe)(?:,\d+)?$") {
                    Add-AsepriteCandidate -Candidates $candidates -Path $Matches[1]
                }
            }
        }
    }

    $steam = Get-ItemProperty -Path "HKCU:\Software\Valve\Steam" -ErrorAction SilentlyContinue
    if ($steam -and $steam.SteamPath) {
        $steamPath = [string]$steam.SteamPath
        Add-AsepriteCandidate -Candidates $candidates -Path (Join-Path $steamPath "steamapps\common\Aseprite\Aseprite.exe")

        $libraryFile = Join-Path $steamPath "steamapps\libraryfolders.vdf"
        if (Test-Path -LiteralPath $libraryFile -PathType Leaf) {
            $libraryText = [System.IO.File]::ReadAllText($libraryFile)
            foreach ($match in [regex]::Matches($libraryText, '"path"\s+"([^"]+)"')) {
                $libraryPath = $match.Groups[1].Value -replace "\\\\", "\"
                Add-AsepriteCandidate -Candidates $candidates -Path (Join-Path $libraryPath "steamapps\common\Aseprite\Aseprite.exe")
            }
        }
    }

    $commonPaths = @(
        (Join-Path $env:ProgramFiles "Aseprite\Aseprite.exe"),
        (Join-Path ${env:ProgramFiles(x86)} "Aseprite\Aseprite.exe"),
        (Join-Path $env:LOCALAPPDATA "Programs\Aseprite\Aseprite.exe")
    )
    foreach ($commonPath in $commonPaths) {
        Add-AsepriteCandidate -Candidates $candidates -Path $commonPath
    }

    $checked = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
    foreach ($candidate in $candidates) {
        if ($checked.Add($candidate) -and (Test-Path -LiteralPath $candidate -PathType Leaf)) {
            return (Resolve-Path -LiteralPath $candidate).Path
        }
    }

    throw "Aseprite.exe was not found. Set ASEPRITE_EXE or add aseprite to PATH."
}

$aseprite = Get-AsepriteExecutable

if ($WhichOnly) {
    Write-Output $aseprite
    exit 0
}

if ($null -eq $ForwardedArgs -or $ForwardedArgs.Count -eq 0) {
    Write-Output $aseprite
    exit 0
}

& $aseprite @ForwardedArgs | Out-Host
exit $LASTEXITCODE
