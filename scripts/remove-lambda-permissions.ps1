# =============================================================================
# Remove Conflicting Lambda Permissions
# =============================================================================
# This script removes existing Lambda permissions that conflict with Terraform
# Run this before terraform apply to resolve ResourceConflictException errors

param(
    [string]$Environment = "dev",
    [string]$ProjectName = "frogedu"
)

$ErrorActionPreference = "Continue"

Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host "Removing conflicting Lambda permissions for $ProjectName-$Environment" -ForegroundColor Cyan
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host ""

# Define Lambda functions and their statement IDs to remove
$lambdaFunctions = @(
    @{
        Name = "$ProjectName-$Environment-content-api"
        StatementIds = @(
            "AllowAPIGatewayInvoke-api-contents-proxyplus",
            "AllowAPIGatewayInvokeOptions-api-contents-proxyplus"
        )
    },
    @{
        Name = "$ProjectName-$Environment-user-api"
        StatementIds = @(
            "AllowAPIGatewayInvoke-api-users-proxyplus",
            "AllowAPIGatewayInvokeOptions-api-users-proxyplus"
        )
    },
    @{
        Name = "$ProjectName-$Environment-assessment-api"
        StatementIds = @(
            "AllowAPIGatewayInvoke-api-assessments-proxyplus",
            "AllowAPIGatewayInvokeOptions-api-assessments-proxyplus"
        )
    },
    @{
        Name = "$ProjectName-$Environment-ai-api"
        StatementIds = @(
            "AllowAPIGatewayInvoke-api-ai-proxyplus",
            "AllowAPIGatewayInvokeOptions-api-ai-proxyplus"
        )
    }
)

$successCount = 0
$errorCount = 0
$notFoundCount = 0

foreach ($lambda in $lambdaFunctions) {
    Write-Host "Processing Lambda: $($lambda.Name)" -ForegroundColor Yellow
    
    foreach ($statementId in $lambda.StatementIds) {
        Write-Host "  Removing permission: $statementId" -NoNewline
        
        try {
            $result = aws lambda remove-permission `
                --function-name $lambda.Name `
                --statement-id $statementId `
                2>&1
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host " ✓ Removed" -ForegroundColor Green
                $successCount++
            }
            elseif ($result -match "ResourceNotFoundException") {
                Write-Host " ⓘ Not found (already removed or never existed)" -ForegroundColor Gray
                $notFoundCount++
            }
            else {
                Write-Host " ✗ Error: $result" -ForegroundColor Red
                $errorCount++
            }
        }
        catch {
            Write-Host " ✗ Error: $_" -ForegroundColor Red
            $errorCount++
        }
    }
    Write-Host ""
}

Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host "Summary:" -ForegroundColor Cyan
Write-Host "  Removed:   $successCount permissions" -ForegroundColor Green
Write-Host "  Not Found: $notFoundCount permissions" -ForegroundColor Gray
Write-Host "  Errors:    $errorCount permissions" -ForegroundColor $(if ($errorCount -gt 0) { "Red" } else { "Green" })
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host ""

if ($errorCount -eq 0) {
    Write-Host "✓ All permissions cleaned up successfully!" -ForegroundColor Green
    Write-Host "You can now run: terraform apply" -ForegroundColor Yellow
    exit 0
}
else {
    Write-Host "⚠ Some errors occurred. Please review the output above." -ForegroundColor Yellow
    exit 1
}
