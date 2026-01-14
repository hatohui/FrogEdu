# Milestone 0: Foundation & Infrastructure Setup

**Feature:** Project Foundation  
**Epic:** Infrastructure  
**Priority:** P0 (Blocking)  
**Estimated Effort:** 6-8 hours  
**Status:** 🔄 In Progress

---

## Table of Contents

1. [Overview](#1-overview)
2. [Prerequisites](#2-prerequisites)
3. [Cloud Services & Free Tiers](#3-cloud-services--free-tiers)
4. [Implementation Tasks](#4-implementation-tasks)
5. [Infrastructure Setup](#5-infrastructure-setup)
6. [Local Development Setup](#6-local-development-setup)
7. [Validation Checklist](#7-validation-checklist)

---

## 1. Overview

This milestone establishes the foundational infrastructure for the FrogEdu serverless platform. It sets up the solution structure, shared kernel, cloud services (AWS Lambda, Cognito, Neon Postgres, Cloudflare), and development environment.

**Architecture:** Fully serverless, cloud-native platform with zero server management.

**See:** [/specs/architecture.md](../architecture.md) for complete architecture documentation.

### Goals

- ✅ Create .NET solution structure with Clean Architecture
- ✅ Implement Shared.Kernel base classes
- ✅ Provision AWS services (Lambda, API Gateway, CloudFront, Cognito)
- ✅ Set up Neon Postgres (serverless database)
- ✅ Configure Cloudflare Pages & R2
- ✅ Configure environment variables and secrets
- ✅ Set up Terraform for Infrastructure as Code

### Dependencies

- .NET 9 SDK
- AWS CLI v2
- Terraform >= 1.6
- Node.js >= 20 (for frontend)
- Cloudflare account (free tier)
- Neon account (free tier)
- Visual Studio 2022 / VS Code with C# Dev Kit

---

## 2. Prerequisites

### Local Development Requirements

```bash
# Verify installations
dotnet --version          # Should be 9.x
aws --version             # Should be 2.x
terraform --version       # Should be 1.6+
node --version            # Should be 20.x or higher

# Install .NET tools
dotnet tool install --global dotnet-ef
dotnet tool install --global Amazon.Lambda.Tools
```

### Cloud Account Setup

#### 1. AWS Account

```bash
# Sign up at aws.amazon.com/free
# Enable MFA for root account

# Create IAM user for development
aws iam create-user --user-name frogedu-dev

# Attach required policies
aws iam attach-user-policy --user-name frogedu-dev --policy-arn arn:aws:iam::aws:policy/AWSLambda_FullAccess
aws iam attach-user-policy --user-name frogedu-dev --policy-arn arn:aws:iam::aws:policy/AmazonAPIGatewayAdministrator
aws iam attach-user-policy --user-name frogedu-dev --policy-arn arn:aws:iam::aws:policy/AmazonCognitoPowerUser
aws iam attach-user-policy --user-name frogedu-dev --policy-arn arn:aws:iam::aws:policy/CloudFrontFullAccess

# Create access key
aws iam create-access-key --user-name frogedu-dev

# Configure AWS CLI
aws configure
# AWS Access Key ID: [Your access key]
# AWS Secret Access Key: [Your secret key]
# Default region name: ap-southeast-1  # Singapore
# Default output format: json
```

#### 2. Cloudflare Account

```bash
# Sign up at dash.cloudflare.com
# Create API token with permissions:
#   - Pages: Edit
#   - R2: Edit
#   - DNS: Edit (if using custom domain)

# Save token for CI/CD
export CLOUDFLARE_API_TOKEN=<your_token>
```

#### 3. Neon Account

```bash
# Sign up at neon.tech
# Create project: frogedu-production
# Note connection string (format: postgresql://user:password@ep-*.aws.neon.tech/dbname)

# Create 4 databases:
# - frogedu-users
# - frogedu-content
# - frogedu-assessments
# - frogedu-ai
```

#### 4. Google AI Studio (Gemini API)

```bash
# Visit ai.google.dev
# Create API key for Gemini 2.0 Flash
# Free tier: 1,500 requests/day

export GEMINI_API_KEY=<your_api_key>
```

---

## 3. Cloud Services & Free Tiers

### Service Comparison Table

| Service              | Provider   | Free Tier Limit                      | Expected Usage   | Monthly Cost |
| -------------------- | ---------- | ------------------------------------ | ---------------- | ------------ |
| **Lambda**           | AWS        | 1M requests, 400K GB-sec             | ~200K requests   | $0           |
| **API Gateway**      | AWS        | 1M requests/month                    | ~200K requests   | $0           |
| **CloudFront**       | AWS        | 1TB data transfer, 10M requests      | ~100GB, 200K req | $0           |
| **Cognito**          | AWS        | 50K MAUs (perpetual)                 | ~500 users       | $0           |
| **CloudWatch**       | AWS        | 5GB logs, 10 metrics                 | ~2GB logs        | $0           |
| **Neon Postgres**    | Neon       | 0.5GB storage, 1 compute (perpetual) | ~0.3GB           | $0           |
| **Cloudflare Pages** | Cloudflare | Unlimited requests (perpetual)       | ~500K/month      | $0           |
| **Cloudflare R2**    | Cloudflare | 10GB storage (perpetual)             | ~5GB             | $0           |
| **Gemini AI**        | Google     | 1,500 requests/day (perpetual)       | ~1,000/day       | $0           |

**Total Monthly Cost:** **$0** (all services within free tiers)

### Cost Estimates After Free Tier (Month 13+)

**Note:** Neon, Cloudflare, and Gemini have perpetual free tiers.

| Service         | Cost After 12 Months          |
| --------------- | ----------------------------- |
| **Lambda**      | ~$3/month (200K × 512MB × 3s) |
| **API Gateway** | ~$0.70/month (200K requests)  |
| **CloudFront**  | ~$1/month (100GB transfer)    |
| **Cognito**     | ~$0 (under 50K MAUs)          |
| **CloudWatch**  | ~$0.50/month (2GB logs)       |
| **Neon**        | $0 (perpetual free tier)      |
| **Cloudflare**  | $0 (perpetual free tier)      |
| **Gemini**      | $0 (perpetual free tier)      |

**Total After Year 1:** ~$5-6/month for 500-1000 active users

---

## 4. Implementation Tasks

### Task 0.1: Solution Structure Setup ⏸️

**Objective:** Create .NET solution with microservices projects

**Steps:**

- [ ] **0.1.1** Create solution root directory and initialize solution:

  ```bash
  cd c:\Users\hatohui\Documents\GitHub\FrogEdu\backend
  dotnet new sln -n FrogEdu
  ```

- [ ] **0.1.2** Create Shared.Kernel project (already exists, validate structure):

  ```bash
  # Validate existing project structure
  dotnet sln add src/Shared/Shared.Kernel/FrogEdu.Shared.Kernel.csproj
  ```

- [ ] **0.1.3** Create microservice projects (already scaffolded, validate):

  ```
  FrogEdu.sln
  └── src/
      ├── Services/
      │   ├── AI/
      │   │   ├── AI.API/
      │   │   ├── AI.Application/
      │   │   ├── AI.Domain/
      │   │   └── AI.Infrastructure/
      │   ├── Assessment/
      │   │   ├── Assessment.API/
      │   │   ├── Assessment.Application/
      │   │   ├── Assessment.Domain/
      │   │   └── Assessment.Infrastructure/
      │   ├── Content/
      │   │   ├── Content.API/
      │   │   ├── Content.Application/
      │   │   ├── Content.Domain/
      │   │   └── Content.Infrastructure/
      │   └── User/
      │       ├── User.API/
      │       ├── User.Application/
      │       ├── User.Domain/
      │       └── User.Infrastructure/
      └── Shared/
          └── Shared.Kernel/
  ```

- [ ] **0.1.4** Add project references (Clean Architecture dependencies):

  ```bash
  # Example: Content Service
  cd src/Services/Content/Content.Application
  dotnet add reference ../Content.Domain/FrogEdu.Content.Domain.csproj
  dotnet add reference ../../../Shared/Shared.Kernel/FrogEdu.Shared.Kernel.csproj

  cd ../Content.Infrastructure
  dotnet add reference ../Content.Application/FrogEdu.Content.Application.csproj

  cd ../Content.API
  dotnet add reference ../Content.Application/FrogEdu.Content.Application.csproj
  dotnet add reference ../Content.Infrastructure/FrogEdu.Content.Infrastructure.csproj
  ```

**Validation:**

```bash
dotnet build
# All projects should compile successfully
```

---

### Task 0.2: Shared.Kernel Implementation ⏸️

**Objective:** Implement base classes for all services

**Files to Create/Update:**

#### 0.2.1 Enhanced `Entity.cs` Base Class

```csharp
namespace FrogEdu.Shared.Kernel;

public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsDeleted { get; protected set; }

    private List<IDomainEvent>? _domainEvents;
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly() ?? new List<IDomainEvent>().AsReadOnly();

    protected Entity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    protected Entity(Guid id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public void AddDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents ??= new List<IDomainEvent>();
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

    public void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool Equals(Entity? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Entity);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity? left, Entity? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }
}
```

**Tasks:**

- [ ] **0.2.1** Update `Entity.cs` with enhanced base class
- [ ] **0.2.2** Add soft delete support (IsDeleted flag)
- [ ] **0.2.3** Add audit fields (CreatedAt, UpdatedAt)
- [ ] **0.2.4** Implement domain event collection

#### 0.2.2 Enhanced `Result<T>` Pattern

```csharp
namespace FrogEdu.Shared.Kernel;

public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, string error)
    {
        if (isSuccess && error != string.Empty)
            throw new InvalidOperationException("Cannot have a successful result with an error.");
        if (!isSuccess && error == string.Empty)
            throw new InvalidOperationException("Cannot have a failed result without an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, string.Empty);
    public static Result Failure(string error) => new(false, error);

    public static Result<T> Success<T>(T value) => new(value, true, string.Empty);
    public static Result<T> Failure<T>(string error) => new(default!, false, error);
}

public class Result<T> : Result
{
    public T Value { get; }

    protected internal Result(T value, bool isSuccess, string error)
        : base(isSuccess, error)
    {
        Value = value;
    }
}
```

**Tasks:**

- [ ] **0.2.5** Update `Result.cs` with generic result pattern
- [ ] **0.2.6** Add validation for success/failure states
- [ ] **0.2.7** Add implicit conversion operators (optional)

#### 0.2.3 Repository Interface

```csharp
namespace FrogEdu.Shared.Kernel;

public interface IRepository<T> where T : Entity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

**Tasks:**

- [ ] **0.2.8** Update `IRepository.cs` with async CRUD operations
- [ ] **0.2.9** Add cancellation token support

#### 0.2.4 ValueObject Base Class

```csharp
namespace FrogEdu.Shared.Kernel;

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public bool Equals(ValueObject? other)
    {
        if (other is null) return false;
        if (GetType() != other.GetType()) return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ValueObject);
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(1, (current, obj) =>
            {
                unchecked
                {
                    return current * 23 + (obj?.GetHashCode() ?? 0);
                }
            });
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }
}
```

**Tasks:**

- [ ] **0.2.10** Update `ValueObject.cs` with structural equality
- [ ] **0.2.11** Add GetEqualityComponents abstract method

#### 0.2.5 Domain Events

```csharp
namespace FrogEdu.Shared.Kernel;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}

public abstract record DomainEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
```

**Tasks:**

- [ ] **0.2.12** Create `IDomainEvent.cs` interface
- [ ] **0.2.13** Create `DomainEvent.cs` base record

#### 0.2.6 Common Exceptions

```csharp
namespace FrogEdu.Shared.Kernel.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}

public class ValidationException : Exception
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }
}

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}
```

**Tasks:**

- [ ] **0.2.14** Create `Exceptions/NotFoundException.cs`
- [ ] **0.2.15** Create `Exceptions/ValidationException.cs`
- [ ] **0.2.16** Create `Exceptions/DomainException.cs`

**Validation:**

```bash
cd src/Shared/Shared.Kernel
dotnet build
# Should compile with no errors
```

---

### Task 0.3: Terraform Infrastructure Setup ✅

**Objective:** Provision AWS and Cloudflare resources using Infrastructure as Code

**Directory Structure:**

```
infra/
├── provider.tf           # AWS & Cloudflare providers
├── terraform.tf          # Backend configuration
├── variables.tf          # Input variables
├── main.tf               # Main resources
├── outputs.tf            # Output values
├── modules/
│   ├── cognito/          # Cognito User Pool
│   ├── api-gateway/      # API Gateway REST API
│   ├── lambda/           # Lambda functions
│   └── cloudfront/       # CloudFront distribution
└── environments/
    ├── dev.tfvars
    └── production.tfvars
