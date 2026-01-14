# Milestone 1: Authentication & User Management

**Feature:** User Authentication & Profile Management  
**Epic:** User Management  
**Priority:** P0 (Blocking for all features)  
**Estimated Effort:** 16-20 hours  
**Status:** 🔄 Ready for Implementation

---

## Table of Contents

1. [Feature Overview](#1-feature-overview)
2. [Domain Model](#2-domain-model)
3. [Database Schema](#3-database-schema)
4. [Application Layer](#4-application-layer)
5. [Infrastructure Layer](#5-infrastructure-layer)
6. [API Endpoints](#6-api-endpoints)
7. [AWS Cognito Integration](#7-aws-cognito-integration)
8. [Implementation Tasks](#8-implementation-tasks)

---

## 1. Feature Overview

### User Stories

**US-1.1: Teacher Registration**  
_As a_ teacher  
_I want to_ register an account with my email  
_So that_ I can create classes and manage students

**US-1.2: Student Registration**  
_As a_ student  
_I want to_ register an account with my email  
_So that_ I can join classes and access learning materials

**US-1.3: User Login**  
_As a_ registered user  
_I want to_ log in with my email and password  
_So that_ I can access my personalized dashboard

**US-1.4: Profile Management**  
_As a_ logged-in user  
_I want to_ update my profile (name, avatar)  
_So that_ my information is current

**US-1.5: Avatar Upload**  
_As a_ logged-in user  
_I want to_ upload a profile picture  
_So that_ others can recognize me visually

### Acceptance Criteria

- ✅ Users can register with email + password (min 8 chars, 1 uppercase, 1 lowercase, 1 number)
- ✅ Email verification required before first login
- ✅ JWT tokens issued upon successful login (15-min access token, 30-day refresh token)
- ✅ User role (Teacher/Student) stored in Cognito custom attributes
- ✅ Profile updates sync to both Cognito and local UserDB
- ✅ Avatar images uploaded to S3 with presigned URLs (max 2MB, JPG/PNG)
- ✅ Soft delete users (IsDeleted flag, Cognito user disabled)

---

## 2. Domain Model

### Domain Entities

#### `User` (Aggregate Root)

```csharp
namespace FrogEdu.User.Domain.Entities;

public class User : Entity
{
    public CognitoUserId CognitoId { get; private set; } // Value object
    public Email Email { get; private set; } // Value object
    public FullName FullName { get; private set; } // Value object
    public UserRole Role { get; private set; } // Enum
    public string? AvatarUrl { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public bool IsEmailVerified { get; private set; }

    // Collections
    private readonly List<ClassMembership> _classMemberships = new();
    public IReadOnlyList<ClassMembership> ClassMemberships => _classMemberships.AsReadOnly();

    // Factory method
    public static User Create(
        string cognitoId,
        string email,
        string firstName,
        string lastName,
        UserRole role)
    {
        var user = new User
        {
            CognitoId = CognitoUserId.Create(cognitoId),
            Email = Email.Create(email),
            FullName = FullName.Create(firstName, lastName),
            Role = role,
            IsEmailVerified = false
        };

        user.AddDomainEvent(new UserCreatedDomainEvent(user.Id, user.Email.Value));
        return user;
    }

    public void UpdateProfile(string firstName, string lastName)
    {
        FullName = FullName.Create(firstName, lastName);
        MarkAsUpdated();
    }

    public void UpdateAvatar(string avatarUrl)
    {
        if (string.IsNullOrWhiteSpace(avatarUrl))
            throw new DomainException("Avatar URL cannot be empty");

        AvatarUrl = avatarUrl;
        MarkAsUpdated();
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        MarkAsUpdated();
    }

    public void VerifyEmail()
    {
        IsEmailVerified = true;
        MarkAsUpdated();
    }
}
```

#### Value Objects

```csharp
// CognitoUserId.cs
public class CognitoUserId : ValueObject
{
    public string Value { get; private set; }

    private CognitoUserId(string value) => Value = value;

    public static CognitoUserId Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Cognito user ID cannot be empty");

        // Cognito User Pool IDs are UUIDs
        if (!Guid.TryParse(value, out _))
            throw new DomainException("Invalid Cognito user ID format");

        return new CognitoUserId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

// Email.cs
public class Email : ValueObject
{
    public string Value { get; private set; }

    private Email(string value) => Value = value;

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email cannot be empty");

        // Simple regex validation
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!emailRegex.IsMatch(value))
            throw new DomainException("Invalid email format");

        return new Email(value.ToLowerInvariant());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

// FullName.cs
public class FullName : ValueObject
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullNameValue => $"{FirstName} {LastName}";

    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static FullName Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name cannot be empty");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name cannot be empty");

        return new FullName(firstName.Trim(), lastName.Trim());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}
```

#### Enumerations

```csharp
public enum UserRole
{
    Student = 1,
    Teacher = 2,
    Admin = 3 // For future use
}
```

#### Domain Events

```csharp
public record UserCreatedDomainEvent(Guid UserId, string Email) : DomainEvent;

public record UserProfileUpdatedDomainEvent(Guid UserId, string FullName) : DomainEvent;

public record UserAvatarUploadedDomainEvent(Guid UserId, string AvatarUrl) : DomainEvent;
```

---

## 3. Database Schema

### UserDB Tables

#### `Users` Table

```sql
CREATE TABLE [dbo].[Users] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [CognitoId] NVARCHAR(128) NOT NULL UNIQUE,
    [Email] NVARCHAR(256) NOT NULL,
    [FirstName] NVARCHAR(100) NOT NULL,
    [LastName] NVARCHAR(100) NOT NULL,
    [Role] INT NOT NULL, -- 1=Student, 2=Teacher, 3=Admin
    [AvatarUrl] NVARCHAR(512) NULL,
    [LastLoginAt] DATETIME2 NULL,
    [IsEmailVerified] BIT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [CK_Users_Role] CHECK ([Role] IN (1, 2, 3)),
    CONSTRAINT [CK_Users_Email] CHECK (Email LIKE '%_@_%._%')
);

-- Indexes
CREATE UNIQUE INDEX [IX_Users_CognitoId] ON [Users]([CognitoId]) WHERE [IsDeleted] = 0;
CREATE UNIQUE INDEX [IX_Users_Email] ON [Users]([Email]) WHERE [IsDeleted] = 0;
CREATE INDEX [IX_Users_Role] ON [Users]([Role]) WHERE [IsDeleted] = 0;
CREATE INDEX [IX_Users_LastLoginAt] ON [Users]([LastLoginAt]) WHERE [IsDeleted] = 0;
```

#### `ClassMemberships` Table (Linked to Class Management)

```sql
CREATE TABLE [dbo].[ClassMemberships] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [ClassId] UNIQUEIDENTIFIER NOT NULL,
    [Role] NVARCHAR(20) NOT NULL, -- 'Teacher', 'Student'
    [JoinedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT [FK_ClassMemberships_Users] FOREIGN KEY ([UserId])
        REFERENCES [Users]([Id]) ON DELETE CASCADE,
    CONSTRAINT [CK_ClassMemberships_Role] CHECK ([Role] IN ('Teacher', 'Student'))
);

-- Indexes
CREATE INDEX [IX_ClassMemberships_UserId] ON [ClassMemberships]([UserId]);
CREATE INDEX [IX_ClassMemberships_ClassId] ON [ClassMemberships]([ClassId]);
CREATE UNIQUE INDEX [IX_ClassMemberships_Unique] ON [ClassMemberships]([UserId], [ClassId])
    WHERE [IsActive] = 1;
```

### EF Core Configuration

```csharp
// UserConfiguration.cs
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.CognitoId)
            .HasConversion(
                cognitoId => cognitoId.Value,
                value => CognitoUserId.Create(value))
            .HasMaxLength(128)
            .IsRequired();

        builder.HasIndex(u => u.CognitoId)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value))
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.OwnsOne(u => u.FullName, fullName =>
        {
            fullName.Property(fn => fn.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(100)
                .IsRequired();

            fullName.Property(fn => fn.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.Property(u => u.Role)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(u => u.AvatarUrl)
            .HasMaxLength(512);

        builder.Property(u => u.LastLoginAt);

        builder.Property(u => u.IsEmailVerified)
            .HasDefaultValue(false);

        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.IsDeleted)
            .HasDefaultValue(false);

        // Global query filter for soft delete
        builder.HasQueryFilter(u => !u.IsDeleted);

        builder.HasMany(u => u.ClassMemberships)
            .WithOne()
            .HasForeignKey("UserId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(u => u.DomainEvents);
    }
}
```

---

## 4. Application Layer

### Commands

#### `RegisterUserCommand`

```csharp
public record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    UserRole Role
) : IRequest<Result<UserDto>>;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one number");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100);

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("Invalid user role");
    }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICognitoService _cognitoService;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        ICognitoService cognitoService,
        ILogger<RegisterUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _cognitoService = cognitoService;
        _logger = logger;
    }

    public async Task<Result<UserDto>> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        // Check if email already exists
        var existingUser = await _userRepository.FindByEmailAsync(request.Email, ct);
        if (existingUser != null)
        {
            return Result.Failure<UserDto>("Email already registered");
        }

        // Register user in Cognito
        var cognitoResult = await _cognitoService.RegisterUserAsync(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            request.Role,
            ct
        );

        if (!cognitoResult.IsSuccess)
        {
            return Result.Failure<UserDto>(cognitoResult.Error);
        }

        // Create user in local database
        var user = User.Create(
            cognitoResult.Value.CognitoUserId,
            request.Email,
            request.FirstName,
            request.LastName,
            request.Role
        );

        await _userRepository.AddAsync(user, ct);
        await _userRepository.SaveChangesAsync(ct);

        _logger.LogInformation(
            "User registered successfully. UserId={UserId}, Email={Email}, Role={Role}",
            user.Id, user.Email.Value, user.Role
        );

        return Result.Success(UserDto.FromEntity(user));
    }
}
```

#### `UpdateUserProfileCommand`

```csharp
public record UpdateUserProfileCommand(
    Guid UserId,
    string FirstName,
    string LastName
) : IRequest<Result<UserDto>>;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICognitoService _cognitoService;

    public async Task<Result<UserDto>> Handle(UpdateUserProfileCommand request, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, ct);
        if (user == null)
        {
            return Result.Failure<UserDto>("User not found");
        }

        user.UpdateProfile(request.FirstName, request.LastName);

        // Sync to Cognito
        await _cognitoService.UpdateUserAttributesAsync(
            user.CognitoId.Value,
            new Dictionary<string, string>
            {
                ["name"] = $"{request.FirstName} {request.LastName}"
            },
            ct
        );

        await _userRepository.UpdateAsync(user, ct);
        await _userRepository.SaveChangesAsync(ct);

        return Result.Success(UserDto.FromEntity(user));
    }
}
```

#### `UploadAvatarCommand`

```csharp
public record UploadAvatarCommand(
    Guid UserId,
    Stream FileStream,
    string FileName,
    string ContentType
) : IRequest<Result<string>>; // Returns avatar URL

public class UploadAvatarCommandHandler : IRequestHandler<UploadAvatarCommand, Result<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IS3StorageService _s3Service;

    public async Task<Result<string>> Handle(UploadAvatarCommand request, CancellationToken ct)
    {
        // Validate file type
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg" };
        if (!allowedTypes.Contains(request.ContentType.ToLowerInvariant()))
        {
            return Result.Failure<string>("Invalid file type. Only JPG and PNG are allowed.");
        }

        // Validate file size (max 2MB)
        if (request.FileStream.Length > 2 * 1024 * 1024)
        {
            return Result.Failure<string>("File size exceeds 2MB limit.");
        }

        var user = await _userRepository.GetByIdAsync(request.UserId, ct);
        if (user == null)
        {
            return Result.Failure<string>("User not found");
        }

        // Generate S3 key: user-uploads/{userId}/avatar.{ext}
        var extension = Path.GetExtension(request.FileName);
        var s3Key = $"user-uploads/{user.Id}/avatar{extension}";

        // Upload to S3
        var s3Url = await _s3Service.UploadFileAsync(
            request.FileStream,
            s3Key,
            request.ContentType,
            ct
        );

        // Update user entity
        user.UpdateAvatar(s3Url);
        await _userRepository.UpdateAsync(user, ct);
        await _userRepository.SaveChangesAsync(ct);

        // Generate presigned URL (15-min expiry for immediate display)
        var presignedUrl = await _s3Service.GetPresignedUrlAsync(s3Key, TimeSpan.FromMinutes(15), ct);

        return Result.Success(presignedUrl);
    }
}
```

### Queries

#### `GetUserByIdQuery`

```csharp
public record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserDto>>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, ct);
        if (user == null)
        {
            return Result.Failure<UserDto>("User not found");
        }

        return Result.Success(UserDto.FromEntity(user));
    }
}
```

#### `GetUserByCognitoIdQuery`

```csharp
public record GetUserByCognitoIdQuery(string CognitoId) : IRequest<Result<UserDto>>;

public class GetUserByCognitoIdQueryHandler : IRequestHandler<GetUserByCognitoIdQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public async Task<Result<UserDto>> Handle(GetUserByCognitoIdQuery request, CancellationToken ct)
    {
        var user = await _userRepository.FindByCognitoIdAsync(request.CognitoId, ct);
        if (user == null)
        {
            return Result.Failure<UserDto>("User not found");
        }

        return Result.Success(UserDto.FromEntity(user));
    }
}
```

### DTOs

```csharp
public record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    UserRole Role,
    string? AvatarUrl,
    DateTime? LastLoginAt,
    bool IsEmailVerified,
    DateTime CreatedAt
)
{
    public static UserDto FromEntity(User user) => new(
        user.Id,
        user.Email.Value,
        user.FullName.FirstName,
        user.FullName.LastName,
        user.FullName.FullNameValue,
        user.Role,
        user.AvatarUrl,
        user.LastLoginAt,
        user.IsEmailVerified,
        user.CreatedAt
    );
}
```

---

## 5. Infrastructure Layer

### Repository Implementation

```csharp
public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context) => _context = context;

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<User?> FindByCognitoIdAsync(string cognitoId, CancellationToken ct)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.CognitoId.Value == cognitoId, ct);
    }

    public async Task<User?> FindByEmailAsync(string email, CancellationToken ct)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email.Value == email.ToLowerInvariant(), ct);
    }

    public async Task AddAsync(User user, CancellationToken ct)
    {
        await _context.Users.AddAsync(user, ct);
    }

    public Task UpdateAsync(User user, CancellationToken ct)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await _context.SaveChangesAsync(ct);
    }
}
```

### S3 Storage Service (from 00-backend-overview.md)

```csharp
public interface IS3StorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string key, string contentType, CancellationToken ct);
    Task<string> GetPresignedUrlAsync(string key, TimeSpan expiration, CancellationToken ct);
    Task DeleteFileAsync(string key, CancellationToken ct);
}

public class S3StorageService : IS3StorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3StorageService(IAmazonS3 s3Client, IOptions<AwsSettings> options)
    {
        _s3Client = s3Client;
        _bucketName = options.Value.S3BucketName;
    }

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

    public Task<string> GetPresignedUrlAsync(string key, TimeSpan expiration, CancellationToken ct)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Expires = DateTime.UtcNow.Add(expiration),
            Verb = HttpVerb.GET
        };

        return Task.FromResult(_s3Client.GetPreSignedURL(request));
    }

    public async Task DeleteFileAsync(string key, CancellationToken ct)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        await _s3Client.DeleteObjectAsync(request, ct);
    }
}
```

---

## 6. API Endpoints

### `UsersController.cs`

```csharp
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterUserCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : BadRequest(result.Error);
    }

    /// <summary>
    /// Get current user profile
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMe(CancellationToken ct)
    {
        // Extract Cognito user ID from JWT claims
        var cognitoId = User.FindFirstValue("sub"); // 'sub' claim contains Cognito User Pool ID
        if (string.IsNullOrEmpty(cognitoId))
        {
            return Unauthorized();
        }

        var query = new GetUserByCognitoIdQuery(cognitoId);
        var result = await _mediator.Send(query, ct);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var query = new GetUserByIdQuery(id);
        var result = await _mediator.Send(query, ct);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    /// <summary>
    /// Update user profile
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserProfileCommand command, CancellationToken ct)
    {
        // Ensure user can only update their own profile (unless admin)
        var cognitoId = User.FindFirstValue("sub");
        var currentUser = await _mediator.Send(new GetUserByCognitoIdQuery(cognitoId!), ct);

        if (currentUser.IsSuccess && currentUser.Value.Id != id)
        {
            return Forbid(); // User trying to update someone else's profile
        }

        var result = await _mediator.Send(command with { UserId = id }, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Upload user avatar
    /// </summary>
    [HttpPost("{id:guid}/avatar")]
    [Authorize]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)] // Returns presigned URL
    [RequestSizeLimit(2 * 1024 * 1024)] // 2MB limit
    public async Task<IActionResult> UploadAvatar(Guid id, IFormFile file, CancellationToken ct)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        var cognitoId = User.FindFirstValue("sub");
        var currentUser = await _mediator.Send(new GetUserByCognitoIdQuery(cognitoId!), ct);

        if (currentUser.IsSuccess && currentUser.Value.Id != id)
        {
            return Forbid();
        }

        await using var stream = file.OpenReadStream();
        var command = new UploadAvatarCommand(id, stream, file.FileName, file.ContentType);
        var result = await _mediator.Send(command, ct);

        return result.IsSuccess ? Ok(new { avatarUrl = result.Value }) : BadRequest(result.Error);
    }
}
```

---

## 7. AWS Cognito Integration

### Cognito Service Interface

```csharp
public interface ICognitoService
{
    Task<Result<CognitoUserResult>> RegisterUserAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        UserRole role,
        CancellationToken ct);

    Task<Result<CognitoTokenResult>> AuthenticateUserAsync(
        string email,
        string password,
        CancellationToken ct);

    Task<Result> UpdateUserAttributesAsync(
        string cognitoId,
        Dictionary<string, string> attributes,
        CancellationToken ct);

    Task<Result> DisableUserAsync(string cognitoId, CancellationToken ct);
}

public record CognitoUserResult(string CognitoUserId, string Email);

public record CognitoTokenResult(
    string AccessToken,
    string RefreshToken,
    string IdToken,
    int ExpiresIn
);
```

### Cognito Service Implementation

```csharp
public class CognitoService : ICognitoService
{
    private readonly IAmazonCognitoIdentityProvider _cognitoClient;
    private readonly string _userPoolId;
    private readonly string _clientId;
    private readonly ILogger<CognitoService> _logger;

    public CognitoService(
        IAmazonCognitoIdentityProvider cognitoClient,
        IOptions<AwsCognitoSettings> settings,
        ILogger<CognitoService> logger)
    {
        _cognitoClient = cognitoClient;
        _userPoolId = settings.Value.UserPoolId;
        _clientId = settings.Value.ClientId;
        _logger = logger;
    }

    public async Task<Result<CognitoUserResult>> RegisterUserAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        UserRole role,
        CancellationToken ct)
    {
        try
        {
            var request = new SignUpRequest
            {
                ClientId = _clientId,
                Username = email,
                Password = password,
                UserAttributes = new List<AttributeType>
                {
                    new() { Name = "email", Value = email },
                    new() { Name = "name", Value = $"{firstName} {lastName}" },
                    new() { Name = "custom:role", Value = role.ToString() }
                }
            };

            var response = await _cognitoClient.SignUpAsync(request, ct);

            _logger.LogInformation(
                "User registered in Cognito. UserSub={UserSub}, Email={Email}",
                response.UserSub, email
            );

            return Result.Success(new CognitoUserResult(response.UserSub, email));
        }
        catch (UsernameExistsException)
        {
            return Result.Failure<CognitoUserResult>("Email already exists");
        }
        catch (InvalidPasswordException ex)
        {
            return Result.Failure<CognitoUserResult>($"Invalid password: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to register user in Cognito");
            return Result.Failure<CognitoUserResult>("Failed to register user");
        }
    }

    public async Task<Result<CognitoTokenResult>> AuthenticateUserAsync(
        string email,
        string password,
        CancellationToken ct)
    {
        try
        {
            var request = new InitiateAuthRequest
            {
                ClientId = _clientId,
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                AuthParameters = new Dictionary<string, string>
                {
                    ["USERNAME"] = email,
                    ["PASSWORD"] = password
                }
            };

            var response = await _cognitoClient.InitiateAuthAsync(request, ct);

            if (response.AuthenticationResult == null)
            {
                return Result.Failure<CognitoTokenResult>("Authentication failed");
            }

            var result = new CognitoTokenResult(
                response.AuthenticationResult.AccessToken,
                response.AuthenticationResult.RefreshToken,
                response.AuthenticationResult.IdToken,
                response.AuthenticationResult.ExpiresIn
            );

            return Result.Success(result);
        }
        catch (NotAuthorizedException)
        {
            return Result.Failure<CognitoTokenResult>("Invalid email or password");
        }
        catch (UserNotConfirmedException)
        {
            return Result.Failure<CognitoTokenResult>("Email not verified. Please check your inbox.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to authenticate user");
            return Result.Failure<CognitoTokenResult>("Authentication failed");
        }
    }

    public async Task<Result> UpdateUserAttributesAsync(
        string cognitoId,
        Dictionary<string, string> attributes,
        CancellationToken ct)
    {
        try
        {
            var request = new AdminUpdateUserAttributesRequest
            {
                UserPoolId = _userPoolId,
                Username = cognitoId,
                UserAttributes = attributes.Select(kvp => new AttributeType
                {
                    Name = kvp.Key,
                    Value = kvp.Value
                }).ToList()
            };

            await _cognitoClient.AdminUpdateUserAttributesAsync(request, ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update Cognito user attributes");
            return Result.Failure("Failed to update user attributes");
        }
    }

    public async Task<Result> DisableUserAsync(string cognitoId, CancellationToken ct)
    {
        try
        {
            var request = new AdminDisableUserRequest
            {
                UserPoolId = _userPoolId,
                Username = cognitoId
            };

            await _cognitoClient.AdminDisableUserAsync(request, ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to disable Cognito user");
            return Result.Failure("Failed to disable user");
        }
    }
}
```

### Configuration

```csharp
// appsettings.json
{
  "AWS": {
    "Cognito": {
      "UserPoolId": "ap-southeast-1_xxxxxxxxx",
      "ClientId": "xxxxxxxxxxxxxxxxxxxx",
      "Region": "ap-southeast-1",
      "Authority": "https://cognito-idp.ap-southeast-1.amazonaws.com/ap-southeast-1_xxxxxxxxx"
    },
    "S3": {
      "BucketName": "frogedu-assets-dev",
      "Region": "ap-southeast-1"
    }
  }
}

// Program.cs - Service Registration
builder.Services.Configure<AwsCognitoSettings>(builder.Configuration.GetSection("AWS:Cognito"));
builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AWS:S3"));

// AWS SDK clients
builder.Services.AddAWSService<IAmazonCognitoIdentityProvider>();
builder.Services.AddAWSService<IAmazonS3>();

// Application services
builder.Services.AddScoped<ICognitoService, CognitoService>();
builder.Services.AddScoped<IS3StorageService, S3StorageService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["AWS:Cognito:Authority"];
        options.Audience = builder.Configuration["AWS:Cognito:ClientId"];
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

---

## 8. Implementation Tasks

### Task 1.1: Domain Layer ⏸️

- [ ] **1.1.1** Create `User` entity with factory method
- [ ] **1.1.2** Create value objects: `CognitoUserId`, `Email`, `FullName`
- [ ] **1.1.3** Create `UserRole` enumeration
- [ ] **1.1.4** Create domain events: `UserCreatedDomainEvent`, etc.
- [ ] **1.1.5** Create `IUserRepository` interface

### Task 1.2: Infrastructure Layer ⏸️

- [ ] **1.2.1** Create `UserDbContext` with `DbSet<User>`
- [ ] **1.2.2** Create `UserConfiguration` for EF Core mapping
- [ ] **1.2.3** Create initial migration: `dotnet ef migrations add InitialUser`
- [ ] **1.2.4** Apply migration to local DB
- [ ] **1.2.5** Implement `UserRepository`
- [ ] **1.2.6** Implement `CognitoService` with AWS SDK
- [ ] **1.2.7** Implement `S3StorageService` for avatar uploads

### Task 1.3: Application Layer ⏸️

- [ ] **1.3.1** Create `RegisterUserCommand` with handler
- [ ] **1.3.2** Create `UpdateUserProfileCommand` with handler
- [ ] **1.3.3** Create `UploadAvatarCommand` with handler
- [ ] **1.3.4** Create `GetUserByIdQuery` with handler
- [ ] **1.3.5** Create `GetUserByCognitoIdQuery` with handler
- [ ] **1.3.6** Create `UserDto` for API responses
- [ ] **1.3.7** Add FluentValidation validators

### Task 1.4: API Layer ⏸️

- [ ] **1.4.1** Create `UsersController`
- [ ] **1.4.2** Implement `POST /api/users/register`
- [ ] **1.4.3** Implement `GET /api/users/me`
- [ ] **1.4.4** Implement `GET /api/users/{id}`
- [ ] **1.4.5** Implement `PUT /api/users/{id}`
- [ ] **1.4.6** Implement `POST /api/users/{id}/avatar`
- [ ] **1.4.7** Configure JWT authentication in Program.cs
- [ ] **1.4.8** Add Swagger annotations
- [ ] **1.4.9** Write API integration tests

### Task 1.5: AWS Cognito Integration ⏸️

- [ ] **1.5.1** Configure Cognito User Pool (see 00-foundation-milestone.md)
- [ ] **1.5.2** Add user groups (Teachers, Students)
- [ ] **1.5.3** Configure custom attributes (`custom:role`)

## 9. Validation Checklist

### Functionality

- [ ] User can register with email + password
- [ ] User receives email verification code
- [ ] User can log in after verification
- [ ] JWT tokens issued correctly
- [ ] User profile can be updated
- [ ] Avatar can be uploaded to S3
- [ ] Avatar URL returned with presigned URL
- [ ] Soft delete works (user disabled in Cognito)

### Security

- [ ] Passwords validated (min 8 chars, complexity)
- [ ] JWT tokens validated on protected endpoints
- [ ] Users cannot update other users' profiles
- [ ] Avatar uploads restricted to 2MB JPG/PNG
- [ ] SQL injection protected (EF Core parameterized queries)

### Performance

- [ ] User lookup by email uses index
- [ ] Avatar presigned URLs cached (15-min expiry)
- [ ] No N+1 queries in user retrieval

### AWS Integration

- [ ] Cognito User Pool configured
- [ ] Custom attribute `custom:role` working
- [ ] S3 bucket `user-uploads/` folder accessible
- [ ] Presigned URLs working for avatar display

---

## Next Steps

After completing this milestone:

1. ✅ **[02-class-management-feature.md](./02-class-management-feature.md)** - Class creation and enrollment
2. Implement Class domain with foreign key to Users table
3. Test full user → class → content flow

---

**Milestone Status:** Ready for Implementation ✅  
**Blocked By:** 00-foundation-milestone  
**Blocking:** All other features (auth required)  
**Estimated Completion:** 16-20 hours
