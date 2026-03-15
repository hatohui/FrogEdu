#!/usr/bin/env pwsh
# Seed test users into the User service DB via the auth webhook endpoint.
# Run AFTER creating the Cognito users.

param(
    [string]$ApiBase = "https://api.frogedu.org/api/users"
)

$webhookUrl = "$ApiBase/auth/webhook"
Write-Host "Webhook URL: $webhookUrl"

$users = @(
    @{
        sub        = "d9aa350c-20a1-70f5-7d68-2eeb003f5333"
        email      = "testadmin@frogedu.org"
        givenName  = "Test"
        familyName = "Admin"
        customRole = "Admin"
    },
    @{
        sub        = "39ea658c-3041-7098-127c-f3e6a6e33952"
        email      = "testteacher@frogedu.org"
        givenName  = "Test"
        familyName = "Teacher"
        customRole = "Teacher"
    },
    @{
        sub        = "197a75dc-4081-70a4-214a-aefb340775f9"
        email      = "teststudent@frogedu.org"
        givenName  = "Test"
        familyName = "Student"
        customRole = "Student"
    }
)

foreach ($u in $users) {
    $payload = @{
        request = @{
            userAttributes = @{
                sub        = $u.sub
                email      = $u.email
                givenName  = $u.givenName
                familyName = $u.familyName
                customRole = $u.customRole
            }
        }
    }

    $json = $payload | ConvertTo-Json -Depth 5 -Compress
    Write-Host "`n--- Registering $($u.customRole): $($u.email) ---"
    Write-Host "Payload: $json"

    try {
        $response = Invoke-RestMethod `
            -Method POST `
            -Uri $webhookUrl `
            -ContentType "application/json" `
            -Body $json `
            -ErrorAction Stop

        Write-Host "SUCCESS: $($response | ConvertTo-Json -Compress)"
    }
    catch {
        $statusCode = $_.Exception.Response.StatusCode.value__
        Write-Host "FAILED (HTTP $statusCode): $($_.Exception.Message)"

        if ($_.Exception.Response) {
            $reader = [System.IO.StreamReader]::new($_.Exception.Response.GetResponseStream())
            $body = $reader.ReadToEnd()
            Write-Host "Response body: $body"
        }
    }
}

Write-Host "`nDone."