```

**Tasks:**

- [ ] **0.3.1** Create `provider.tf` with AWS and Cloudflare providers
- [ ] **0.3.2** Create Cognito User Pool module
- [ ] **0.3.3** Create API Gateway module with Cognito authorizer
- [ ] **0.3.4** Create Lambda function module (placeholder)
- [ ] **0.3.5** Create CloudFront distribution module
- [ ] **0.3.6** Initialize Terraform: `terraform init`
- [ ] **0.3.7** Apply infrastructure: `terraform apply -var-file=environments/dev.tfvars`

**Sample `modules/cognito/main.tf`:**

```hcl
resource "aws_cognito_user_pool" "frogedu" {
  name = var.user_pool_name

  password_policy {
    minimum_length    = 8
    require_lowercase = true
    require_uppercase = true
    require_numbers   = true
  }

  auto_verified_attributes = ["email"]

  schema {
    name                = "email"
    attribute_data_type = "String"
    required            = true
    mutable             = false
  }

  schema {
    name                = "role"
    attribute_data_type = "String"
    required            = false
    mutable             = true
  }
}

resource "aws_cognito_user_group" "teachers" {
  name         = "Teachers"
  user_pool_id = aws_cognito_user_pool.frogedu.id
  description  = "Teacher user group"
}

