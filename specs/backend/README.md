# Backend AI-DLC Specification Index

**Project:** Edu-AI Classroom - Backend Services  
**Version:** 2.0  
**Last Updated:** January 13, 2026  
**Architecture:** Direct HTTP/gRPC Communication (No RabbitMQ/Event-Driven)

---

## 📋 Quick Navigation

### Core Architecture

- **[00-architecture-overview.md](./00-architecture-overview.md)** - System architecture, Clean Architecture layers, technology stack
- **[backend-spec.md](./backend-spec.md)** - Complete backend technical specification (comprehensive reference)

### Service Specifications

Each service has a detailed spec with:

- ✅ Domain model (Aggregates, Entities, Value Objects)
- ✅ Database schema with migrations
- ✅ API endpoints (REST + gRPC contracts)
- ✅ Business rules and validation
- ✅ Integration points with other services
- ✅ Granular implementation tasks with checkboxes
- ✅ Acceptance criteria and validation

#### Service Specs:

1. **[01-content-service.md](./01-content-service.md)** - Content Service ✅
   - Textbook catalog management
   - Chapter and page organization
   - S3 presigned URL generation
2. **[02-user-service.md](./02-user-service.md)** - User Service
   - User authentication (AWS Cognito integration)
   - Profile management
   - Class management (teacher/student relationships)
3. **[03-assessment-service.md](./03-assessment-service.md)** - Assessment Service
   - Question bank management
   - Exam matrix and generation
   - PDF export to S3
4. **[04-ai-service.md](./04-ai-service.md)** - AI Orchestrator Service
   - LLM integration (OpenAI/Anthropic)
   - RAG pipeline for Socratic tutoring
   - Conversation history management

---

## 🏗️ System Architecture

### Service Communication

```
Frontend (React)
    ↓ HTTPS
API Gateway (YARP)
    ↓
┌─────────────┬─────────────┬─────────────┬─────────────┐
│   Content   │    User     │ Assessment  │     AI      │
│   Service   │   Service   │   Service   │  Service    │
│   :5001     │   :5003     │   :5002     │   :5004     │
└──────┬──────┴──────┬──────┴──────┬──────┴──────┬──────┘
       │             │              │             │
       │             │              ├─────gRPC────┤
       │             │              │   (sync)    │
       └─────────────┴──────────────┴─────────────┘
                            │
       ┌────────────────────┴────────────────────┐
       │                                         │
   SQL Server (4 DBs)                        AWS S3
   - ContentDB                         (Textbooks, Exams)
   - UserDB
   - AssessmentDB
   - AiContextDB
```

**Key Decisions:**

- ✅ **No RabbitMQ/Event Bus** - Direct HTTP/gRPC calls between services
- ✅ **Database-per-Service** - Each service owns its data
- ✅ **gRPC for Internal Calls** - Assessment → Content, AI → Content
- ✅ **REST for Frontend** - All frontend calls via API Gateway
- ✅ **S3 for Storage** - Textbook pages, generated exams, avatars

---

## 💻 Technology Stack

### Backend Core

- **Framework:** .NET 9, ASP.NET Core
- **Architecture:** Clean Architecture + DDD
- **Language:** C# 12 with nullable reference types
- **Database:** SQL Server + Entity Framework Core 9
- **API:** REST (OpenAPI/Swagger) + gRPC
- **Validation:** FluentValidation
- **Mapping:** AutoMapper
- **Logging:** Serilog + OpenTelemetry

### External Services

- **Authentication:** AWS Cognito (OAuth 2.0/OIDC)
- **Storage:** AWS S3 SDK
- **AI:** OpenAI API / Anthropic Claude API
- **Observability:** OpenTelemetry, Application Insights

### Development Tools

- **Testing:** xUnit, Moq, FluentAssertions
- **API Testing:** Postman, REST Client
- **Database:** SQL Server Management Studio, Azure Data Studio
- **Containerization:** Docker

---

## 📝 Clean Architecture Standards

### Layer Structure (Mandatory for All Services)

```csharp
// Domain Layer - No dependencies
namespace FrogEdu.{Service}.Domain
{
    public class {Aggregate} : Entity, IAggregateRoot
    {
        // Business logic, invariants, domain events
    }
}

// Application Layer - Depends on Domain
namespace FrogEdu.{Service}.Application
{
    public class {Feature}Command : IRequest<Result<{Response}>>
    {
        // Command properties
    }

    public class {Feature}CommandHandler : IRequestHandler<{Feature}Command, Result<{Response}>>
    {
        // Use case orchestration
    }
}

// Infrastructure Layer - Depends on Domain + Application
namespace FrogEdu.{Service}.Infrastructure
{
    public class {Service}DbContext : DbContext
    {
        // EF Core configuration
    }

    public class {Aggregate}Repository : I{Aggregate}Repository
    {
        // Data access implementation
    }
}

// API Layer - Depends on Application
namespace FrogEdu.{Service}.API
{
    [ApiController]
    [Route("api/{service}/{resource}")]
    public class {Resource}Controller : ControllerBase
    {
        // HTTP endpoints
    }
}
```

