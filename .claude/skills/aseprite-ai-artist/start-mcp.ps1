$ErrorActionPreference = 'Stop'
$entry = Join-Path $PSScriptRoot 'node_modules/@pebbly/aseprite-ai-artist/dist/cli.js'
if (-not (Test-Path -LiteralPath $entry -PathType Leaf)) {
    throw 'Install Aseprite AI Artist dependencies with npm ci in this skill directory.'
}

$runner = Join-Path $PSScriptRoot '../../scripts/run-aseprite.ps1'
$env:ASEPRITE_PATH = & $runner -WhichOnly
if (-not $env:ASEPRITE_PATH) {
    throw 'Aseprite could not be located by the project launcher.'
}

& node $entry serve --headless
exit $LASTEXITCODE
