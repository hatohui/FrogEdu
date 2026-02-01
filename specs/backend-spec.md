# Backend Architecture Specification

## Overview

The FrogEdu backend follows a **microservices architecture** with **Domain-Driven Design (DDD)**, **Clean Architecture**, and **CQRS (Command Query Responsibility Segregation)** patterns. Each microservice is independently deployable and follows a consistent layered architecture.

## Table of Contents

1. [Architecture Principles](#architecture-principles)
2. [Shared Kernel](#shared-kernel)
3. [Layered Architecture](#layered-architecture)
4. [CQRS Pattern](#cqrs-pattern)
5. [Microservices Structure](#microservices-structure)
6. [Domain Layer](#domain-layer)
7. [Application Layer](#application-layer)
8. [Infrastructure Layer](#infrastructure-layer)
9. [API Layer](#api-layer)
10. [Cross-Cutting Concerns](#cross-cutting-concerns)
11. [Best Practices](#best-practices)

---

## Architecture Principles

### Core Principles

1. **Domain-Driven Design (DDD)**
   - Rich domain models with behavior
   - Ubiquitous language across team and code
   - Domain events for inter-aggregate communication
   - Value Objects for domain primitives

2. **Clean Architecture**
   - Dependency rule: dependencies point inward
   - Domain layer has no external dependencies
   - Business logic isolated from infrastructure
   - Framework-agnostic domain and application layers

3. **CQRS (Command Query Responsibility Segregation)**
   - Separate models for reads (Queries) and writes (Commands)
   - Commands change state, Queries return data
   - MediatR for request/response handling
   - Independent optimization of read and write operations

4. **Microservices Independence**
   - Each service owns its database
   - Service-to-service communication via HTTP/REST through API Gateway
   - No shared database across services
   - Independent deployment and scaling
   - Each microservice runs as containerized AWS Lambda function

---

## Shared Kernel

The **Shared.Kernel** project contains reusable abstractions and patterns used across all microservices.

### Location

```
backend/Shared/Shared.Kernel/
```

### Components

#### 1. Primitives (`Primitives/`)

##### Entity Base Class

```csharp
public abstract class Entity
{
    public Guid Id { get; protected set; }

    // Domain Events Support
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();
    protected void AddDomainEvent(IDomainEvent domainEvent) { /* ... */ }
    public void ClearDomainEvents() { /* ... */ }

    // Identity-based equality
    public override bool Equals(object? obj) { /* ... */ }
    public override int GetHashCode() { /* ... */ }
}
```

**Usage**: All domain entities inherit from `Entity`

- Provides unique identity (Guid)
- Domain event tracking for aggregate roots
- Identity-based equality

##### AuditableEntity Base Class

```csharp
public abstract class AuditableEntity : Entity, IAuditable
{
    public DateTime CreatedAt { get; protected set; }
    public string CreatedBy { get; protected set; } = null!;
    public DateTime? UpdatedAt { get; protected set; }
    public string? UpdatedBy { get; protected set; }

    protected void MarkAsCreated(string userId) { /* ... */ }
    protected void MarkAsUpdated(string userId) { /* ... */ }
}
```

**Usage**: Entities requiring audit trails

##### AuditableSoftdeletableEntity Base Class

```csharp
public abstract class AuditableSoftdeletableEntity : AuditableEntity, ISoftDeletable
{
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public string? DeletedBy { get; protected set; }

    public void Delete(string userId) { /* ... */ }
    public void Restore() { /* ... */ }
}
```

**Usage**: Entities requiring both audit trails and soft deletion

##### ValueObject Base Class

```csharp
public abstract class ValueObject
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj) { /* ... */ }
    public override int GetHashCode() { /* ... */ }
}
```

**Usage**: Domain primitives (Email, Money, Address, etc.)

- Immutable by design
- Structural equality based on components

##### IDomainEvent Interface

```csharp
public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}
```

**Usage**: Domain events for eventual consistency and integration

#### 2. Messaging (`Messaging/`)

##### Result Pattern

```csharp
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }

    public static Result Success() => new(true, string.Empty);
    public static Result Failure(string error) => new(false, error);
}

public class Result<T> : Result
{
    public T? Value { get; }

    public static Result<T> Success(T value) => new(true, value, string.Empty);
    public static new Result<T> Failure(string error) => new(false, default, error);
}
```

**Usage**:

- Commands return `Result` or `Result<T>`
- Avoids throwing exceptions for business rule violations
- Railway-oriented programming

#### 3. Exceptions (`Exceptions/`)

```csharp
public class DomainException : Exception { /* ... */ }
public class DomainRuleViolationException : DomainException { /* ... */ }
public class NotFoundException : Exception { /* ... */ }
public class ValidationException : Exception { /* ... */ }
```

**Usage**: Domain-specific exceptions for invariant violations

#### 4. Auditing (`Auditing/`)

```csharp
public interface IAuditable
{
    DateTime CreatedAt { get; }
    string CreatedBy { get; }
    DateTime? UpdatedAt { get; }
    string? UpdatedBy { get; }
}
```

#### 5. Deletion (`Deletion/`)

```csharp
public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    string? DeletedBy { get; }
}
```

---

## Layered Architecture

Each microservice follows a **4-layer architecture**:

```
┌─────────────────────────────────────────┐
│           API Layer                     │
│  (Controllers, Middleware, Auth)        │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│       Application Layer                 │
│  (Commands, Queries, Handlers, DTOs)    │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│         Domain Layer                    │
│  (Entities, ValueObjects, Events,       │
│   Repositories Interfaces, Services)    │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│      Infrastructure Layer               │
│  (DbContext, Repositories, External     │
│   Services, Persistence)                │
└─────────────────────────────────────────┘
```

### Dependency Rules

1. **API → Application → Domain ← Infrastructure**
2. Domain has **NO** dependencies on other layers
3. Application depends on **Domain only**
4. Infrastructure depends on **Domain only**
5. API depends on **Application** and **Infrastructure** (composition root)

---

## CQRS Pattern

### Commands (Write Operations)

**Purpose**: Mutate state, enforce business rules

**Structure**:

```
Application/
  Commands/
    {Feature}/
      {Feature}Command.cs
      {Feature}CommandHandler.cs
      {Feature}CommandValidator.cs
```

**Example: CreateUserCommand**

```csharp
// Command (Request)
public sealed record CreateUserCommand(
    string CognitoId,
    string Email,
    string FirstName,
    string LastName,
    string Role
) : IRequest<Result<Guid>>;

// Handler (Business Logic)
public sealed class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleService _roleService;

    public async Task<Result<Guid>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Validate business rules
        if (await _userRepository.ExistsAsync(request.Email))
            return Result<Guid>.Failure("User already exists");

        var role = await _roleService.GetRoleByNameAsync(request.Role);
        if (role is null)
            return Result<Guid>.Failure("Invalid role");

        // 2. Create domain entity
        var user = User.Create(
            request.CognitoId,
            request.Email,
            request.FirstName,
            request.LastName,
            role.Id
        );

        // 3. Persist
        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(user.Id);
    }
}

// Validator (Input Validation)
public sealed class CreateUserCommandValidator
    : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);
    }
}
```

**Command Characteristics**:

- Returns `Result` or `Result<T>`
- Side effects (create, update, delete)
- Triggers domain events
- Wrapped in transactions

### Queries (Read Operations)

**Purpose**: Retrieve data without side effects

**Structure**:

```
Application/
  Queries/
    {Feature}/
      {Feature}Query.cs
      {Feature}QueryHandler.cs
```

**Example: GetUserProfileQuery**

```csharp
// Query (Request)
public sealed record GetUserProfileQuery(string CognitoId)
    : IRequest<UserDto?>;

// Handler (Data Retrieval)
public sealed class GetUserProfileQueryHandler
    : IRequestHandler<GetUserProfileQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleService _roleService;

    public async Task<UserDto?> Handle(
        GetUserProfileQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByCognitoIdAsync(
            request.CognitoId,
            cancellationToken
        );

        if (user is null)
            return null;

        var role = await _roleService.GetRoleByIdAsync(user.RoleId);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email.Value,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = role?.Name ?? "Unknown",
            AvatarUrl = user.AvatarUrl,
            IsEmailVerified = user.IsEmailVerified,
            CreatedAt = user.CreatedAt
        };
    }
}
```

**Query Characteristics**:

- Returns DTOs (not domain entities)
- No side effects (idempotent)
- No domain logic
- Optimized for reads (can bypass aggregate boundaries)

---

## Microservices Structure

### Standard Structure

```
Services/
  {ServiceName}/
    dev.Dockerfile                    # Development container
    Dockerfile                        # Production container

    {ServiceName}.API/               # API Layer
      Program.cs                      # Entry point & DI configuration
      appsettings.json               # Configuration
      appsettings.Development.json   # Dev configuration
      .env.example                   # Environment variables template
      Controllers/                   # REST endpoints
      Middleware/                    # Custom middleware
      Attributes/                    # Custom attributes

    {ServiceName}.Application/       # Application Layer
      DependencyInjection.cs         # Layer registration
      Commands/                      # Write operations
        {Feature}/
          {Feature}Command.cs
          {Feature}CommandHandler.cs
          {Feature}CommandValidator.cs
      Queries/                       # Read operations
        {Feature}/
          {Feature}Query.cs
          {Feature}QueryHandler.cs
      DTOs/                          # Data Transfer Objects
      Interfaces/                    # Application services interfaces
      Behaviors/                     # MediatR pipeline behaviors

    {ServiceName}.Domain/            # Domain Layer
      Entities/                      # Aggregate roots & entities
      ValueObjects/                  # Domain primitives
      Events/                        # Domain events
      Repositories/                  # Repository interfaces
      Services/                      # Domain services interfaces
      Enums/                         # Domain enumerations

    {ServiceName}.Infrastructure/    # Infrastructure Layer
      DependencyInjection.cs         # Layer registration
      Persistence/
        {ServiceName}DbContext.cs    # EF Core DbContext
        Configurations/              # Entity configurations
      Repositories/                  # Repository implementations
      Services/                      # External service implementations
      Migrations/                    # EF Core migrations
      Storage/                       # File storage implementations
```

---

## Domain Layer

### Purpose

Contains business logic, entities, and domain rules. **Framework-agnostic**.

### Components

#### 1. Entities (`Entities/`)

**Aggregate Roots**: Entry point for business operations

```csharp
public sealed class User : AuditableSoftdeletableEntity
{
    // Value Objects for domain primitives
    public CognitoUserId CognitoId { get; private set; } = null!;
    public Email Email { get; private set; } = null!;

    // Primitive properties
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public Guid RoleId { get; private set; }
    public string? AvatarUrl { get; private set; }
    public bool IsEmailVerified { get; private set; }

    // Private constructor enforces factory method
    private User() { }

    // Factory method - enforces invariants
    public static User Create(
        string cognitoId,
        string email,
        string firstName,
        string lastName,
        Guid roleId)
    {
        var user = new User(
            CognitoUserId.Create(cognitoId),
            Email.Create(email),
            firstName,
            lastName,
            roleId
        );

        // Raise domain event
        user.AddDomainEvent(
            new UserCreatedDomainEvent(user.Id, user.Email.Value, user.RoleId)
        );

        return user;
    }

    // Behavior methods - encapsulate business logic
    public void UpdateProfile(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty");
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty");

        FirstName = firstName;
        LastName = lastName;
        MarkAsUpdated();

        AddDomainEvent(
            new UserProfileUpdatedDomainEvent(Id, firstName, lastName)
        );
    }

    public void UpdateAvatar(string avatarUrl)
    {
        if (string.IsNullOrWhiteSpace(avatarUrl))
            throw new ArgumentException("Avatar URL cannot be empty");

        AvatarUrl = avatarUrl;
        MarkAsUpdated();
        AddDomainEvent(new UserAvatarUpdatedDomainEvent(Id, avatarUrl));
    }

    public void VerifyEmail()
    {
        if (IsEmailVerified)
            return;

        IsEmailVerified = true;
    }
}
```

**Entity Design Principles**:

- Private setters protect invariants
- Factory methods for creation
- Behavior methods for mutations
- Domain events for state changes
- Rich domain model (not anemic)

#### 2. Value Objects (`ValueObjects/`)

**Immutable domain primitives** with structural equality

```csharp
public sealed partial class Email : ValueObject
{
    public string Value { get; private set; }

    private Email(string value) => Value = value;

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty");

        var normalized = value.Trim().ToLowerInvariant();

        if (!EmailRegex().IsMatch(normalized))
            throw new ArgumentException("Invalid email format");

        return new Email(normalized);
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
    private static partial Regex EmailRegex();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
```

**Value Object Principles**:

- Immutable (private setters)
- Validation in factory method
- Structural equality
- No identity
- Encapsulate domain concept

#### 3. Domain Events (`Events/`)

```csharp
public sealed record UserCreatedDomainEvent(
    Guid UserId,
    string Email,
    Guid RoleId
) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
```

**Usage**:

- Eventual consistency between aggregates
- Integration events for microservices communication
- Audit trails and event sourcing

#### 4. Repository Interfaces (`Repositories/`)

```csharp
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByCognitoIdAsync(string cognitoId, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    void Update(User user);
    void Delete(User user);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

**Repository Principles**:

- Defined in Domain, implemented in Infrastructure
- Aggregate-level operations only
- Return domain entities
- Unit of Work pattern via `SaveChangesAsync()`

#### 5. Domain Services (`Services/`)

**For business logic that doesn't belong to a single entity**

```csharp
public interface IRoleService
{
    Task<Role?> GetRoleByIdAsync(Guid roleId);
    Task<Role?> GetRoleByNameAsync(string roleName);
    Task<IReadOnlyList<Role>> GetAllRolesAsync();
}
```

---

## Application Layer

### Purpose

Orchestrates domain logic, implements use cases via CQRS.

### Components

#### 1. Commands & Handlers (`Commands/`)

See [CQRS Pattern - Commands](#commands-write-operations)

**Command Handler Responsibilities**:

1. Retrieve aggregates via repositories
2. Execute domain behavior methods
3. Persist changes via repositories
4. Return `Result` or `Result<T>`

#### 2. Queries & Handlers (`Queries/`)

See [CQRS Pattern - Queries](#queries-read-operations)

**Query Handler Responsibilities**:

1. Retrieve data via repositories or DbContext
2. Map domain entities to DTOs
3. Return DTOs (never domain entities)

#### 3. DTOs (`DTOs/`)

**Data Transfer Objects** for API contracts

```csharp
public sealed record UserDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Role { get; init; } = null!;
    public string? AvatarUrl { get; init; }
    public bool IsEmailVerified { get; init; }
    public DateTime CreatedAt { get; init; }
}

public sealed record UpdateProfileDto
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
}
```

**DTO Principles**:

- Immutable records
- No business logic
- API contract stability
- Separate from domain entities

#### 4. Validators (`Commands/{Feature}/`)

**FluentValidation** for input validation

```csharp
public sealed class CreateUserCommandValidator
    : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100);

        RuleFor(x => x.CognitoId)
            .NotEmpty().WithMessage("Cognito ID is required");
    }
}
```

#### 5. Behaviors (`Behaviors/`)

**MediatR Pipeline Behaviors** for cross-cutting concerns

```csharp
public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures);

        return await next();
    }
}
```

**Other Pipeline Behaviors**:

- Logging
- Transaction management
- Performance monitoring
- Caching

#### 6. Dependency Injection (`DependencyInjection.cs`)

```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Register MediatR with all handlers
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>)
            );
        });

        // Register FluentValidation validators
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
```

---

## Infrastructure Layer

### Purpose

Implements infrastructure concerns: persistence, external services, messaging.

### Components

#### 1. DbContext (`Persistence/`)

```csharp
public class UserDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; } = null!;

    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly()
        );
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        // Handle auditable entities
        foreach (var entry in ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.MarkAsCreated(GetCurrentUserId());
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.MarkAsUpdated(GetCurrentUserId());
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
```

#### 2. Entity Configurations (`Persistence/Configurations/`)

```csharp
public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        // Value Object mapping
        builder.Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value)
            )
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.CognitoId)
            .HasConversion(
                cognitoId => cognitoId.Value,
                value => CognitoUserId.Create(value)
            )
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.AvatarUrl)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.CognitoId).IsUnique();

        // Soft delete filter
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
```

#### 3. Repository Implementation (`Repositories/`)

```csharp
public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByCognitoIdAsync(
        string cognitoId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                u => u.CognitoId.Value == cognitoId,
                cancellationToken
            );
    }

    public async Task<bool> ExistsAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(u => u.Email.Value == email, cancellationToken);
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
```

#### 4. External Services (`Services/`)

```csharp
public interface IStorageService
{
    Task<string> GeneratePresignedUploadUrlAsync(
        string key,
        string contentType,
        int expirationMinutes = 15
    );
    Task<string> GeneratePresignedDownloadUrlAsync(
        string key,
        int expirationMinutes = 60
    );
}

