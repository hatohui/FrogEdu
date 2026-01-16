# Update all Lambda functions with new container images
# Usage: doppler run -- pwsh .\scripts\update-lambdas.ps1

param(
    [string]$Region = $env:TF_AWS_REGION
)

if ([string]::IsNullOrEmpty($Region)) {
    $Region = "ap-southeast-1"
}

$ErrorActionPreference = "Stop"

Write-Host "[*] Updating Lambda functions..." -ForegroundColor Cyan
Write-Host "   Region: $Region`n" -ForegroundColor Gray

$services = @("content-api", "user-api", "assessment-api", "ai-api")

foreach ($service in $services) {
    $functionName = "frogedu-dev-$service"
    
    Write-Host "[*] Updating $functionName..." -ForegroundColor Yellow
    
    # Get the latest image digest from ECR
    $imageDigest = aws ecr describe-images `
        --repository-name "frogedu-dev-$service" `
        --region $Region `
        --image-ids imageTag=latest `
        --query 'imageDetails[0].imageDigest' `
        --output text
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[ERROR] Failed to get image digest for $service" -ForegroundColor Red
        continue
    }
    
    # Update Lambda function
    $result = aws lambda update-function-code `
        --function-name $functionName `
        --region $Region `
        --image-uri "630633962130.dkr.ecr.$Region.amazonaws.com/frogedu-dev-${service}:latest" `
        --output json 2>&1
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[ERROR] Failed to update $functionName" -ForegroundColor Red
        Write-Host "   $result" -ForegroundColor Gray
    } else {
        Write-Host "[SUCCESS] Updated $functionName" -ForegroundColor Green
    }
}

Write-Host "`n[SUCCESS] All Lambda functions updated!" -ForegroundColor Cyan
Write-Host "Wait a few seconds for functions to become active, then test with:" -ForegroundColor Yellow
Write-Host "   .\scripts\test-api.ps1" -ForegroundColor Gray
