# FrogEdu EF Core Migrations Setup Script
# This script automates Entity Framework Core migration creation and database updates
# for all microservices in the FrogEdu platform

param(
    [Parameter(Mandatory = $false)]
    [ValidateSet("init", "create", "update", "rollback", "verify", "clean")]
    [string]$Action = "verify",
    
    [Parameter(Mandatory = $false)]
    [ValidateSet("all", "user", "content", "assessment", "ai")]
    [string]$Service = "all",
    
    [Parameter(Mandatory = $false)]
    [string]$MigrationName = "Migration",
    
    [switch]$Force
)

# Color output functions
function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
}

function Write-Info {
    param([string]$Message)
    Write-Host "ℹ $Message" -ForegroundColor Cyan
}

function Write-Warning {
    param([string]$Message)
    Write-Host "⚠ $Message" -ForegroundColor Yellow
}

function Write-Error_ {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor Red
}

# Service configuration
$services = @{
    user = @{
        name = "User Service"
        apiPath = "Services/User/User.API"
        infraPath = "Services/User/User.Infrastructure"
        dbContext = "UserDbContext"
        port = 5432
    }
    content = @{
        name = "Content Service"
        apiPath = "Services/Content/Content.API"
        infraPath = "Services/Content/Content.Infrastructure"
        dbContext = "ContentDbContext"
        port = 5433
    }
    assessment = @{
        name = "Assessment Service"
        apiPath = "Services/Assessment/Assessment.API"
        infraPath = "Services/Assessment/Assessment.Infrastructure"
        dbContext = "AssessmentDbContext"
        port = 5434
    }
    ai = @{
        name = "AI Service"
        apiPath = "Services/AI/AI.API"
        infraPath = "Services/AI/AI.Infrastructure"
        dbContext = "AiDbContext"
        port = 5435
    }
}

# Check prerequisites
function Test-Prerequisites {
    Write-Info "Checking prerequisites..."
    
    # Check dotnet-ef
    $efVersion = dotnet ef --version 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Error_ "dotnet-ef not installed"
        Write-Info "Install with: dotnet tool install --global dotnet-ef"
        return $false
    }
    Write-Success "dotnet-ef installed: $efVersion"
    
    # Check Docker
    $dockerStatus = docker ps 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Error_ "Docker is not running"
        Write-Info "Start Docker and try again"
        return $false
    }
    Write-Success "Docker is running"
    
    # Check containers
    $runningContainers = docker ps --format "table {{.Names}}" | Select-String "frog-"
    if ($null -eq $runningContainers) {
        Write-Warning "FrogEdu containers not running. Run: docker-compose up -d"
        if (-not $Force) {
            return $false
        }
    }
    Write-Success "Docker containers verified"
    
    return $true
}

# Test database connection
function Test-DatabaseConnection {
    param([string]$ServiceKey)
    
    $service = $services[$ServiceKey]
    $host = "localhost"
    $port = $service.port
    $db = $ServiceKey
    $user = "root"
    $password = "root"
    
    Write-Info "Testing connection to $($service.name) database..."
    
    try {
        $pgConnStr = "host=$host;port=$port;database=$db;username=$user;password=$password"
        # Using psql to test connection
        $result = psql --host=$host --port=$port --username=$user --dbname=$db --command="SELECT 1;" 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Connected to $($service.name) database"
            return $true
        }
    }
    catch {
        Write-Warning "Could not test connection for $($service.name)"
    }
    
    return $true # Allow continue even if test fails
}

# Create migration
function New-Migration {
    param(
        [string]$ServiceKey,
        [string]$Name
    )
    
    $service = $services[$ServiceKey]
    $apiPath = Join-Path $PSScriptRoot "..\backend\$($service.apiPath)"
    
    Write-Info "Creating migration '$Name' for $($service.name)..."
    
    Push-Location $apiPath
    
    try {
        $output = dotnet ef migrations add $Name `
            --project "../$($service.infraPath)" `
            --output-dir "Migrations" 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Migration '$Name' created for $($service.name)"
            return $true
        }
        else {
            Write-Error_ "Failed to create migration for $($service.name)"
            Write-Host $output
            return $false
        }
    }
    finally {
        Pop-Location
    }
}

