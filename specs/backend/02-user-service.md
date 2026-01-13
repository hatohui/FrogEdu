# User Service Specification

**Service:** User Service  
**Port:** 5003  
**Database:** UserDB  
**Version:** 1.0  
**Status:** Specification Phase

---

## Table of Contents

1. [Overview](#overview)
2. [Bounded Context](#bounded-context)
3. [Domain Model](#domain-model)
4. [Database Schema](#database-schema)
5. [API Endpoints](#api-endpoints)
6. [Authentication Flow](#authentication-flow)
7. [Business Rules](#business-rules)
8. [Integration Points](#integration-points)
9. [Implementation Tasks](#implementation-tasks)
10. [Acceptance Criteria](#acceptance-criteria)

---

## Overview

The User Service manages user authentication, profile management, and class relationships. It integrates with AWS Cognito for authentication and manages user data in its own database.

### Responsibilities

- ✅ User authentication via AWS Cognito
- ✅ User profile management (CRUD)
- ✅ Class creation and management
- ✅ Student-class enrollment
- ✅ Avatar upload to S3
- ✅ JWT token validation and refresh
- ✅ Role-based authorization (Teacher vs Student)

### Technology Stack

- **Framework:** .NET 9, ASP.NET Core
- **Database:** SQL Server (UserDB)
- **Authentication:** AWS Cognito (OAuth 2.0 / OIDC)
- **Storage:** AWS S3 (avatars)
- **Communication:** REST API only (no gRPC needed)

---

## Bounded Context

**Domain:** User Management and Authentication

**Ubiquitous Language:**

- **User**: A person using the platform (Teacher or Student)
- **Profile**: User's personal information and settings
- **Class**: A group of students managed by a teacher
- **Enrollment**: Association between a student and a class
- **Invite Code**: Unique code for students to join a class
- **Role**: User type (Teacher or Student)

---

## Domain Model

### Aggregate Roots

#### User

```csharp
public class User : Entity, IAggregateRoot
{
    public Guid Id { get; private set; }
    public string CognitoUserId { get; private set; } = string.Empty; // AWS Cognito Sub
    public string Email { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public string? AvatarS3Key { get; private set; }
    public string? Bio { get; private set; }
    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public bool IsDeleted { get; private set; }

    // Factory method
    public static User Create(
        string cognitoUserId,
        string email,
        string fullName,
        UserRole role)
    {
        if (string.IsNullOrWhiteSpace(cognitoUserId))
            throw new ArgumentException("Cognito user ID is required", nameof(cognitoUserId));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));

        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required", nameof(fullName));

        return new User
        {
            Id = Guid.NewGuid(),
            CognitoUserId = cognitoUserId,
            Email = email.ToLowerInvariant(),
            FullName = fullName,
            Role = role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    // Domain methods
    public void UpdateProfile(string fullName, string? bio)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty", nameof(fullName));

        FullName = fullName;
        Bio = bio;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAvatar(string s3Key)
    {
        if (string.IsNullOrWhiteSpace(s3Key))
            throw new ArgumentException("S3 key cannot be empty", nameof(s3Key));

        AvatarS3Key = s3Key;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
```

#### Class

```csharp
public class Class : Entity, IAggregateRoot
{
    public Guid Id { get; private set; }
    public Guid TeacherId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public GradeLevel GradeLevel { get; private set; }
    public string Subject { get; private set; } = string.Empty;
    public string InviteCode { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private readonly List<Enrollment> _enrollments = new();
    public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation property
    public User Teacher { get; private set; } = null!;

    // Factory method
    public static Class Create(
        Guid teacherId,
        string name,
        GradeLevel gradeLevel,
        string subject)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Class name is required", nameof(name));

        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Subject is required", nameof(subject));

        return new Class
        {
            Id = Guid.NewGuid(),
            TeacherId = teacherId,
            Name = name,
            GradeLevel = gradeLevel,
            Subject = subject,
            InviteCode = GenerateInviteCode(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    // Domain methods
    public void UpdateDetails(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Class name cannot be empty", nameof(name));

        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RegenerateInviteCode()
    {
        InviteCode = GenerateInviteCode();
        UpdatedAt = DateTime.UtcNow;
    }

    public Enrollment EnrollStudent(Guid studentId)
    {
        if (_enrollments.Any(e => e.StudentId == studentId && !e.IsDeleted))
            throw new InvalidOperationException("Student already enrolled in this class");

        var enrollment = Enrollment.Create(Id, studentId);
        _enrollments.Add(enrollment);
        return enrollment;
    }

    private static string GenerateInviteCode()
    {
        // Generate 6-character alphanumeric code
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Range(0, 6)
            .Select(_ => chars[random.Next(chars.Length)])
            .ToArray());
    }
}
```

### Entities

#### Enrollment

```csharp
public class Enrollment : Entity
{
    public Guid Id { get; private set; }
    public Guid ClassId { get; private set; }
    public Guid StudentId { get; private set; }
    public DateTime EnrolledAt { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation properties
    public Class Class { get; private set; } = null!;
    public User Student { get; private set; } = null!;

    public static Enrollment Create(Guid classId, Guid studentId)
    {
        return new Enrollment
        {
            Id = Guid.NewGuid(),
            ClassId = classId,
            StudentId = studentId,
            EnrolledAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false
        };
    }

    public void Unenroll()
    {
        IsActive = false;
        IsDeleted = true;
    }
}
```

### Value Objects

#### UserRole

```csharp
public enum UserRole
{
    Teacher = 1,
    Student = 2
}
```

#### GradeLevel

```csharp
public enum GradeLevel
{
    Grade1 = 1,
    Grade2 = 2,
    Grade3 = 3,
    Grade4 = 4,
    Grade5 = 5
}
```

---

## Database Schema

### SQL Schema

```sql
-- Users Table
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CognitoUserId NVARCHAR(256) NOT NULL UNIQUE,
    Email NVARCHAR(256) NOT NULL,
    FullName NVARCHAR(256) NOT NULL,
    Role INT NOT NULL,
    AvatarS3Key NVARCHAR(512) NULL,
    Bio NVARCHAR(1000) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    LastLoginAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CONSTRAINT CK_Users_Role CHECK (Role IN (1, 2))
);

CREATE INDEX IX_Users_CognitoUserId ON Users(CognitoUserId);
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_IsDeleted ON Users(IsDeleted) WHERE IsDeleted = 0;

-- Classes Table
CREATE TABLE Classes (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    TeacherId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(256) NOT NULL,
    Description NVARCHAR(1000) NULL,
    GradeLevel INT NOT NULL,
    Subject NVARCHAR(100) NOT NULL,
    InviteCode NVARCHAR(10) NOT NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Classes_Teacher FOREIGN KEY (TeacherId) REFERENCES Users(Id),
    CONSTRAINT CK_Classes_GradeLevel CHECK (GradeLevel BETWEEN 1 AND 5)
);

CREATE INDEX IX_Classes_TeacherId ON Classes(TeacherId);
CREATE INDEX IX_Classes_InviteCode ON Classes(InviteCode);
CREATE INDEX IX_Classes_IsDeleted ON Classes(IsDeleted) WHERE IsDeleted = 0;

-- Enrollments Table
CREATE TABLE Enrollments (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ClassId UNIQUEIDENTIFIER NOT NULL,
    StudentId UNIQUEIDENTIFIER NOT NULL,
    EnrolledAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Enrollments_Class FOREIGN KEY (ClassId) REFERENCES Classes(Id),
    CONSTRAINT FK_Enrollments_Student FOREIGN KEY (StudentId) REFERENCES Users(Id),
    CONSTRAINT UQ_Enrollments_ClassStudent UNIQUE (ClassId, StudentId)
);

CREATE INDEX IX_Enrollments_ClassId ON Enrollments(ClassId);
CREATE INDEX IX_Enrollments_StudentId ON Enrollments(StudentId);
CREATE INDEX IX_Enrollments_IsDeleted ON Enrollments(IsDeleted) WHERE IsDeleted = 0;
```

### EF Core Configuration

```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.CognitoUserId)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(u => u.AvatarS3Key)
            .HasMaxLength(512);

        builder.Property(u => u.Bio)
            .HasMaxLength(1000);

        // Global query filter for soft delete
        builder.HasQueryFilter(u => !u.IsDeleted);

        // Indexes
        builder.HasIndex(u => u.CognitoUserId).IsUnique();
        builder.HasIndex(u => u.Email);
    }
}
```

---

## API Endpoints

### Authentication Endpoints

#### POST /api/auth/register

Register a new user via AWS Cognito.

**Request:**

```json
{
  "email": "teacher@example.com",
  "password": "SecurePass123!",
  "fullName": "John Doe",
  "role": "Teacher"
}
```

**Response (201 Created):**

```json
{
  "userId": "123e4567-e89b-12d3-a456-426614174000",
  "email": "teacher@example.com",
  "message": "Verification email sent. Please check your inbox."
}
```

**Process:**

1. Call AWS Cognito `SignUp`
2. Create User record in database (IsActive = false)
3. Return success (user must verify email)

#### POST /api/auth/verify-email

Verify email with code sent by Cognito.

**Request:**

```json
{
  "email": "teacher@example.com",
  "code": "123456"
}
```

**Response (200 OK):**

```json
{
  "message": "Email verified successfully"
}
```

**Process:**

1. Call AWS Cognito `ConfirmSignUp`
2. Update User record (IsActive = true)

#### POST /api/auth/login

Login with email and password.

**Request:**

```json
{
  "email": "teacher@example.com",
  "password": "SecurePass123!",
  "rememberMe": false
}
```

**Response (200 OK):**

```json
{
  "accessToken": "eyJhbGc...",
  "refreshToken": "eyJjdH...",
  "expiresIn": 3600,
  "userId": "123e4567-e89b-12d3-a456-426614174000",
  "email": "teacher@example.com",
  "fullName": "John Doe",
  "role": "Teacher",
  "avatarUrl": "https://s3.amazonaws.com/..."
}
```

**Process:**

1. Call AWS Cognito `InitiateAuth`
2. Get Cognito user info
3. Find User by CognitoUserId
4. Update LastLoginAt
5. Return tokens + user info

#### POST /api/auth/refresh

Refresh access token.

**Request:**

```json
{
  "refreshToken": "eyJjdH..."
}
```

**Response (200 OK):**

```json
{
  "accessToken": "eyJhbGc...",
  "expiresIn": 3600
}
```

#### POST /api/auth/forgot-password

Request password reset code.

**Request:**

```json
{
  "email": "teacher@example.com"
}
```

**Response (200 OK):**

```json
{
  "message": "Password reset code sent to your email"
}
```

#### POST /api/auth/reset-password

Reset password with code.

**Request:**

```json
{
  "email": "teacher@example.com",
  "code": "123456",
  "newPassword": "NewSecurePass123!"
}
```

**Response (200 OK):**

```json
{
  "message": "Password reset successfully"
}
```

### Profile Endpoints

#### GET /api/profile

Get current user profile.

**Response (200 OK):**

```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "email": "teacher@example.com",
  "fullName": "John Doe",
  "role": "Teacher",
  "avatarUrl": "https://s3.amazonaws.com/...",
  "bio": "Experienced math teacher",
  "createdAt": "2026-01-01T00:00:00Z",
  "lastLoginAt": "2026-01-13T10:30:00Z"
}
```

#### PUT /api/profile

Update user profile.

**Request:**

```json
{
  "fullName": "John Updated Doe",
  "bio": "Updated bio"
}
```

**Response (200 OK):**

```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "fullName": "John Updated Doe",
  "bio": "Updated bio",
  "updatedAt": "2026-01-13T10:35:00Z"
}
```

#### POST /api/profile/avatar

Upload avatar image.

**Request:** Multipart form-data

- `file`: Image file (jpg/png, max 5MB)

**Response (200 OK):**

```json
{
  "avatarUrl": "https://s3.amazonaws.com/edu-classroom-assets/avatars/{userId}/avatar.jpg",
  "message": "Avatar uploaded successfully"
}
```

**Process:**

1. Validate file (type, size)
2. Upload to S3 (`avatars/{userId}/avatar.{ext}`)
3. Update User.AvatarS3Key
4. Return presigned URL

### Class Endpoints (Teacher Only)

#### POST /api/classes

Create a new class.

**Request:**

```json
{
  "name": "Math Grade 5 - Class A",
  "description": "Advanced mathematics",
  "gradeLevel": 5,
  "subject": "Mathematics"
}
```

**Response (201 Created):**

```json
{
  "id": "456e4567-e89b-12d3-a456-426614174000",
  "name": "Math Grade 5 - Class A",
  "description": "Advanced mathematics",
  "gradeLevel": 5,
  "subject": "Mathematics",
  "inviteCode": "ABC123",
  "studentCount": 0,
  "createdAt": "2026-01-13T10:40:00Z"
}
```

#### GET /api/classes

Get all classes for current teacher.

**Response (200 OK):**

```json
[
  {
    "id": "456e4567-e89b-12d3-a456-426614174000",
    "name": "Math Grade 5 - Class A",
    "gradeLevel": 5,
    "subject": "Mathematics",
    "inviteCode": "ABC123",
    "studentCount": 25,
    "isActive": true,
    "createdAt": "2026-01-13T10:40:00Z"
  }
]
```

#### GET /api/classes/{id}

Get class details with enrolled students.

**Response (200 OK):**

```json
{
  "id": "456e4567-e89b-12d3-a456-426614174000",
  "name": "Math Grade 5 - Class A",
  "description": "Advanced mathematics",
  "gradeLevel": 5,
  "subject": "Mathematics",
  "inviteCode": "ABC123",
  "teacher": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "fullName": "John Doe",
    "email": "teacher@example.com"
  },
  "students": [
    {
      "id": "789e4567-e89b-12d3-a456-426614174000",
      "fullName": "Student One",
      "email": "student1@example.com",
      "enrolledAt": "2026-01-10T08:00:00Z"
    }
  ],
  "createdAt": "2026-01-13T10:40:00Z"
}
```

#### PUT /api/classes/{id}

Update class details.

**Request:**

```json
{
  "name": "Math Grade 5 - Class A (Updated)",
  "description": "Updated description"
}
```

#### POST /api/classes/{id}/regenerate-code

Regenerate invite code.

**Response (200 OK):**

```json
{
  "inviteCode": "XYZ789"
}
```

#### DELETE /api/classes/{id}

Soft delete a class.

**Response (204 No Content)**

### Enrollment Endpoints (Student Only)

#### POST /api/enrollments

Enroll in a class using invite code.

**Request:**

```json
{
  "inviteCode": "ABC123"
}
```

**Response (201 Created):**

```json
{
  "classId": "456e4567-e89b-12d3-a456-426614174000",
  "className": "Math Grade 5 - Class A",
  "teacherName": "John Doe",
  "enrolledAt": "2026-01-13T11:00:00Z"
}
```

#### GET /api/enrollments/my-classes

Get all classes student is enrolled in.

**Response (200 OK):**

```json
[
  {
    "classId": "456e4567-e89b-12d3-a456-426614174000",
    "className": "Math Grade 5 - Class A",
    "subject": "Mathematics",
    "gradeLevel": 5,
    "teacherName": "John Doe",
    "enrolledAt": "2026-01-13T11:00:00Z"
  }
]
```

#### DELETE /api/enrollments/{classId}

Unenroll from a class.

**Response (204 No Content)**

---

## Authentication Flow

### Registration Flow

```
User (Frontend)
    ↓ POST /api/auth/register
User Service
    ↓ SignUp API
AWS Cognito
    ↓ Send verification email
User's Email
    ↓ User clicks link / enters code
User (Frontend)
    ↓ POST /api/auth/verify-email
User Service
    ↓ ConfirmSignUp API
AWS Cognito
    ↓ Account activated
User Service (Update IsActive = true)
```

### Login Flow

```
User (Frontend)
    ↓ POST /api/auth/login
User Service
    ↓ InitiateAuth (username/password)
AWS Cognito
    ↓ Validate credentials
    ↓ Return tokens
User Service
    ↓ Get user from DB by CognitoUserId
    ↓ Update LastLoginAt
    ↓ Return tokens + user info
User (Frontend stores tokens)
```

### Token Refresh Flow

```
Frontend (Token expired)
    ↓ POST /api/auth/refresh {refreshToken}
User Service
    ↓ InitiateAuth (REFRESH_TOKEN_AUTH)
AWS Cognito
    ↓ Validate refresh token
    ↓ Return new access token
User Service
    ↓ Return new access token
Frontend (Update stored token)
```

### Authorized Request Flow

```
Frontend
    ↓ GET /api/profile
    ↓ Header: Authorization: Bearer {accessToken}
API Gateway
    ↓ Validate JWT signature
    ↓ Check expiration
    ↓ Extract user claims (sub, role)
User Service
    ↓ Process request with user context
    ↓ Return response
```

---

## Business Rules

### User Registration

- [ ] Email must be unique (enforced by Cognito)
- [ ] Password must meet requirements (8+ chars, 1 uppercase, 1 number, 1 special)
- [ ] Role must be specified (Teacher or Student)
- [ ] Email verification required before login
- [ ] New users start as inactive (IsActive = false)

### Profile Updates

- [ ] Users can only update their own profile
- [ ] Email cannot be changed (requires Cognito email change flow)
- [ ] FullName is required (cannot be empty)
- [ ] Bio has 1000 character limit
- [ ] Avatar must be image (jpg/png/webp), max 5MB

### Class Management

- [ ] Only Teachers can create classes
- [ ] Class name is required
- [ ] Invite code must be unique (6 alphanumeric characters)
- [ ] Teachers can only manage their own classes
- [ ] Deleting a class is soft delete (preserves history)

### Enrollment

- [ ] Only Students can enroll in classes
- [ ] Valid invite code required
- [ ] Student cannot enroll in same class twice
- [ ] Students can enroll in multiple classes
- [ ] Students can unenroll from classes

---

## Integration Points

### AWS Cognito Integration

```csharp
public class CognitoAuthService : IAuthService
{
    private readonly IAmazonCognitoIdentityProvider _cognitoClient;
    private readonly IOptions<CognitoSettings> _settings;

    public async Task<SignUpResponse> RegisterAsync(
        string email,
        string password,
        string fullName,
        CancellationToken cancellationToken)
    {
        var request = new SignUpRequest
        {
            ClientId = _settings.Value.ClientId,
            Username = email,
            Password = password,
            UserAttributes = new List<AttributeType>
            {
                new AttributeType { Name = "email", Value = email },
                new AttributeType { Name = "name", Value = fullName }
            }
        };

        return await _cognitoClient.SignUpAsync(request, cancellationToken);
    }

    public async Task<LoginResponse> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        var request = new InitiateAuthRequest
        {
            ClientId = _settings.Value.ClientId,
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", email },
                { "PASSWORD", password }
            }
        };

        var response = await _cognitoClient.InitiateAuthAsync(request, cancellationToken);

        return new LoginResponse
        {
            AccessToken = response.AuthenticationResult.AccessToken,
            RefreshToken = response.AuthenticationResult.RefreshToken,
            ExpiresIn = response.AuthenticationResult.ExpiresIn
        };
    }
}
```

### S3 Avatar Upload

```csharp
public class AvatarService : IAvatarService
{
    private readonly IAmazonS3 _s3Client;
    private readonly IOptions<S3Settings> _settings;

    public async Task<string> UploadAvatarAsync(
        Guid userId,
        Stream fileStream,
        string contentType,
        CancellationToken cancellationToken)
    {
        var key = $"avatars/{userId}/avatar.{GetExtension(contentType)}";

        var request = new PutObjectRequest
        {
            BucketName = _settings.Value.BucketName,
            Key = key,
            InputStream = fileStream,
            ContentType = contentType,
            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
        };

        await _s3Client.PutObjectAsync(request, cancellationToken);

        return key;
    }

    public async Task<string> GetPresignedUrlAsync(
        string s3Key,
        CancellationToken cancellationToken)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _settings.Value.BucketName,
            Key = s3Key,
            Expires = DateTime.UtcNow.AddMinutes(15)
        };

        return _s3Client.GetPreSignedURL(request);
    }
}
```

---

## Implementation Tasks

### Milestone 1: Project Setup

#### Task 1.1: Create Solution Structure - [ ]

- [ ] Create `FrogEdu.User.Domain` project
- [ ] Create `FrogEdu.User.Application` project
- [ ] Create `FrogEdu.User.Infrastructure` project
- [ ] Create `FrogEdu.User.API` project
- [ ] Configure project references
- [ ] Add NuGet packages (EF Core, MediatR, FluentValidation, AWS SDKs)

**Validation:**

- [ ] Solution builds successfully
- [ ] All projects reference correct dependencies

#### Task 1.2: Configure appsettings.json - [ ]

```json
{
  "ConnectionStrings": {
    "UserDb": "Server=localhost;Database=UserDB;Trusted_Connection=True;"
  },
  "Cognito": {
    "UserPoolId": "us-east-1_XXX",
    "ClientId": "XXX",
    "Region": "us-east-1"
  },
  "AWS": {
    "S3": {
      "BucketName": "edu-classroom-assets",
      "Region": "us-east-1"
    }
  },
  "Jwt": {
    "Authority": "https://cognito-idp.us-east-1.amazonaws.com/us-east-1_XXX",
    "Audience": "XXX"
  }
}
```

### Milestone 2: Domain Layer

#### Task 2.1: Define Domain Entities - [ ]

- [ ] Create `User` aggregate root
- [ ] Create `Class` aggregate root
- [ ] Create `Enrollment` entity
- [ ] Create `UserRole` enum
- [ ] Create `GradeLevel` enum
- [ ] Create `Entity` base class
- [ ] Create `IAggregateRoot` marker interface

**Validation:**

- [ ] All entities have factory methods
- [ ] Invariants enforced in constructors
- [ ] Domain logic in methods (not anemic)
- [ ] Unit tests for business logic

#### Task 2.2: Define Repository Interfaces - [ ]

- [ ] Create `IUserRepository`
- [ ] Create `IClassRepository`
- [ ] Create `IEnrollmentRepository`
- [ ] Define query methods (GetByEmail, GetByCognitoUserId, etc.)

### Milestone 3: Application Layer

#### Task 3.1: Create DTOs - [ ]

- [ ] `RegisterRequest/Response`
- [ ] `LoginRequest/Response`
- [ ] `UserProfileDto`
- [ ] `ClassDto`
- [ ] `EnrollmentDto`
- [ ] Map all API contracts to DTOs

#### Task 3.2: Create Commands - [ ]

- [ ] `RegisterUserCommand`
- [ ] `VerifyEmailCommand`
- [ ] `LoginCommand`
- [ ] `UpdateProfileCommand`
- [ ] `UploadAvatarCommand`
- [ ] `CreateClassCommand`
- [ ] `EnrollStudentCommand`

#### Task 3.3: Create Queries - [ ]

- [ ] `GetUserProfileQuery`
- [ ] `GetTeacherClassesQuery`
- [ ] `GetClassDetailsQuery`
- [ ] `GetStudentEnrollmentsQuery`

#### Task 3.4: Implement Handlers - [ ]

- [ ] Create MediatR handlers for all commands
- [ ] Create MediatR handlers for all queries
- [ ] Inject repositories and services
- [ ] Implement error handling with Result<T>

**Validation:**

- [ ] Handlers have unit tests
- [ ] Error cases handled
- [ ] Validation logic present

#### Task 3.5: Create Validators - [ ]

- [ ] `RegisterUserCommandValidator`
- [ ] `LoginCommandValidator`
- [ ] `UpdateProfileCommandValidator`
- [ ] `CreateClassCommandValidator`
- [ ] Use FluentValidation rules

**Validation:**

- [ ] All validators have unit tests
- [ ] Email format validated
- [ ] Required fields checked
- [ ] String length limits enforced

### Milestone 4: Infrastructure Layer

#### Task 4.1: Configure DbContext - [ ]

- [ ] Create `UserDbContext`
- [ ] Configure entity mappings
- [ ] Configure global query filters (soft delete)
- [ ] Add audit fields (CreatedAt, UpdatedAt)
- [ ] Seed initial data (if needed)

**Validation:**

- [ ] DbContext configured correctly
- [ ] Migrations can be generated

#### Task 4.2: Create Initial Migration - [ ]

- [ ] Run `dotnet ef migrations add Initial`
- [ ] Review generated migration
- [ ] Ensure indexes are created
- [ ] Ensure constraints are added

**Validation:**

- [ ] Migration applies successfully
- [ ] Schema matches design

#### Task 4.3: Implement Repositories - [ ]

- [ ] `UserRepository`
- [ ] `ClassRepository`
- [ ] `EnrollmentRepository`
- [ ] Implement all interface methods
- [ ] Use `.AsNoTracking()` for read-only queries

**Validation:**

- [ ] Integration tests for repositories
- [ ] CRUD operations work
- [ ] Query methods return correct data

#### Task 4.4: Implement Cognito Service - [ ]

- [ ] Create `CognitoAuthService`
- [ ] Implement `RegisterAsync`
- [ ] Implement `VerifyEmailAsync`
- [ ] Implement `LoginAsync`
- [ ] Implement `RefreshTokenAsync`
- [ ] Implement `ForgotPasswordAsync`
- [ ] Implement `ResetPasswordAsync`
- [ ] Handle Cognito exceptions

**Validation:**

- [ ] Integration tests with Cognito (use test user pool)
- [ ] Error handling works
- [ ] Tokens returned correctly

#### Task 4.5: Implement S3 Service - [ ]

- [ ] Create `AvatarService`
- [ ] Implement `UploadAvatarAsync`
- [ ] Implement `GetPresignedUrlAsync`
- [ ] Validate file type and size
- [ ] Handle S3 exceptions

**Validation:**

- [ ] Integration tests with S3 (use test bucket)
- [ ] Files upload successfully
- [ ] Presigned URLs work

### Milestone 5: API Layer

#### Task 5.1: Create Controllers - [ ]

- [ ] `AuthController` (register, login, refresh, etc.)
- [ ] `ProfileController` (get, update, avatar)
- [ ] `ClassesController` (CRUD, regenerate code)
- [ ] `EnrollmentsController` (enroll, unenroll, my classes)

**Validation:**

- [ ] All endpoints return correct status codes
- [ ] Swagger documentation generated

#### Task 5.2: Configure Authentication - [ ]

- [ ] Add JWT Bearer authentication
- [ ] Configure Cognito as authority
- [ ] Add authorization policies (Teacher, Student)
- [ ] Protect endpoints with `[Authorize]`

**Validation:**

- [ ] Unauthenticated requests return 401
- [ ] Unauthorized requests return 403
- [ ] Role-based access works

#### Task 5.3: Add Middleware - [ ]

- [ ] Global exception handler
- [ ] Request logging
- [ ] Correlation ID propagation
- [ ] Performance monitoring

**Validation:**

- [ ] Exceptions caught and logged
- [ ] ProblemDetails returned for errors
- [ ] Logs include correlation IDs

#### Task 5.4: Configure Swagger - [ ]

- [ ] Add Swagger/OpenAPI
- [ ] Configure JWT bearer auth in Swagger
- [ ] Add XML comments
- [ ] Group endpoints by tags

**Validation:**

- [ ] Swagger UI loads
- [ ] All endpoints documented
- [ ] Test requests work in Swagger

### Milestone 6: Testing

#### Task 6.1: Unit Tests - [ ]

- [ ] Domain entity tests (80%+ coverage)
- [ ] Command/query handler tests
- [ ] Validator tests
- [ ] Mock repositories and services

**Target:** 80%+ code coverage

#### Task 6.2: Integration Tests - [ ]

- [ ] Repository tests (with test database)
- [ ] API endpoint tests (with WebApplicationFactory)
- [ ] Cognito integration tests
- [ ] S3 integration tests

**Validation:**

- [ ] All tests passing
- [ ] Test database isolated
- [ ] Tests run in CI/CD pipeline

### Milestone 7: Deployment

#### Task 7.1: Create Dockerfile - [ ]

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["User.API/FrogEdu.User.API.csproj", "User.API/"]
RUN dotnet restore
COPY . .
WORKDIR "/src/User.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FrogEdu.User.API.dll"]
```

#### Task 7.2: Configure Health Checks - [ ]

- [ ] Add health check endpoint `/health`
- [ ] Check database connectivity
- [ ] Check Cognito connectivity
- [ ] Check S3 connectivity

**Validation:**

- [ ] Health check returns 200 when healthy
- [ ] Returns 503 when unhealthy

#### Task 7.3: Setup Logging - [ ]

- [ ] Configure Serilog
- [ ] Add console sink
- [ ] Add file sink (optional)
- [ ] Add Application Insights sink
- [ ] Structure logs with correlation IDs

---

## Acceptance Criteria

### Definition of Done

#### Functionality

- [ ] Users can register and verify email
- [ ] Users can login and get JWT tokens
- [ ] Token refresh works automatically
- [ ] Users can update profile and avatar
- [ ] Teachers can create and manage classes
- [ ] Students can enroll using invite codes
- [ ] Role-based authorization works

#### Code Quality

- [ ] Clean Architecture maintained
- [ ] All async methods have CancellationToken
- [ ] DTOs used (never domain entities in responses)
- [ ] FluentValidation on all commands
- [ ] No hardcoded secrets
- [ ] No `any` or untyped code

#### Testing

- [ ] Unit tests: 80%+ coverage
- [ ] Integration tests for all endpoints
- [ ] All tests passing in CI/CD

#### Security

- [ ] JWT validation configured
- [ ] Role-based authorization working
- [ ] Input validation comprehensive
- [ ] Secrets in environment variables
- [ ] SQL injection prevented (parameterized queries)

#### Performance

- [ ] API responses < 500ms (p95)
- [ ] Database queries optimized (no N+1)
- [ ] S3 uploads < 5 seconds

#### Documentation

- [ ] Swagger/OpenAPI complete
- [ ] README with setup instructions
- [ ] Architecture diagrams updated

---

**Next Service:** [Assessment Service](./03-assessment-service.md)
