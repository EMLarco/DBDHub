$ErrorActionPreference = 'Stop'
Set-Location (Join-Path $PSScriptRoot '..\..')
$tokenFile = 'C:\Users\suerz\Documents\Codex\2026-11-05\tengo\work\sonar_token.tmp'
$logPath = 'C:\Users\suerz\OneDrive\Desktop\DBDHub\sonar\results\sonar-scan-20261105.log'
$env:SONAR_TOKEN = (Get-Content -LiteralPath $tokenFile -Raw).Trim()
try {
  dotnet sonarscanner begin /k:"dbdhub" /n:"DBDHub" /d:sonar.host.url="http://localhost:9000" /d:sonar.token="$env:SONAR_TOKEN" /d:sonar.qualitygate.wait=true /d:sonar.qualitygate.timeout=300 2>&1 | Tee-Object -FilePath $logPath
  if ($LASTEXITCODE -ne 0) { throw "Sonar begin failed: $LASTEXITCODE" }
  dotnet build .\DBDHub.csproj --no-incremental /p:UseSharedCompilation=false 2>&1 | Tee-Object -FilePath $logPath -Append
  if ($LASTEXITCODE -ne 0) { throw "dotnet build failed: $LASTEXITCODE" }
  dotnet sonarscanner end /d:sonar.token="$env:SONAR_TOKEN" 2>&1 | Tee-Object -FilePath $logPath -Append
  if ($LASTEXITCODE -ne 0) { throw "Sonar end failed: $LASTEXITCODE" }
}
finally {
  Remove-Item Env:\SONAR_TOKEN -ErrorAction SilentlyContinue
}
