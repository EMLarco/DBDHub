param(
    [string]$Namespace = 'dbdhub',
    [string]$JobName = 'k6-match-load',
    [string]$ConfigMapName = 'k6-match-load'
)

$ErrorActionPreference = 'Stop'

$scriptPath = Join-Path $PSScriptRoot 'match-load.js'
$jobPath = Join-Path $PSScriptRoot 'k8s\job.yaml'
$resultsDir = Join-Path $PSScriptRoot 'results'
New-Item -ItemType Directory -Path $resultsDir -Force | Out-Null

$stamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$timelinePath = Join-Path $resultsDir "hpa-timeline-$stamp.log"
$logPath = Join-Path $resultsDir "k6-$stamp.log"

kubectl delete job $JobName -n $Namespace --ignore-not-found | Out-Null
kubectl delete configmap $ConfigMapName -n $Namespace --ignore-not-found | Out-Null
kubectl create configmap $ConfigMapName -n $Namespace --from-file="match-load.js=$scriptPath" | Out-Null
kubectl apply -f $jobPath | Out-Null

Write-Output "Job $JobName lanzado. Evidencia:"
Write-Output "  $logPath"
Write-Output "  $timelinePath"

$deadline = (Get-Date).AddMinutes(8)
while ((Get-Date) -lt $deadline) {
    $ready = kubectl get deploy dbdhub-app -n $Namespace -o jsonpath='{.status.readyReplicas}' 2>$null
    $hpa = kubectl get hpa dbdhub-app -n $Namespace --no-headers 2>$null
    $line = "$(Get-Date -Format 'HH:mm:ss') ready=$ready hpa=$hpa"
    Add-Content -LiteralPath $timelinePath -Value $line

    $jobJson = kubectl get job $JobName -n $Namespace -o json 2>$null
    $jobObject = $null
    if ($LASTEXITCODE -eq 0 -and $jobJson) {
        $jobObject = $jobJson | ConvertFrom-Json
    }
    $complete = $jobObject.status.conditions | Where-Object { $_.type -eq 'Complete' -and $_.status -eq 'True' }
    $failed = $jobObject.status.conditions | Where-Object { $_.type -eq 'Failed' -and $_.status -eq 'True' }
    if ($complete -or $failed) {
        break
    }

    Start-Sleep -Seconds 5
}

kubectl logs -n $Namespace job/$JobName --tail=-1 2>&1 | Tee-Object -FilePath $logPath
Write-Output '--- HPA TIMELINE ---'
Get-Content -LiteralPath $timelinePath
Write-Output "--- max ready replicas ---"
(Get-Content -LiteralPath $timelinePath | ForEach-Object {
    if ($_ -match 'ready=(\d+)') { [int]$Matches[1] }
} | Measure-Object -Maximum).Maximum
Write-Output "Logs: $logPath"
Write-Output "Timeline: $timelinePath"