### Dependency Rules

```
API → Application → Domain ← Infrastructure
  ↓                              ↓
  └──────────────────────────────┘
```

**MUST:**

- [ ] Domain has zero dependencies (pure business logic)
- [ ] Application depends only on Domain
- [ ] Infrastructure depends on Domain + Application (implements interfaces)
- [ ] API depends on Application (calls handlers, returns DTOs)

**MUST NOT:**

- ❌ Domain reference Application, Infrastructure, or API
- ❌ Application reference Infrastructure or API
- ❌ Return domain entities from API (use DTOs)

---

## 🎯 Coding Standards Summary

### C# Standards

**MUST Follow:**

- [ ] Nullable reference types enabled (`<Nullable>enable</Nullable>`)
- [ ] Async/await for all I/O operations
- [ ] `CancellationToken` in all async methods
- [ ] CQRS pattern (Commands vs Queries)
- [ ] `Result<T>` or `OneOf<Success, Error>` for error handling
- [ ] DTOs for API requests/responses (never domain entities)
- [ ] FluentValidation for all input validation
- [ ] `ILogger<T>` with structured logging
- [ ] `IOptions<T>` pattern for configuration
- [ ] Constructor injection only (no service locator)

**MUST NOT:**

- ❌ Use `Task.Result` or `.Wait()` (deadlocks)
- ❌ Catch generic `Exception` without rethrowing
- ❌ Hardcode connection strings or secrets
- ❌ Use static mutable state
- ❌ Create `new` instances of services (inject them)

### Database Standards

**MUST Follow:**

- [ ] EF Core migrations for all schema changes
- [ ] Indexes on foreign keys and query predicates
- [ ] SQL constraints (UNIQUE, CHECK, FK)
- [ ] Soft delete with `IsDeleted` flag + global query filter
- [ ] Audit fields: `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy`
- [ ] Optimistic concurrency with `RowVersion`
- [ ] Connection resilience with retry logic
- [ ] `.AsNoTracking()` for read-only queries
- [ ] `.Include()` for eager loading (avoid N+1)
- [ ] `.Select()` projection to DTOs

### AWS S3 Standards

**Bucket Structure:**

```
edu-classroom-assets/
├── textbooks/
│   └── {textbookId}/
│       ├── pages/
│       │   └── page-{number}.pdf
│       └── cover.jpg
├── exams/
│   └── {examId}/
│       ├── exam.pdf
│       └── answer-key.pdf
├── avatars/
│   └── {userId}/
│       └── avatar.{ext}
└── temp/              # Auto-expire 24h
```

**MUST Follow:**

- [ ] Presigned URLs (15-min expiry) for frontend access
- [ ] Server-side encryption (SSE-S3)
- [ ] Lifecycle policies (auto-delete temp files)
- [ ] Retry logic (exponential backoff)
- [ ] Content-Type headers set correctly
- [ ] Object naming with GUIDs (never user input)

---

## 🔄 Service Communication Patterns

### 1. REST API (Frontend → Service)

```csharp
// API Controller
[ApiController]
[Route("api/[controller]")]
public class TextbooksController : ControllerBase
{
    private readonly IMediator _mediator;

    [HttpGet]
    [ProducesResponseType(typeof(List<TextbookDto>), 200)]
    public async Task<IActionResult> GetTextbooks(
        [FromQuery] GetTextbooksQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
```

### 2. gRPC (Service → Service)

```protobuf
// content.proto
syntax = "proto3";

service ContentService {
  rpc GetChapter (GetChapterRequest) returns (ChapterResponse);
  rpc GetPages (GetPagesRequest) returns (PagesResponse);
}

message GetChapterRequest {
  string chapter_id = 1;
}

message ChapterResponse {
  string id = 1;
  string title = 2;
  int32 chapter_number = 3;
  repeated PageDto pages = 4;
}
```

```csharp
// gRPC Client (in Assessment Service)
public class ContentGrpcClient : IContentClient
{
    private readonly ContentService.ContentServiceClient _client;

    public async Task<ChapterDto> GetChapterAsync(string chapterId, CancellationToken ct)
    {
        var request = new GetChapterRequest { ChapterId = chapterId };
        var response = await _client.GetChapterAsync(request, cancellationToken: ct);
        return MapToDto(response);
    }
}
```

### 3. Circuit Breaker (Polly)

```csharp
// Configure resilience
services.AddHttpClient<IContentClient, ContentHttpClient>()
    .AddTransientHttpErrorPolicy(policy =>
        policy.WaitAndRetryAsync(3, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
    .AddTransientHttpErrorPolicy(policy =>
        policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
```

---

## 🧪 Testing Strategy

### Test Pyramid

```
        ╱╲
       ╱  ╲   E2E Tests (10%)
      ╱────╲
     ╱      ╲  Integration Tests (30%)
    ╱────────╲
   ╱          ╲ Unit Tests (60%)
  ╱────────────╲
```

### Unit Tests (xUnit + Moq)