resource "aws_cognito_user_group" "students" {
  name         = "Students"
  user_pool_id = aws_cognito_user_pool.frogedu.id
  description  = "Student user group"
}
```

**Validation:**

```bash
# Verify resources created
terraform show
aws cognito-idp list-user-pools --max-results 10
```

---

### Task 0.4: Neon Database Setup ✅

**Objective:** Create serverless Postgres databases

**Steps:**

- [ ] **0.4.1** Create Neon project: `frogedu-production`
- [ ] **0.4.2** Create 4 databases:
  - `frogedu-users`
  - `frogedu-content`
  - `frogedu-assessments`
  - `frogedu-ai`
- [ ] **0.4.3** Enable connection pooling (PgBouncer)
- [ ] **0.4.4** Create database users with least privilege
- [ ] **0.4.5** Store connection strings in AWS Secrets Manager

**Connection String Format:**

```
postgresql://user:password@ep-project-id.region.aws.neon.tech/dbname?sslmode=require&pooler=true
```

**AWS Secrets Manager:**

```bash
# Store each database connection string
aws secretsmanager create-secret \
  --name frogedu/production/neon-users \
  --secret-string "postgresql://..."

aws secretsmanager create-secret \
  --name frogedu/production/neon-content \
  --secret-string "postgresql://..."