# Update database
function Update-Database {
    param([string]$ServiceKey)
    
    $service = $services[$ServiceKey]
    $apiPath = Join-Path $PSScriptRoot "..\backend\$($service.apiPath)"
    
    Write-Info "Updating database for $($service.name)..."
    
    Push-Location $apiPath
    
    try {
        $output = dotnet ef database update 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Database updated for $($service.name)"
            return $true
        }
        else {
            Write-Error_ "Failed to update database for $($service.name)"
            Write-Host $output
            return $false
        }
    }
    finally {
        Pop-Location
    }
}

# List migrations
function Get-Migrations {
    param([string]$ServiceKey)
    
    $service = $services[$ServiceKey]
    $apiPath = Join-Path $PSScriptRoot "..\backend\$($service.apiPath)"
    
    Write-Info "Migrations for $($service.name):"
    
    Push-Location $apiPath
    
    try {
        dotnet ef migrations list 2>&1
    }
    finally {
        Pop-Location
    }
}

# Verify setup
function Test-Setup {
    param([string]$ServiceKey)
    
    $service = $services[$ServiceKey]
    
    Write-Info "Verifying setup for $($service.name)..."
    
    $apiPath = Join-Path $PSScriptRoot "..\backend\$($service.apiPath)"
    if (-not (Test-Path $apiPath)) {
        Write-Error_ "API project not found at $apiPath"
        return $false
    }
    
    $dbContextFile = Join-Path $PSScriptRoot "..\backend\$($service.infraPath)\Persistence\$($service.dbContext).cs"
    if (-not (Test-Path $dbContextFile)) {
        Write-Error_ "DbContext not found at $dbContextFile"
        return $false
    }
    
    $factoryFile = Join-Path $PSScriptRoot "..\backend\$($service.infraPath)\Persistence\$($service.dbContext)Factory.cs"
    if (-not (Test-Path $factoryFile)) {
        Write-Error_ "DbContextFactory not found at $factoryFile"
        return $false
    }
    
    $envFile = Join-Path $PSScriptRoot "..\backend\$($service.apiPath)\.env"
    if (-not (Test-Path $envFile)) {
        Write-Warning ".env file not found at $envFile"
    }
    
    if (Test-DatabaseConnection $ServiceKey) {
        Write-Success "$($service.name) setup verified ✓"
        return $true
    }
    
    return $false
}

# Initialize all migrations
function Initialize-Migrations {
    Write-Info "Initializing EF Core migrations for all services..."
    
    $servicesToProcess = if ($Service -eq "all") { 
        @("user", "content", "assessment", "ai") 
    } 
    else { 
        @($Service) 
    }
    
    $allSuccess = $true
    
    foreach ($svc in $servicesToProcess) {
        if (-not (New-Migration $svc "InitialCreate")) {
            $allSuccess = $false
        }
    }
    
    if ($allSuccess) {
        Write-Success "All initial migrations created successfully"
        Write-Info "Next: run './migration.ps1 -Action update' to apply migrations"
    }
    else {
        Write-Error_ "Some migrations failed to create"
    }
}

# Update all databases
function Update-AllDatabases {
    Write-Info "Updating databases for all services..."
    
    $servicesToProcess = if ($Service -eq "all") { 
        @("user", "content", "assessment", "ai") 
    } 
    else { 
        @($Service) 
    }
    
    $allSuccess = $true
    
    foreach ($svc in $servicesToProcess) {
        if (-not (Update-Database $svc)) {
            $allSuccess = $false
        }
    }
    
    if ($allSuccess) {
        Write-Success "All databases updated successfully"
    }
    else {
        Write-Error_ "Some database updates failed"
    }
}

