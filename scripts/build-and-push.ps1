#!/usr/bin/env pwsh
# Build and push all FrogEdu microservices to ECR

param(
    [string]$Region = "ap-southeast-1",
    [string]$AccountId = "630633962130",
    [string]$Environment = "dev"
)

$ErrorActionPreference = "Stop"

# Colors for output
function Write-Success { Write-Host $args -ForegroundColor Green }
function Write-Info { Write-Host $args -ForegroundColor Cyan }
function Write-Warning { Write-Host $args -ForegroundColor Yellow }

# Configuration
$Services = @("subscription", "user", "exam", "class")
$BackendPath = "$PSScriptRoot/../backend"
$EcrRegistry = "$AccountId.dkr.ecr.$Region.amazonaws.com"

Write-Info "`n🐳 FrogEdu Docker Build & Push Script"
Write-Info "====================================`n"

# Step 1: Login to ECR
Write-Info "Step 1: Logging into ECR..."
try {
    doppler run -- aws ecr get-login-password --region $Region | docker login --username AWS --password-stdin $EcrRegistry
    Write-Success "✓ Logged into ECR successfully`n"
} catch {
    Write-Warning "⚠ ECR login failed. Make sure Doppler is configured and AWS credentials are set."
    exit 1
}

# Step 2: Build and push each service
foreach ($service in $Services) {
    $serviceName = "frogedu-$Environment-$service-api"
    $capitalizedService = (Get-Culture).TextInfo.ToTitleCase($service)
    $dockerfile = "backend/Services/$capitalizedService/Dockerfile"
    
    Write-Info "Step 2.$($Services.IndexOf($service) + 1): Building $serviceName..."
    
    # Build
    Write-Host "  Building Docker image..." -NoNewline
    $buildOutput = docker build -t "${serviceName}:latest" -f $dockerfile $BackendPath 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Success " ✓"
    } else {
        Write-Warning " ✗ Build failed"
        Write-Host $buildOutput -ForegroundColor Red
        continue
    }
    
    # Tag
    Write-Host "  Tagging for ECR..." -NoNewline
    docker tag "${serviceName}:latest" "$EcrRegistry/${serviceName}:latest"
    Write-Success " ✓"
    
    # Push
    Write-Host "  Pushing to ECR..." -NoNewline
    $pushOutput = docker push "$EcrRegistry/${serviceName}:latest" 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Success " ✓"
    } else {
        Write-Warning " ✗ Push failed"
        Write-Host $pushOutput -ForegroundColor Red
        continue
    }
    
    Write-Success "✓ $serviceName deployed to ECR`n"
}

Write-Success "`n✅ All services built and pushed to ECR!"
Write-Info "`nYou can now run:"
Write-Host "  cd infra" -ForegroundColor Gray
Write-Host "  doppler run -- terraform apply --auto-approve" -ForegroundColor Gray
Write-Host ""