```

---

### Task 0.5: Cloudflare Setup ✅

**Objective:** Configure Pages and R2 for frontend and storage

**Steps:**

- [ ] **0.5.1** Create Cloudflare Pages project

  ```bash
  # Using Wrangler CLI
  npx wrangler pages project create frogedu-frontend
  ```

- [ ] **0.5.2** Create R2 bucket

  ```bash
  npx wrangler r2 bucket create frogedu-storage
  ```

- [ ] **0.5.3** Configure R2 CORS policy

  ```json
  {
    "AllowedOrigins": ["https://frogedu.pages.dev", "https://yourdomain.com"],
    "AllowedMethods": ["GET", "PUT", "POST", "DELETE"],
    "AllowedHeaders": ["*"],
    "MaxAgeSeconds": 3600
  }
  ```

- [ ] **0.5.4** Set up custom domain (optional)

**Validation:**

```bash
npx wrangler pages deployment list
npx wrangler r2 bucket list
```

**Tasks:**

- [ ] **0.4.1** Create `docker-compose.yml`
- [ ] **0.4.2** Create Dockerfiles for each service (placeholder for now)
- [ ] **0.4.3** Test with: `docker-compose up -d sqlserver sqlserver-init`
- [ ] **0.4.4** Verify databases created: `docker exec -it frogedu-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrongPassword123! -Q "SELECT name FROM sys.databases;"`

**Note:** Full service containers will be built after implementing each feature.

---

## 6. Local Development Setup

### Lambda Local Testing

**Using AWS SAM CLI:**

```bash
# Install SAM CLI
pip install aws-sam-cli

# Test Lambda function locally
cd backend/src/Services/User/User.API
sam local start-api --template template.yaml

# Invoke function directly
sam local invoke UserFunction --event test-event.json
```

**Using .NET Lambda Test Tool:**

```bash
# Install test tool
dotnet tool install -g Amazon.Lambda.TestTool-9.0

# Run Lambda locally with debugger
cd backend/src/Services/User/User.API
dotnet lambda-test-tool-9.0
# Opens browser at http://localhost:5050
```

### Database Migrations

**Using EF Core with Neon:**

```bash
# Install EF Core tools
dotnet tool install --global dotnet-ef

