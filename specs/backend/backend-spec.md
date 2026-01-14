# Backend Architecture Overview: Edu-AI Classroom

**Version:** 2.0  
**Last Updated:** January 14, 2026  
**Status:** Implementation Ready ✅

---

## Table of Contents

1. [Overview](#1-overview)
2. [Global Architecture](#2-global-architecture)
3. [Service Boundaries](#3-service-boundaries)
4. [Infrastructure Components](#4-infrastructure-components)
5. [Technical Standards](#5-technical-standards)
6. [Development Guidelines](#6-development-guidelines)
7. [Security & Authentication](#7-security--authentication)
8. [Testing Strategy](#8-testing-strategy)

---

## 1. Overview

The backend is a distributed microservices system built on **.NET 9** using **Clean Architecture** and **Domain-Driven Design (DDD)**. It powers the Edu-AI Classroom platform for Vietnamese primary education.

### Key Architectural Decisions

- **Pattern:** Microservices (Service-oriented)
- **Database Strategy:** Database-per-Service (SQL Server)
- **Communication:** Direct HTTP/REST + gRPC (internal service-to-service)
- **Authentication:** AWS Cognito (OAuth 2.0 / OpenID Connect)
- **Storage:** AWS S3 (presigned URLs)
- **Observability:** OpenTelemetry + Serilog
- **API Gateway:** AWS API Gateway

### Non-Functional Requirements

- **Availability:** 99.5% uptime (scheduled maintenance windows allowed)
- **Performance:**
  - API response time: < 500ms (p95)
  - Exam generation: < 10 seconds
  - AI Tutor response: < 5 seconds (streaming)
- **Scalability:** Support 10,000 concurrent users per service
- **Data Retention:** 7 years (compliance with Vietnamese education regulations)

---

## 2. Global Architecture

### System Context Diagram

```
┌─────────────┐
│   Frontend  │ (React/Vite)
│  (Cloudflare│
│    Pages)   │
└──────┬──────┘
       │ HTTPS
       ↓
┌─────────────────────┐
│   API Gateway       │ (AWS API Gateway)
│  - JWT Validation   │
│  - Rate Limiting    │
│  - Routing          │
└──────┬──────────────┘
       │
       ├──→ Content Service      (Port 5001)
       │     │
       │     ├──→ ContentDB (SQL Server)
       │     └──→ AWS S3
       │
       ├──→ Assessment Service   (Port 5002)
       │     │
       │     ├──→ AssessmentDB (SQL Server)
       │     ├──→ AWS S3
       │     └──→ Content Service (gRPC)
       │
       ├──→ User Service         (Port 5003)

       │     └──→ AWS Cognito
       │
       └──→ AI Orchestrator      (Port 5004)
             │
             ├──→ AiContextDB (SQL Server)
             ├──→ OpenAI API
             └──→ Content Service (gRPC)
```

---

## 3. Service Boundaries

Each service is **autonomous** with its own:

- Database (no shared databases)
- Deployment pipeline
- Scaling configuration
- Team ownership

### Service Catalog

| Service         | Responsibility                       | Port | Database     | External Dependencies      |
| --------------- | ------------------------------------ | ---- | ------------ | -------------------------- |
| Content         | Textbook management, lesson assets   | 5001 | ContentDB    | S3                         |
| Assessment      | Question banks, exam generation      | 5002 | AssessmentDB | S3, Content (gRPC)         |
| User            | Profile management, class enrollment | 5003 | UserDB       | Cognito                    |
| AI Orchestrator | LLM integration, RAG pipeline        | 5004 | AiContextDB  | OpenAI API, Content (gRPC) |

### Cross-Service Data Consistency

**Challenge:** Microservices have denormalized data (e.g., Assessment service stores `TextbookId` but doesn't own Textbooks).

**Solution: Direct gRPC Calls + Local Caching**

```csharp
// Assessment service validates textbook existence via gRPC
public class QuestionService
{
    private readonly ContentServiceClient _contentClient;
    private readonly IMemoryCache _cache;

    public async Task<Result<Question>> CreateQuestionAsync(
        CreateQuestionCommand command,
        CancellationToken ct)
    {
        // Validate textbook exists via gRPC call (with caching)
        var cacheKey = $"textbook:{command.TextbookId}";
        var textbook = await _cache.GetOrCreateAsync(
            cacheKey,
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
                return await _contentClient.GetTextbookAsync(
                    new GetTextbookRequest { Id = command.TextbookId.ToString() },
                    cancellationToken: ct);
            });

        if (textbook == null || textbook.IsDeleted)
        {
            return Result.Failure<Question>("Textbook not found or inactive");
        }

        // Proceed with question creation
        var question = Question.Create(command.Text, command.Difficulty, textbook.Id);
        await _repository.AddAsync(question, ct);
        return Result.Success(question);
    }
}
```

---

## 4. Infrastructure Components

### 4.1 API Gateway (AWS API Gateway)

**Configuration:**

AWS API Gateway serves as the single entry point for all client requests, providing:

- **Request routing** to appropriate microservices
- **JWT validation** via AWS Cognito authorizer
- **Rate limiting** and throttling
- **CORS configuration**
- **Request/response transformation**
- **API versioning**

**Route Configuration (AWS Console/Terraform):**

```yaml
# API Gateway REST API Configuration
Routes:
  - Path: /api/content/*
    Method: ANY
    Integration: HTTP_PROXY
    Backend: http://content-service:5001
    Authorizer: CognitoAuthorizer

  - Path: /api/assessment/*
    Method: ANY
    Integration: HTTP_PROXY
    Backend: http://assessment-service:5002
    Authorizer: CognitoAuthorizer

  - Path: /api/users/*
    Method: ANY
    Integration: HTTP_PROXY
    Backend: http://user-service:5003
    Authorizer: CognitoAuthorizer

  - Path: /api/ai/*
    Method: ANY
    Integration: HTTP_PROXY
    Backend: http://ai-service:5004
    Authorizer: CognitoAuthorizer

Settings:
  - Throttle: 1000 requests/second
  - Burst: 2000 requests
  - Rate Limit Per User: 100 requests/minute
  - CORS:
      AllowOrigins: ["https://app.frogedu.com"]
      AllowMethods: ["GET", "POST", "PUT", "DELETE", "OPTIONS"]
      AllowHeaders: ["Content-Type", "Authorization"]
```

**Cognito Authorizer Configuration:**

```json
{
  "type": "COGNITO_USER_POOLS",
  "name": "CognitoAuthorizer",
  "identitySource": "method.request.header.Authorization",
  "providerARNs": ["arn:aws:cognito-idp:region:account-id:userpool/pool-id"]
}
```

### 4.2 gRPC Service-to-Service Communication

**Configuration:**

```csharp
// Content Service - Server Configuration (Program.cs)
services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.MaxReceiveMessageSize = 5 * 1024 * 1024; // 5MB
});

services.AddGrpcReflection(); // For debugging with tools like grpcurl

app.MapGrpcService<ContentGrpcService>();
app.MapGrpcReflectionService(); // Enable reflection in development
```

```csharp
// Assessment Service - Client Configuration
services.AddGrpcClient<ContentService.ContentServiceClient>(options =>
{
    options.Address = new Uri(configuration["Services:Content:GrpcUrl"]!);
})
.AddPolicyHandler(GetRetryPolicy())
.AddPolicyHandler(GetCircuitBreakerPolicy());

// Polly resilience policies
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
}
```

**Proto Definition Example:**

```protobuf
syntax = "proto3";

package content;

service ContentService {
  rpc GetTextbook (GetTextbookRequest) returns (TextbookResponse);
  rpc GetChapter (GetChapterRequest) returns (ChapterResponse);
  rpc GetPages (GetPagesRequest) returns (PagesResponse);
}

message GetTextbookRequest {
  string id = 1;
}

message TextbookResponse {
  string id = 1;
  string title = 2;
  string subject = 3;
  int32 grade_level = 4;
  bool is_deleted = 5;
}
```

### 4.3 AWS S3 Integration

**Bucket Structure:**

```
edu-classroom-assets/
├── textbooks/
│   └── {textbookId}/
│       ├── pages/
│       └── cover.jpg
├── generated-exams/
│   └── {examId}/
│       ├── exam.pdf
│       └── answer-key.pdf
├── user-uploads/
│   └── {userId}/
│       └── avatar.jpg
└── temp/                    # Auto-expire after 24h
```

**Service Implementation:**

```csharp
public class S3StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName = "edu-classroom-assets";

    public async Task<string> UploadFileAsync(
        Stream fileStream,
        string key,
        string contentType,
        CancellationToken ct)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = fileStream,
            ContentType = contentType,
            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
        };

        await _s3Client.PutObjectAsync(request, ct);
        return $"s3://{_bucketName}/{key}";
    }

    public async Task<string> GetPresignedUrlAsync(string key, TimeSpan expiration, CancellationToken ct)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Expires = DateTime.UtcNow.Add(expiration),
            Verb = HttpVerb.GET
        };

        return _s3Client.GetPreSignedURL(request);
    }
}
```

---

## 5. Technical Standards

### 5.1 Clean Architecture Enforcement

**Dependency Rules:**

```
API → Application → Domain
  ↓         ↓
Infrastructure  (Can reference all layers)
```

**Project Structure (per service):**

```
ServiceName/
├── ServiceName.Domain/       # Core business logic (no dependencies)
│   ├── Entities/             # Aggregate roots, entities
│   ├── ValueObjects/         # Immutable value objects
│   ├── DomainEvents/         # Domain events
│   ├── Interfaces/           # Repository interfaces
│   └── Exceptions/           # Domain-specific exceptions
├── ServiceName.Application/  # Use cases & orchestration
│   ├── Commands/             # CQRS commands
│   ├── Queries/              # CQRS queries
│   ├── Handlers/             # MediatR handlers
│   ├── DTOs/                 # Data transfer objects
│   ├── Validators/           # FluentValidation
│   ├── Interfaces/           # Service interfaces
│   └── Mappers/              # AutoMapper profiles
├── ServiceName.Infrastructure/ # External concerns
│   ├── Persistence/          # EF Core DbContext, repositories
│   ├── Services/             # AWS S3, external APIs
│   └── Configuration/        # Dependency injection setup
└── ServiceName.API/          # HTTP/gRPC endpoints
    ├── Controllers/          # REST API controllers
    ├── Middleware/           # Error handling, logging
    ├── Filters/              # Action filters
    └── Program.cs            # Application entry point
```

**Anti-Patterns to Avoid:**

- ❌ Domain layer referencing Infrastructure (violates dependency inversion)
- ❌ Controllers containing business logic (should delegate to Application layer)
- ❌ Direct database access in API controllers (use repositories)
- ❌ Exposing EF Core entities in API responses (use DTOs)

### 5.2 CQRS Implementation

**Command Example:**

```csharp
// Application/Commands/CreateQuestion.cs
public record CreateQuestionCommand(
    string Content,
    QuestionType Type,
    Difficulty Difficulty,
    Guid ChapterId,
    List<QuestionOptionDto> Options
) : IRequest<Result<Guid>>;

// Application/Handlers/CreateQuestionHandler.cs
public class CreateQuestionHandler : IRequestHandler<CreateQuestionCommand, Result<Guid>>
{
    private readonly IQuestionRepository _repository;
    private readonly IValidator<CreateQuestionCommand> _validator;

    public async Task<Result<Guid>> Handle(CreateQuestionCommand command, CancellationToken ct)
    {
        // 1. Validate
        var validationResult = await _validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
            return Result.Failure<Guid>(validationResult.ToString());

        // 2. Create domain entity
        var question = Question.Create(
            command.Content,
            command.Type,
            command.Difficulty,
            command.ChapterId
        );

        // 3. Add options (for MCQ)
        foreach (var option in command.Options)
        {
            question.AddOption(option.Text, option.IsCorrect);
        }

        // 4. Persist
        await _repository.AddAsync(question, ct);
        await _repository.SaveChangesAsync(ct);

        return Result.Success(question.Id);
    }
}
```

**Query Example:**

```csharp
// Application/Queries/GetQuestionById.cs
public record GetQuestionByIdQuery(Guid Id) : IRequest<QuestionDto?>;

public class GetQuestionByIdHandler : IRequestHandler<GetQuestionByIdQuery, QuestionDto?>
{
    private readonly IQuestionRepository _repository;
    private readonly IMapper _mapper;

    public async Task<QuestionDto?> Handle(GetQuestionByIdQuery query, CancellationToken ct)
    {
        var question = await _repository.GetByIdAsync(query.Id, ct);
        return question == null ? null : _mapper.Map<QuestionDto>(question);
    }
}
```

### 5.3 Error Handling

**ProblemDetails Standard:**

```csharp
// Global exception handler middleware
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception occurred");

        var problemDetails = exception switch
        {
            NotFoundException notFound => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Detail = notFound.Message
            },
            ValidationException validation => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed",
                Detail = validation.Message
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred",
                Detail = "An unexpected error occurred. Please try again later."
            }
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
```

### 5.4 Database Standards (SQL Server + EF Core)

**MUST Follow:**

- [ ] **Migrations:** All schema changes via `dotnet ef migrations add`
- [ ] **Indexing:** Add indexes for foreign keys and query predicates
- [ ] **Constraints:** Use SQL constraints (UNIQUE, CHECK, FK) in addition to EF validation
- [ ] **Soft Delete:** Implement `IsDeleted` flag with global query filters
- [ ] **Audit Fields:** `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy` on all entities
- [ ] **Optimistic Concurrency:** Use `RowVersion` or `Timestamp` for concurrency control
- [ ] **Connection Resilience:** Enable retry logic for transient failures

**Query Optimization:**

- [ ] Use `.AsNoTracking()` for read-only queries
- [ ] Avoid N+1 queries with `.Include()` or `AsSplitQuery()`
- [ ] Project to DTOs using `.Select()` to avoid loading entire entities

**Migration Strategy:**

```bash
# Development: Create migration
dotnet ef migrations add <MigrationName> --project Infrastructure --startup-project API

# Review generated SQL
dotnet ef migrations script <FromMigration> <ToMigration> --output migration.sql

# Apply migration
dotnet ef database update --project Infrastructure --startup-project API

# Rollback to previous migration
dotnet ef database update <PreviousMigrationName>
```

### 5.5 C# Coding Standards

**MUST Follow:**

- [ ] **Nullable Reference Types:** Enable `<Nullable>enable</Nullable>` in all projects
- [ ] **Async/Await:** All I/O operations (DB, S3, HTTP) must be async
- [ ] **CancellationToken:** Pass `CancellationToken` to all async methods
- [ ] **CQRS Pattern:** Separate commands (write) from queries (read)
- [ ] **Result Pattern:** Use `Result<T>` instead of throwing exceptions for business logic errors
- [ ] **Immutability:** Value objects and DTOs should be immutable (`record` types preferred)
- [ ] **Dependency Injection:** Never use `new` for services; always inject via constructor
- [ ] **Configuration:** Use `IOptions<T>` pattern for configuration binding
- [ ] **Logging:** Use `ILogger<T>` with structured logging (Serilog)
- [ ] **Validation:** FluentValidation for all input validation

**MUST NOT:**

- ❌ Use `Task.Result` or `.Wait()` (causes deadlocks)
- ❌ Catch generic `Exception` without rethrowing
- ❌ Expose EF Core entities directly in API responses
- ❌ Use static mutable state
- ❌ Hardcode connection strings or secrets

---

## 6. Development Guidelines

### 6.1 Code Review Checklist

Before merging any PR, verify:

- [ ] All tests pass (unit + integration)
- [ ] Code coverage ≥ 80%
- [ ] No new SonarQube critical/major issues
- [ ] Follows C# naming conventions (PascalCase for public, camelCase for private)
- [ ] XML documentation comments on public APIs
- [ ] Async methods have `Async` suffix
- [ ] Null checks for nullable parameters
- [ ] Proper disposal of `IDisposable` resources (`using` statements)

### 6.2 Logging Standards

**Structured Logging with Serilog:**

```csharp
_logger.LogInformation(
    "Exam generated. ExamId={ExamId}, QuestionCount={QuestionCount}, Duration={Duration}ms",
    exam.Id,
    exam.Questions.Count,
    stopwatch.ElapsedMilliseconds
);
```

**Log Levels:**

- `Trace`: Detailed diagnostic (not in production)
- `Debug`: Development insights
- `Information`: General flow (exam generated, user logged in)
- `Warning`: Recoverable issues (API rate limit approached)
- `Error`: Failures (database timeout, S3 upload failed)
- `Critical`: System-wide failures (database unavailable)

---

## 7. Security & Authentication

### 7.1 JWT Validation

**Configuration:**

```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = configuration["AWS:Cognito:Authority"];
        options.Audience = configuration["AWS:Cognito:ClientId"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
    });
```

### 7.2 Authorization Policies

```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("TeacherOnly", policy =>
        policy.RequireClaim("custom:role", "Teacher"));

    options.AddPolicy("StudentOnly", policy =>
        policy.RequireClaim("custom:role", "Student"));

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("custom:role", "Admin"));
});

// Usage in controller
[Authorize(Policy = "TeacherOnly")]
[HttpPost("classes")]
public async Task<IActionResult> CreateClass(CreateClassCommand command, CancellationToken ct)
{
    var result = await _mediator.Send(command, ct);
    return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
}
```

---

## 8. Testing Strategy

### 8.1 Unit Tests

**Guidelines:**

- Test business logic in Domain and Application layers
- Use `xUnit` as testing framework
- Use `FluentAssertions` for readable assertions
- Mock dependencies with `NSubstitute`
- Target: 80% code coverage

**Example:**

```csharp
public class QuestionTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateQuestion()
    {
        // Arrange
        var content = "What is 2 + 2?";
        var type = QuestionType.MCQ;
        var difficulty = Difficulty.Easy;

        // Act
        var question = Question.Create(content, type, difficulty, Guid.NewGuid());

        // Assert
        question.Should().NotBeNull();
        question.Content.Should().Be(content);
        question.Type.Should().Be(type);
        question.Difficulty.Should().Be(difficulty);
    }

    [Fact]
    public void AddOption_WithCorrectAnswer_ShouldSetIsCorrect()
    {
        // Arrange
        var question = Question.Create("Test?", QuestionType.MCQ, Difficulty.Easy, Guid.NewGuid());

        // Act
        question.AddOption("Option A", isCorrect: true);

        // Assert
        question.Options.Should().ContainSingle(o => o.IsCorrect);
    }
}
```

### 8.2 Integration Tests

**Setup:**

```csharp
public class AssessmentApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public AssessmentApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace real DB with in-memory
                services.RemoveAll<DbContextOptions<AssessmentDbContext>>();
                services.AddDbContext<AssessmentDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetQuestions_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/api/assessment/questions");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
```

---

## Next Steps

This overview provides the architectural foundation. For feature-specific implementation details, refer to:

- **[01-auth-user-feature.md](./01-auth-user-feature.md)** - Authentication & User Management
- **[02-class-management-feature.md](./02-class-management-feature.md)** - Class Management
- **[03-content-library-feature.md](./03-content-library-feature.md)** - Content Library
- **[04-assessment-feature.md](./04-assessment-feature.md)** - Smart Exam Generator
- **[05-ai-tutor-feature.md](./05-ai-tutor-feature.md)** - AI Student Tutor

---

**Remember:** This is a living spec. Update it if you discover missing requirements or better approaches during implementation.
