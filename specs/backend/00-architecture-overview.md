# Backend Architecture Overview

**Version:** 2.0  
**Last Updated:** January 13, 2026  
**Status:** Implementation Ready ✅

---

## Table of Contents

1. [System Overview](#system-overview)
2. [Architectural Decisions](#architectural-decisions)
3. [Service Catalog](#service-catalog)
4. [Communication Patterns](#communication-patterns)
5. [Technology Stack](#technology-stack)
6. [Cross-Cutting Concerns](#cross-cutting-concerns)

---

## System Overview

The Edu-AI Classroom backend is a distributed microservices system built on **.NET 9** using **Clean Architecture** and **Domain-Driven Design (DDD)**. It powers an educational platform for Vietnamese primary schools.

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
│   API Gateway       │ (YARP / AWS API Gateway)
│  - JWT Validation   │
│  - Rate Limiting    │
│  - Routing          │
└──────┬──────────────┘
       │
       ├──→ Content Service      (Port 5001)
       ├──→ Assessment Service   (Port 5002)
       ├──→ User Service         (Port 5003)
       └──→ AI Orchestrator      (Port 5004)
              │
              ↓
       ┌─────────────┐
       │  RabbitMQ   │ (Event Bus)
       │  (MassTransit)
       └─────────────┘
              │
              ↓
       ┌─────────────┐
       │ SQL Server  │ (4 databases)
       └─────────────┘
       ┌─────────────┐
       │   AWS S3    │ (Asset storage)
       └─────────────┘
```

---

## Architectural Decisions

### Clean Architecture Layers

Each service **MUST** follow this 4-layer structure:

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
│   ├── Messaging/            # MassTransit consumers/producers
│   └── Configuration/        # Dependency injection setup
└── ServiceName.API/          # HTTP/gRPC endpoints
    ├── Controllers/          # REST API controllers
    ├── Endpoints/            # Minimal API endpoints (alternative)
    ├── Middleware/           # Error handling, logging
    ├── Filters/              # Action filters
    └── Program.cs            # Application entry point
```

**Dependency Rules:**

```
API → Application → Domain
  ↓         ↓
Infrastructure  (Can reference all layers)
```

### Database Strategy

- **Pattern:** Database-per-Service (no shared databases)
- **Technology:** SQL Server
- **Databases:**
  - `ContentDB` - Textbooks, chapters, pages
  - `AssessmentDB` - Questions, exams, matrices
  - `UserDB` - User profiles, classes, memberships
  - `AiContextDB` - Tutor sessions, conversations, prompts

### Key Design Patterns

- ✅ **CQRS**: Separate commands (write) from queries (read)
- ✅ **Repository Pattern**: Abstract data access
- ✅ **Unit of Work**: Transaction management
- ✅ **Domain Events**: Decouple domain logic from side effects
- ✅ **Result Pattern**: Return success/failure instead of throwing exceptions
- ✅ **Specification Pattern**: Encapsulate query logic

---

## Service Catalog

| Service                                          | Bounded Context       | Port | Database     | External Dependencies  |
| ------------------------------------------------ | --------------------- | ---- | ------------ | ---------------------- |
| [Content](01-content-service.md)                 | Educational materials | 5001 | ContentDB    | S3                     |
| [Assessment](02-assessment-service.md)           | Testing & evaluation  | 5002 | AssessmentDB | S3, Content (gRPC)     |
| [User](03-user-service.md)                       | Identity & profiles   | 5003 | UserDB       | Cognito                |
| [AI Orchestrator](04-ai-orchestrator-service.md) | LLM & RAG             | 5004 | AiContextDB  | OpenAI, Content (gRPC) |

**Service Autonomy Principles:**

- [ ] Each service has its own database (no shared databases)
- [ ] Each service can be deployed independently
- [ ] Each service has its own scaling configuration
- [ ] Services communicate via events (async) or gRPC (sync)
- [ ] No direct database access between services

---

## Communication Patterns

### Synchronous Communication (gRPC)

**When to use:**

- Real-time data requirements (e.g., fetching chapter details)
- Request-response semantics needed
- Low latency critical

**Example:**

```csharp
// Assessment service calls Content service via gRPC
var chapter = await _contentGrpcClient.GetChapterAsync(new ChapterRequest
{
    ChapterId = chapterId
});
```

### Asynchronous Communication (Events via MassTransit/RabbitMQ)

**When to use:**

- Fire-and-forget operations
- Cross-service data synchronization
- Long-running processes (exam PDF generation)
- Eventual consistency acceptable

**Event Naming Convention:**

- Use past tense: `TextbookUpdated`, `ExamGenerated`, `UserRegistered`
- Include timestamp and correlation ID

**Example:**

```csharp
// Content service publishes event
await _bus.Publish(new TextbookUpdatedEvent
{
    TextbookId = textbook.Id,
    Title = textbook.Title,
    IsDeleted = textbook.IsDeleted,
    OccurredAt = DateTime.UtcNow,
    CorrelationId = Guid.NewGuid()
});

// Assessment service consumes event
public class TextbookUpdatedConsumer : IConsumer<TextbookUpdatedEvent>
{
    public async Task Consume(ConsumeContext<TextbookUpdatedEvent> context)
    {
        // Update denormalized data
        await _repository.UpdateTextbookReferenceAsync(
            context.Message.TextbookId,
            context.Message.Title);
    }
}
```

---

## Technology Stack

### Core Framework

- **.NET 9** - Latest LTS with improved performance
- **C# 13** - With nullable reference types enabled

### Libraries & Packages

**Application Layer:**

- `MediatR` (12.x) - CQRS/Mediator pattern
- `FluentValidation` (11.x) - Input validation
- `AutoMapper` (13.x) - Object mapping

**Infrastructure Layer:**

- `EntityFrameworkCore` (9.x) - ORM
- `EntityFrameworkCore.SqlServer` - SQL Server provider
- `MassTransit` (8.x) - Messaging abstraction
- `MassTransit.RabbitMQ` - RabbitMQ integration
- `AWSSDK.S3` (3.x) - S3 client
- `Grpc.AspNetCore` (2.x) - gRPC support

**API Layer:**

- `Swashbuckle.AspNetCore` (6.x) - OpenAPI/Swagger
- `Microsoft.AspNetCore.Authentication.JwtBearer` - JWT validation
- `Serilog.AspNetCore` (8.x) - Structured logging

**Testing:**

- `xUnit` (2.x) - Test framework
- `FluentAssertions` (6.x) - Assertion library
- `NSubstitute` (5.x) - Mocking framework
- `Microsoft.AspNetCore.Mvc.Testing` (9.x) - Integration testing

**Observability:**

- `OpenTelemetry` (1.x) - Distributed tracing
- `Serilog` - Structured logging
- `Prometheus.NET` - Metrics

---

## Cross-Cutting Concerns

### Authentication & Authorization

**Provider:** AWS Cognito (OAuth 2.0 / OpenID Connect)

**JWT Validation:**

```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://cognito-idp.ap-southeast-1.amazonaws.com/ap-southeast-1_XXXXX";
        options.Audience = "your-cognito-app-client-id";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(5)
        };
    });
```

**Authorization Policies:**

```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("TeacherOnly", policy =>
        policy.RequireClaim("custom:role", "Teacher"));

    options.AddPolicy("StudentOnly", policy =>
        policy.RequireClaim("custom:role", "Student"));
});
```

### Error Handling

**Global Exception Handler:**

```csharp
public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken ct)
    {
        var problemDetails = exception switch
        {
            ValidationException valEx => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Error",
                Detail = string.Join(", ", valEx.Errors)
            },
            NotFoundException notFoundEx => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource Not Found",
                Detail = notFoundEx.Message
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred."
            }
        };

        context.Response.StatusCode = problemDetails.Status.Value;
        await context.Response.WriteAsJsonAsync(problemDetails, ct);
        return true;
    }
}
```

### Logging

**Structured Logging with Serilog:**

```csharp
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "EduAI-Classroom")
    .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
```

**Logging Best Practices:**

```csharp
_logger.LogInformation(
    "Exam generated. ExamId={ExamId}, QuestionCount={QuestionCount}, Duration={Duration}ms",
    exam.Id,
    exam.Questions.Count,
    stopwatch.ElapsedMilliseconds
);
```

### Observability

**OpenTelemetry Configuration:**

```csharp
services.AddOpenTelemetry()
    .WithTracing(builder =>
    {
        builder
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .AddSource("MassTransit")
            .AddJaegerExporter(options =>
            {
                options.AgentHost = "jaeger";
                options.AgentPort = 6831;
            });
    });
```

---

## Non-Functional Requirements

### Performance Targets

- **API Response Time:** < 500ms (p95)
- **Exam Generation:** < 10 seconds
- **AI Tutor Response:** < 5 seconds (streaming)
- **Database Query:** < 100ms (p95)

### Scalability

- **Concurrent Users:** 10,000 per service
- **Horizontal Scaling:** Auto-scale based on CPU/memory
- **Database Connections:** Pool size 100 per service instance

### Availability

- **Target Uptime:** 99.5% (43 hours downtime/year)
- **Scheduled Maintenance:** Allowed during off-peak hours
- **Health Checks:** Every 30 seconds

### Security

- [ ] All API endpoints require JWT authentication
- [ ] Role-based access control (RBAC) enforced
- [ ] SQL injection prevention (parameterized queries)
- [ ] XSS prevention (output encoding)
- [ ] HTTPS only (TLS 1.3)
- [ ] Rate limiting (100 req/min per user)
- [ ] API keys encrypted at rest
- [ ] PII data encrypted in database

---

## Development Workflow

### Local Development Setup

1. **Prerequisites:**

   ```bash
   # Install .NET 9 SDK
   dotnet --version  # Should be 9.x

   # Install Docker Desktop
   docker --version

   # Install SQL Server Management Studio (optional)
   ```

2. **Start Infrastructure:**

   ```bash
   cd backend
   docker-compose up -d  # Starts SQL, RabbitMQ, LocalStack
   ```

3. **Run Services:**

   ```bash
   # Terminal 1: Content Service
   cd src/Services/Content/Content.API
   dotnet run

   # Terminal 2: Assessment Service
   cd src/Services/Assessment/Assessment.API
   dotnet run

   # Terminal 3: User Service
   cd src/Services/User/User.API
   dotnet run

   # Terminal 4: AI Orchestrator
   cd src/Services/AI/AI.API
   dotnet run
   ```

4. **Access Services:**
   - API Gateway: http://localhost:5000
   - Swagger Docs: http://localhost:5001/swagger (per service)
   - RabbitMQ Management: http://localhost:15672
   - SQL Server: localhost:1433

### Git Workflow

**Branch Strategy:**

- `main` - Production-ready code
- `develop` - Integration branch
- `feature/<service>-<feature-name>` - Feature branches

**Commit Message Convention:**

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**

- `feat` - New feature
- `fix` - Bug fix
- `docs` - Documentation
- `refactor` - Code refactoring
- `test` - Adding tests
- `chore` - Build/config changes

**Example:**

```
feat(assessment): implement exam generation algorithm

- Add weighted random selection for questions
- Validate matrix distribution against available questions
- Emit ExamGenerated event after creation

Closes #42
```

---

## Testing Strategy

### Unit Tests (80% coverage minimum)

- Test Domain entities and business logic
- Test Application handlers
- Mock all dependencies

### Integration Tests

- Test API endpoints end-to-end
- Use in-memory database (SQL Server)
- Verify database operations

### Contract Tests

- Test gRPC service contracts
- Test event schemas (publish/consume)

### Load Tests

- Use k6 or JMeter
- Simulate 10,000 concurrent users
- Verify SLA compliance

---

## Deployment Architecture

### Containerization (Docker)

Each service runs in its own container:

- Base image: `mcr.microsoft.com/dotnet/aspnet:9.0`
- Multi-stage builds (build + runtime)
- Health check endpoints

### Orchestration (Kubernetes recommended)

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: content-service
spec:
  replicas: 3
  selector:
    matchLabels:
      app: content-service
  template:
    metadata:
      labels:
        app: content-service
    spec:
      containers:
        - name: content-api
          image: frogedu/content-service:latest
          ports:
            - containerPort: 5001
          env:
            - name: ASPNETCORE_ENVIRONMENT
              value: "Production"
          livenessProbe:
            httpGet:
              path: /health
              port: 5001
            initialDelaySeconds: 30
            periodSeconds: 10
```

---

## Next Steps

Read the individual service specifications:

1. [Content Service](01-content-service.md)
2. [Assessment Service](02-assessment-service.md)
3. [User Service](03-user-service.md)
4. [AI Orchestrator Service](04-ai-orchestrator-service.md)
5. [Infrastructure Setup](05-infrastructure-setup.md)
6. [API Gateway](06-api-gateway.md)
7. [Shared Kernel](07-shared-kernel.md)