# Create initial migration (example: Content service)
cd backend/src/Services/Content/Content.Infrastructure
dotnet ef migrations add InitialCreate \
  --startup-project ../Content.API/FrogEdu.Content.API.csproj

# Apply migrations to Neon database
dotnet ef database update \
  --startup-project ../Content.API/FrogEdu.Content.API.csproj \
  --connection "postgresql://...@neon.tech/frogedu-content?sslmode=require"

# Generate SQL script (for review)
dotnet ef migrations script \
  --startup-project ../Content.API/FrogEdu.Content.API.csproj \
  -o migrations.sql
```

### Frontend Development

```bash
cd frontend
npm install
npm run dev  # Starts Vite dev server at http://localhost:5173

# Build for production
npm run build

# Preview production build
npm run preview
```

---

## 7. Validation Checklist

### Infrastructure Validation

- [ ] **AWS Services**

  - [ ] Cognito User Pool created with groups (Teachers, Students, Admins)
  - [ ] API Gateway REST API created with Cognito authorizer
  - [ ] CloudFront distribution configured with API Gateway origin
  - [ ] Lambda execution roles created with least privilege
  - [ ] CloudWatch log groups created
  - [ ] Secrets Manager configured for Neon connection strings

- [ ] **Neon Database**

  - [ ] Project created with 4 databases
  - [ ] Connection pooling enabled
  - [ ] Connection strings stored in Secrets Manager
  - [ ] Can connect from local machine

- [ ] **Cloudflare**
  - [ ] Pages project created
  - [ ] R2 bucket created with CORS policy
  - [ ] Custom domain configured (optional)

### Code Validation

- [ ] **Shared.Kernel**

  - [ ] Entity base class compiles
  - [ ] Result pattern works
  - [ ] ValueObject base class works
  - [ ] IRepository interface defined
  - [ ] Domain events infrastructure ready
  - [ ] Common exceptions defined

- [ ] **Solution Structure**
  - [ ] All service projects created
  - [ ] Project references configured correctly
  - [ ] Solution builds without errors: `dotnet build`
  - [ ] Can package Lambda deployment: `dotnet lambda package`

### Security Validation

- [ ] `.env` file added to `.gitignore`
- [ ] `.env.example` created without secrets
- [ ] Cognito password policy enforced (min 8 chars, complexity)
- [ ] Lambda execution roles follow least privilege
- [ ] Secrets stored in AWS Secrets Manager (not environment variables)
- [ ] API Gateway uses HTTPS only
- [ ] CloudFront uses SSL/TLS 1.3

### Cost Monitoring

- [ ] AWS Free Tier usage dashboard configured
- [ ] CloudWatch billing alarms set up (>$5/month)
- [ ] Neon compute auto-pause enabled (5min inactivity)
- [ ] Lambda reserved concurrency not set (use on-demand)

---

## Next Steps

Once this milestone is complete:

1. ✅ **Architecture documented** in [/specs/architecture.md](../architecture.md)
2. ✅ Start with **[01-auth-user-feature.md](./01-auth-user-feature.md)** - Authentication (Cognito-only)
3. Implement features in order (Auth → Class → Content → Assessment → AI Tutor)
4. Deploy via GitHub Actions (see CI/CD section in architecture.md)

---

## Troubleshooting

### Neon Connection Issues

```bash
# Test connection
psql "postgresql://user:password@ep-project-id.aws.neon.tech/frogedu-users?sslmode=require"

# If fails, check:
# 1. IP whitelist in Neon dashboard
# 2. Connection string format (must include ?sslmode=require)
# 3. Database name is correct
```

### Lambda Deployment Issues

```bash
# Verify Lambda package
dotnet lambda package --output-package lambda.zip
unzip -l lambda.zip  # Should contain compiled DLLs

# Test locally before deploying
dotnet lambda-test-tool-9.0

# Check Lambda logs
aws logs tail /aws/lambda/frogedu-user-function --follow
```

### Terraform Issues

```bash
# Verify AWS credentials
aws sts get-caller-identity

# Check Terraform state
terraform show

# Destroy and recreate (development only)
terraform destroy -var-file=environments/dev.tfvars
terraform apply -var-file=environments/dev.tfvars
```

### Cloudflare Deployment Issues

```bash
# Verify Wrangler auth
npx wrangler whoami

# Check Pages deployment
npx wrangler pages deployment list

# View deployment logs
npx wrangler pages deployment tail
```

---

**Milestone Status:** Ready for Implementation ✅  
**Blocked By:** None  
**Blocking:** All feature development  
**Estimated Completion:** 8-12 hours