```csharp
public class CreateTextbookCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_CreatesTextbook()
    {
        // Arrange
        var repository = new Mock<ITextbookRepository>();
        var handler = new CreateTextbookCommandHandler(repository.Object);
        var command = new CreateTextbookCommand { Title = "Math Grade 5" };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        repository.Verify(r => r.AddAsync(It.IsAny<Textbook>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
```

### Integration Tests (WebApplicationFactory)

```csharp
public class TextbooksControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    [Fact]
    public async Task GetTextbooks_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/textbooks");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var textbooks = await response.Content.ReadFromJsonAsync<List<TextbookDto>>();
        textbooks.Should().NotBeNull();
    }
}
```

---

## ✅ Implementation Checklist Template

Use this for every service implementation:

### Setup Phase

- [ ] Create solution structure (Domain, Application, Infrastructure, API)
- [ ] Configure project references
- [ ] Setup NuGet packages
- [ ] Configure appsettings.json
- [ ] Setup DbContext with connection string

### Domain Layer

- [ ] Define aggregate roots
- [ ] Define entities and value objects
- [ ] Implement domain events (if needed)
- [ ] Define repository interfaces
- [ ] Define domain exceptions
- [ ] Unit tests for business logic (80%+ coverage)

### Application Layer

- [ ] Define DTOs
- [ ] Create commands and queries
- [ ] Implement MediatR handlers
- [ ] Create FluentValidation validators
- [ ] Configure AutoMapper profiles
- [ ] Unit tests for handlers

### Infrastructure Layer

- [ ] Configure DbContext (entity configurations)
- [ ] Create initial migration
- [ ] Implement repositories
- [ ] Configure S3 service (if needed)
- [ ] Configure gRPC clients (if needed)
- [ ] Integration tests for data access

### API Layer

- [ ] Create controllers/minimal APIs
- [ ] Configure Swagger/OpenAPI
- [ ] Add middleware (error handling, logging)
- [ ] Configure CORS
- [ ] Configure authentication/authorization
- [ ] API integration tests

### Deployment

- [ ] Dockerfile created
- [ ] Environment variables configured
- [ ] Health check endpoints
- [ ] Logging configured (Serilog + OpenTelemetry)
- [ ] Database migration strategy
- [ ] CI/CD pipeline

---

## 📊 Service Status

| Service    | Status        | Spec Complete | Domain Model | API Endpoints | Tests      | Notes           |
| ---------- | ------------- | ------------- | ------------ | ------------- | ---------- | --------------- |
| Content    | ✅ Ready      | ✅ Yes        | ✅ Yes       | ✅ Yes        | ⏳ Pending | Fully specified |
| User       | ⏳ Spec Phase | ❌ No         | ❌ No        | ❌ No         | ❌ No      | Spec needed     |
| Assessment | ⏳ Spec Phase | ❌ No         | ❌ No        | ❌ No         | ❌ No      | Spec needed     |
| AI         | ⏳ Spec Phase | ❌ No         | ❌ No        | ❌ No         | ❌ No      | Spec needed     |

---

## 🚀 Getting Started for AI Agents

### Phase 1: Before "LFG"

1. Read [00-architecture-overview.md](./00-architecture-overview.md)
2. Study Clean Architecture principles
3. Review the specific service spec you'll implement
4. Understand database schema and relationships
5. Review API contracts (REST + gRPC)

### Phase 2: After "LFG" Command

1. Start with **User Service** (foundation for auth)
2. Then **Content Service** (already has detailed spec)
3. Then **Assessment Service** (depends on Content)
4. Finally **AI Service** (depends on Content)

### Phase 3: Implementation

1. Create solution structure (4 projects per service)
2. Implement Domain layer first (pure business logic)
3. Then Application layer (handlers, DTOs)
4. Then Infrastructure layer (DbContext, repositories)
5. Finally API layer (controllers, endpoints)
6. Write tests at each layer
7. Update checkboxes as you complete tasks

### Phase 4: Validation

- [ ] All tests passing (unit + integration)
- [ ] Swagger documentation complete
- [ ] Database migrations working
- [ ] S3 integration tested
- [ ] gRPC contracts working
- [ ] API responses match DTOs
- [ ] Error handling comprehensive
- [ ] Logging implemented

---

## 📚 Additional Resources

### Documentation

- [Clean Architecture Guide](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)
- [.NET 9 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [gRPC in .NET](https://learn.microsoft.com/en-us/aspnet/core/grpc/)

### Tools

- **SQL Server Management Studio** - Database management
- **Postman** - API testing
- **gRPC UI** - gRPC testing
- **Docker Desktop** - Containerization
- **Azure Data Studio** - Cross-platform database tool

---

## 🎯 Next Steps

### Immediate Tasks:

1. ✅ Complete Content Service spec (already done)
2. ⏳ Create User Service spec (02-user-service.md)
3. ⏳ Create Assessment Service spec (03-assessment-service.md)
4. ⏳ Create AI Service spec (04-ai-service.md)

### Implementation Order:

1. User Service (auth foundation)
2. Content Service (textbook management)
3. Assessment Service (exam generation)
4. AI Service (tutoring)

---

**Remember:** These specs are living documents. Update them when you discover better approaches or missing requirements during implementation!