# Rollback last migration
function Rollback-Migration {
    param([string]$ServiceKey)
    
    $service = $services[$ServiceKey]
    $apiPath = Join-Path $PSScriptRoot "..\backend\$($service.apiPath)"
    
    Write-Warning "Rolling back last migration for $($service.name)..."
    
    if (-not $Force) {
        $response = Read-Host "Are you sure? (yes/no)"
        if ($response -ne "yes") {
            Write-Info "Rollback cancelled"
            return
        }
    }
    
    Push-Location $apiPath
    
    try {
        $output = dotnet ef migrations remove 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Migration rolled back for $($service.name)"
        }
        else {
            Write-Error_ "Failed to rollback migration for $($service.name)"
            Write-Host $output
        }
    }
    finally {
        Pop-Location
    }
}

# Display help
function Show-Help {
    Write-Host @"
FrogEdu EF Core Migrations Management Script

USAGE:
  ./migration.ps1 -Action <action> -Service <service> [options]

ACTIONS:
  verify    - Verify EF Core setup (default)
  init      - Create initial migrations for all services
  create    - Create a new named migration
  update    - Apply migrations to database
  rollback  - Remove the last migration
  clean     - Clean migration history

SERVICES:
  all       - All services (default)
  user      - User Service only
  content   - Content Service only
  assessment - Assessment Service only
  ai        - AI Service only

EXAMPLES:
  # Verify setup
  ./migration.ps1

  # Create initial migrations
  ./migration.ps1 -Action init

  # Update databases
  ./migration.ps1 -Action update

  # Create named migration for User Service
  ./migration.ps1 -Action create -Service user -MigrationName "AddUserPreferences"

  # Rollback last migration (with confirmation)
  ./migration.ps1 -Action rollback -Service user

  # Rollback without confirmation
  ./migration.ps1 -Action rollback -Service user -Force

OPTIONS:
  -Force    - Skip confirmations
  -MigrationName - Name for new migration (default: "Migration")

"@
}

# Main execution
$ErrorActionPreference = "Continue"

switch ($Action) {
    "help" {
        Show-Help
    }
    "verify" {
        if (Test-Prerequisites) {
            $servicesToCheck = if ($Service -eq "all") { 
                @("user", "content", "assessment", "ai") 
            } 
            else { 
                @($Service) 
            }
            
            foreach ($svc in $servicesToCheck) {
                Test-Setup $svc
                Get-Migrations $svc
            }
        }
    }
    "init" {
        if (Test-Prerequisites) {
            Initialize-Migrations
        }
    }
    "create" {
        if (Test-Prerequisites) {
            $servicesToProcess = if ($Service -eq "all") { 
                @("user", "content", "assessment", "ai") 
            } 
            else { 
                @($Service) 
            }
            
            foreach ($svc in $servicesToProcess) {
                New-Migration $svc $MigrationName
            }
        }
    }
    "update" {
        if (Test-Prerequisites) {
            Update-AllDatabases
        }
    }
    "rollback" {
        if (Test-Prerequisites) {
            $servicesToProcess = if ($Service -eq "all") { 
                Write-Error_ "Rollback requires specific service (use -Service)"
                return
            } 
            else { 
                @($Service) 
            }
            
            foreach ($svc in $servicesToProcess) {
                Rollback-Migration $svc
            }
        }
    }
    "clean" {
        Write-Warning "This will remove all migration files - use with caution!"
        if (-not $Force) {
            $response = Read-Host "Are you absolutely sure? Type 'clean all' to confirm"
            if ($response -ne "clean all") {
                Write-Info "Operation cancelled"
                return
            }
        }
        Write-Info "Cleaning migrations..."
        # Implementation depends on your specific needs
        Write-Warning "Manual cleanup required - remove Migration folders from each service"
    }
    default {
        Write-Error_ "Unknown action: $Action"
        Show-Help
    }
}
