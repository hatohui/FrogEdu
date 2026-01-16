# Bootstrap ECR Images for Initial Deployment
# Run this BEFORE terraform apply on first deployment
# Usage: doppler run -- pwsh .\scripts\bootstrap-ecr.ps1

param(
    [string]$Region = $env:TF_AWS_REGION,
    [string]$AccountId = ""
)

$ErrorActionPreference = "Stop"

# Get region from environment if not provided
if ([string]::IsNullOrEmpty($Region)) {
    $Region = "ap-southeast-1"
}

Write-Host "[*] Bootstrapping ECR Images..." -ForegroundColor Cyan
Write-Host "   Region: $Region" -ForegroundColor Gray

# Service mapping: name -> folder
$services = @{
    "content" = "Content"
    "user" = "User"
    "assessment" = "Assessment"
    "ai" = "AI"
}

# AWS ECR Login
Write-Host "`n[*] Logging into Amazon ECR..." -ForegroundColor Yellow

# Get AWS account ID from STS
Write-Host "   Getting AWS account ID..." -ForegroundColor Gray
$identity = aws sts get-caller-identity --query Account --output text
if ($LASTEXITCODE -ne 0) {
    Write-Host "[ERROR] Failed to get AWS account ID. Make sure AWS credentials are configured." -ForegroundColor Red
    exit 1
}
$AccountId = $identity.Trim()
Write-Host "   AWS Account: $AccountId" -ForegroundColor Gray

# Check if ECR repositories exist
Write-Host "   Checking ECR repositories..." -ForegroundColor Gray
foreach ($service in $services.Keys) {
    $serviceName = "$service-api"
    $repoName = "frogedu-dev-$serviceName"
    $repoCheck = aws ecr describe-repositories --repository-names $repoName --region $Region 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[WARNING] ECR repository $repoName does not exist" -ForegroundColor Yellow
        Write-Host "   Creating repository..." -ForegroundColor Gray
        aws ecr create-repository --repository-name $repoName --region $Region --image-scanning-configuration scanOnPush=true | Out-Null
        if ($LASTEXITCODE -ne 0) {
            Write-Host "[ERROR] Failed to create ECR repository $repoName" -ForegroundColor Red
            exit 1
        }
        Write-Host "   Created $repoName" -ForegroundColor Green
    }
}

# Use cmd.exe for ECR login (handles piping better on Windows)
Write-Host "   Logging into ECR..." -ForegroundColor Gray
cmd /c "aws ecr get-login-password --region $Region | docker login --username AWS --password-stdin $AccountId.dkr.ecr.$Region.amazonaws.com"

if ($LASTEXITCODE -ne 0) {
    Write-Host "[ERROR] Failed to login to ECR" -ForegroundColor Red
    exit 1
}
Write-Host "   ECR login successful" -ForegroundColor Green

foreach ($service in $services.Keys) {
    $folderName = $services[$service]
    $serviceName = "$service-api"
    $repoName = "frogedu-dev-$serviceName"
    $imageUri = "$AccountId.dkr.ecr.$Region.amazonaws.com/$repoName"
    
    Write-Host "`n[*] Building $serviceName..." -ForegroundColor Green
    Write-Host "   Dockerfile: backend/Services/$folderName/Dockerfile" -ForegroundColor Gray
    
    # Build the image
    docker build `
        --provenance=false `
        -f "backend/Services/$folderName/Dockerfile" `
        -t "$imageUri`:latest" `
        backend/
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[ERROR] Failed to build $serviceName" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "[*] Pushing $serviceName to ECR..." -ForegroundColor Blue
    docker push "$imageUri`:latest"
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[ERROR] Failed to push $serviceName" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "[SUCCESS] $serviceName deployed to ECR" -ForegroundColor Green
}

Write-Host "`n[SUCCESS] All images bootstrapped successfully!" -ForegroundColor Cyan
Write-Host "Now you can run: doppler run -- terraform apply" -ForegroundColor Yellow