public class S3StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public async Task<string> GeneratePresignedUploadUrlAsync(
        string key,
        string contentType,
        int expirationMinutes = 15)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            ContentType = contentType
        };

        return _s3Client.GetPreSignedURL(request);
    }
}
```

#### 5. Dependency Injection (`DependencyInjection.cs`)

```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("UserDb");
        services.AddDbContext<UserDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(3);
                    npgsqlOptions.CommandTimeout(30);
                }
            );
        });

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        // Domain Services
        services.AddScoped<IRoleService, RoleService>();

        // Infrastructure Services
        services.AddScoped<IStorageService, S3StorageService>();

        // AWS Services
        services.AddAWSService<IAmazonS3>();

        return services;
    }
}
```

---

## API Layer

### Purpose

HTTP endpoints, authentication, authorization, request/response handling.

### Components

#### 1. Program.cs (Entry Point)

```csharp
var builder = WebApplication.CreateBuilder(args);

// API & OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(/* ... */);

// AWS Lambda Hosting
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Application & Infrastructure Layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Authentication & Authorization
builder.Services.AddCognitoAuthentication(builder.Configuration);
builder.Services.AddRoleBasedAuthorization();

// CORS
builder.Services.AddDevelopmentCors();

var app = builder.Build();

// Middleware Pipeline
app.UsePathPrefixRewrite("/api/users");
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseDevelopmentCors();
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapControllers();

