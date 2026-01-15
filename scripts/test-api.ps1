# Test FrogEdu API Endpoints
# This script tests all API endpoints to verify they're working

param(
    [string]$ApiUrl = "https://api.frogedu.org/api"
)

Write-Host "[*] Testing FrogEdu API..." -ForegroundColor Cyan
Write-Host "API URL: $ApiUrl`n" -ForegroundColor Gray

$endpoints = @(
    @{Name="Content API"; Path="/contents"},
    @{Name="Content Health"; Path="/contents/health"; NoAuth=$true},
    @{Name="User API"; Path="/users"},
    @{Name="User Health"; Path="/users/health"; NoAuth=$true},
    @{Name="Assessment API"; Path="/assessments"},
    @{Name="Assessment Health"; Path="/assessments/health"; NoAuth=$true},
    @{Name="AI API"; Path="/ai"},
    @{Name="AI Health"; Path="/ai/health"; NoAuth=$true}
)

foreach ($endpoint in $endpoints) {
    $url = "$ApiUrl$($endpoint.Path)"
    $isHealthCheck = $endpoint.NoAuth -eq $true
    $checkType = if ($isHealthCheck) { "Health Check (No Auth)" } else { "API Endpoint (Auth Required)" }
    
    Write-Host "[*] Testing $($endpoint.Name) - $checkType..." -ForegroundColor Yellow
    Write-Host "    URL: $url" -ForegroundColor Gray
    
    try {
        $response = Invoke-WebRequest -Uri $url -Method GET -UseBasicParsing -ErrorAction Stop
        Write-Host "[SUCCESS] Status: $($response.StatusCode) - $($endpoint.Name) is responding" -ForegroundColor Green
        Write-Host "    Response: $($response.Content.Substring(0, [Math]::Min(100, $response.Content.Length)))..." -ForegroundColor Gray
    }
    catch {
        $statusCode = $_.Exception.Response.StatusCode.value__
        if ($statusCode -eq 401) {
            if ($isHealthCheck) {
                Write-Host "[ERROR] Status: 401 - Health check should NOT require auth!" -ForegroundColor Red
            } else {
                Write-Host "[EXPECTED] Status: 401 Unauthorized - Cognito auth is required" -ForegroundColor Yellow
                Write-Host "    This is normal - the API requires authentication" -ForegroundColor Gray
            }
        }
        elseif ($statusCode -eq 404) {
            Write-Host "[INFO] Status: 404 - Endpoint exists but no route handler implemented yet" -ForegroundColor Yellow
        }
        elseif ($statusCode -eq 502 -or $statusCode -eq 503) {
            Write-Host "[ERROR] Status: $statusCode - Lambda function error" -ForegroundColor Red
            Write-Host "    Check Lambda logs: aws logs tail /aws/lambda/frogedu-dev-$($endpoint.Path.Replace('/',''))-api --follow" -ForegroundColor Gray
        }
        else {
            Write-Host "[ERROR] Status: $statusCode - $($_.Exception.Message)" -ForegroundColor Red
        }
    }
    Write-Host ""
}

Write-Host "`n[*] Summary:" -ForegroundColor Cyan
Write-Host "- If you see 401 Unauthorized: APIs are working, they just need authentication" -ForegroundColor Yellow
Write-Host "- If you see 404: Lambda is running but no routes are implemented yet in the code" -ForegroundColor Yellow
Write-Host "- If you see 502/503: Check Lambda function logs for errors" -ForegroundColor Red
Write-Host "`nTo view Lambda logs:" -ForegroundColor Cyan
Write-Host "  doppler run -- aws logs tail /aws/lambda/frogedu-dev-content-api --follow" -ForegroundColor Gray
Write-Host "  doppler run -- aws logs tail /aws/lambda/frogedu-dev-user-api --follow" -ForegroundColor Gray