app.Run();
```

#### 2. Controllers (`Controllers/`)

```csharp
[ApiController]
[Route("")]
[Tags("Users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserController> _logger;

    public UserController(IMediator mediator, ILogger<UserController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentUser(
        CancellationToken cancellationToken)
    {
        var cognitoId = GetCognitoId();
        if (string.IsNullOrEmpty(cognitoId))
            return Unauthorized();

        var query = new GetUserProfileQuery(cognitoId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound("User not found");

        return Ok(result);
    }

    [HttpPut("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateCurrentUser(
        [FromBody] UpdateProfileDto dto,
        CancellationToken cancellationToken)
    {
        var cognitoId = GetCognitoId();
        if (string.IsNullOrEmpty(cognitoId))
            return Unauthorized();

        var command = new UpdateProfileCommand(
            cognitoId,
            dto.FirstName,
            dto.LastName
        );
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    private string? GetCognitoId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;
    }
}
```

**Controller Principles**:

- Thin controllers (no business logic)
- Delegate to MediatR handlers
- Return appropriate HTTP status codes
- Use DTOs for requests/responses
- Handle authentication/authorization at controller level

#### 3. Middleware (`Middleware/`)

Custom middleware for cross-cutting concerns:

- Exception handling
- Request/response logging
- Path rewriting for API Gateway
- CORS

---

## Cross-Cutting Concerns

### 1. Authentication & Authorization

**AWS Cognito Integration**:

```csharp
builder.Services.AddCognitoAuthentication(builder.Configuration);
```

**Role-Based Authorization**:

```csharp
[Authorize(Roles = "Admin")]
public async Task<IActionResult> AdminOnlyEndpoint() { /* ... */ }
```

### 2. Validation

**Input Validation**: FluentValidation via `ValidationBehavior<,>`
**Domain Validation**: Enforced in Entity factory methods and behavior methods

### 3. Error Handling

**Domain Exceptions**: For invariant violations
**Result Pattern**: For business rule failures
**Global Exception Handler**: Middleware for unhandled exceptions

### 4. Logging

**Structured Logging**: ILogger<T> injected into handlers
**Log Levels**: Trace, Debug, Information, Warning, Error, Critical

### 5. Transaction Management

**Unit of Work**: `SaveChangesAsync()` in repositories
**Transactional Boundaries**: Command handlers

---

## Best Practices

### Domain Layer

✅ **DO**:

- Use Value Objects for domain primitives
- Encapsulate invariants in entities
- Raise domain events for state changes
- Use factory methods for entity creation
- Keep domain logic in domain layer

❌ **DON'T**:

- Reference infrastructure libraries
- Use data annotations for validation
- Expose setters publicly
- Create anemic domain models
- Throw exceptions for business rule violations (use Result pattern)

### Application Layer

✅ **DO**:

- Use CQRS pattern consistently
- Return DTOs from queries
- Return Result from commands
- Validate inputs with FluentValidation
- Keep handlers focused (Single Responsibility)

❌ **DON'T**:

- Return domain entities from queries
- Put business logic in handlers
- Skip validation
- Couple to infrastructure concerns

### Infrastructure Layer

✅ **DO**:

- Implement repository interfaces from domain
- Use Entity Framework conventions
- Apply migrations for schema changes
- Use connection pooling
- Handle transient failures (retry policies)

❌ **DON'T**:

- Reference domain entities in API responses
- Bypass repositories for data access
- Use raw SQL unless necessary
- Leak infrastructure abstractions to other layers

### API Layer

✅ **DO**:

- Use attribute routing
- Document with OpenAPI/Swagger
- Return appropriate HTTP status codes
- Use cancellation tokens
- Validate authorization at endpoint level

❌ **DON'T**:

- Put business logic in controllers
- Return domain entities directly
- Ignore error handling
- Expose internal details in responses

---

## Testing Strategy

### Unit Tests

- **Domain Logic**: Test entities, value objects, domain services
- **Application Logic**: Test command/query handlers in isolation
- **Validation**: Test FluentValidation rules

### Integration Tests

- **API Endpoints**: Test controllers end-to-end
- **Database**: Test repositories with real database
- **External Services**: Test with mocked or test doubles

### Architecture Tests

- **Dependency Rules**: Verify layer dependencies
- **Naming Conventions**: Enforce naming standards
- **Immutability**: Verify value objects are immutable

---

## Deployment

### Docker Containers

Each microservice has:

- `dev.Dockerfile` for development
- `Dockerfile` for production
- `docker-compose.yml` for local orchestration

### AWS Lambda

- Containerized Lambda functions
- API Gateway integration
- Cognito for authentication

### Database Migrations

```bash
dotnet ef migrations add {MigrationName} --project {Service}.Infrastructure
dotnet ef database update --project {Service}.Infrastructure
```

---

## Summary

This specification defines a **robust, scalable, and maintainable** backend architecture following:

1. **Domain-Driven Design**: Rich domain models with behavior
2. **Clean Architecture**: Separation of concerns with clear boundaries
3. **CQRS**: Separate read and write models
4. **Microservices**: Independent, deployable services
5. **Shared Kernel**: Reusable primitives and patterns

By following these patterns, each microservice maintains:

- **Testability**: Isolated layers and dependencies
- **Maintainability**: Clear structure and conventions
- **Scalability**: Independent deployment and scaling
- **Flexibility**: Easy to extend and modify

---

## References

- [Domain-Driven Design by Eric Evans](https://www.domainlanguage.com/ddd/)
- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
