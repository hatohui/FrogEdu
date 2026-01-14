# Milestone 2: Class Management

**Feature:** Class Creation & Enrollment  
**Epic:** Class Management  
**Priority:** P1 (High)  
**Estimated Effort:** 12-16 hours  
**Status:** 🔄 Ready for Implementation

---

## Table of Contents

1. [Feature Overview](#1-feature-overview)
2. [Domain Model](#2-domain-model)
3. [Database Schema](#3-database-schema)
4. [Application Layer](#4-application-layer)
5. [API Endpoints](#5-api-endpoints)
6. [Implementation Tasks](#6-implementation-tasks)
7. [Validation Checklist](#7-validation-checklist)

---

## 1. Feature Overview

### User Stories

**US-2.1: Teacher Creates Class**  
_As a_ teacher  
_I want to_ create a new class with a unique invite code  
_So that_ students can join my class

**US-2.2: Student Joins Class**  
_As a_ student  
_I want to_ join a class using an invite code  
_So that_ I can access learning materials

**US-2.3: Teacher Views Class Roster**  
_As a_ teacher  
_I want to_ view all students enrolled in my class  
_So that_ I can track attendance and engagement

**US-2.4: Student Views My Classes**  
_As a_ student  
_I want to_ see all classes I'm enrolled in  
_So that_ I can access materials quickly

**US-2.5: Teacher Archives Class**  
_As a_ teacher  
_I want to_ archive a class at the end of semester  
_So that_ it's no longer active but data is preserved

### Acceptance Criteria

- ✅ Teacher can create class with name, subject, grade level
- ✅ System generates unique 6-character invite code (uppercase alphanumeric)
- ✅ Invite code expires after 30 days (configurable)
- ✅ Student can join class with valid invite code
- ✅ Student cannot join same class twice
- ✅ Teacher can view all enrolled students
- ✅ Student can view all enrolled classes
- ✅ Teacher can remove students from class
- ✅ Teacher can archive class (soft delete)

---

## 2. Domain Model

### Domain Entities

#### `Class` (Aggregate Root)

```csharp
namespace FrogEdu.User.Domain.Entities;

public class Class : Entity
{
    public ClassName Name { get; private set; } // Value object
    public Subject Subject { get; private set; } // Value object
    public GradeLevel GradeLevel { get; private set; } // Value object
    public InviteCode InviteCode { get; private set; } // Value object
    public DateTime InviteCodeExpiresAt { get; private set; }
    public Guid TeacherId { get; private set; } // Foreign key to User
    public bool IsArchived { get; private set; }

    // Collections
    private readonly List<ClassMembership> _memberships = new();
    public IReadOnlyList<ClassMembership> Memberships => _memberships.AsReadOnly();

    // Factory method
    public static Class Create(
        string name,
        string subject,
        int gradeLevel,
        Guid teacherId)
    {
        var classEntity = new Class
        {
            Name = ClassName.Create(name),
            Subject = Subject.Create(subject),
            GradeLevel = GradeLevel.Create(gradeLevel),
            InviteCode = InviteCode.Generate(),
            InviteCodeExpiresAt = DateTime.UtcNow.AddDays(30),
            TeacherId = teacherId,
            IsArchived = false
        };

        classEntity.AddDomainEvent(new ClassCreatedDomainEvent(
            classEntity.Id,
            classEntity.Name.Value,
            teacherId
        ));

        return classEntity;
    }

    public Result EnrollStudent(Guid studentId)
    {
        if (IsArchived)
            return Result.Failure("Cannot enroll in archived class");

        if (_memberships.Any(m => m.UserId == studentId && m.IsActive))
            return Result.Failure("Student already enrolled");

        var membership = ClassMembership.Create(studentId, Id, "Student");
        _memberships.Add(membership);

        AddDomainEvent(new StudentEnrolledDomainEvent(Id, studentId));
        MarkAsUpdated();

        return Result.Success();
    }

    public Result RemoveStudent(Guid studentId)
    {
        var membership = _memberships.FirstOrDefault(m =>
            m.UserId == studentId && m.IsActive);

        if (membership == null)
            return Result.Failure("Student not enrolled");

        membership.Deactivate();
        AddDomainEvent(new StudentRemovedDomainEvent(Id, studentId));
        MarkAsUpdated();

        return Result.Success();
    }

    public void RegenerateInviteCode()
    {
        InviteCode = InviteCode.Generate();
        InviteCodeExpiresAt = DateTime.UtcNow.AddDays(30);
        MarkAsUpdated();
    }

    public void Archive()
    {
        IsArchived = true;
        MarkAsUpdated();
        AddDomainEvent(new ClassArchivedDomainEvent(Id));
    }

    public bool IsInviteCodeValid()
    {
        return !IsArchived && InviteCodeExpiresAt > DateTime.UtcNow;
    }
}
```

#### `ClassMembership` (Entity)

```csharp
public class ClassMembership : Entity
{
    public Guid UserId { get; private set; }
    public Guid ClassId { get; private set; }
    public string Role { get; private set; } // "Teacher" or "Student"
    public DateTime JoinedAt { get; private set; }
    public bool IsActive { get; private set; }

    public static ClassMembership Create(Guid userId, Guid classId, string role)
    {
        return new ClassMembership
        {
            UserId = userId,
            ClassId = classId,
            Role = role,
            JoinedAt = DateTime.UtcNow,
            IsActive = true
        };
    }

    public void Deactivate()
    {
        IsActive = false;
        MarkAsUpdated();
    }
}
```

### Value Objects

```csharp
// ClassName.cs
public class ClassName : ValueObject
{
    public string Value { get; private set; }

    private ClassName(string value) => Value = value;

    public static ClassName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Class name cannot be empty");

        if (value.Length > 100)
            throw new DomainException("Class name cannot exceed 100 characters");

        return new ClassName(value.Trim());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

// Subject.cs
public class Subject : ValueObject
{
    public string Value { get; private set; }

    private Subject(string value) => Value = value;

    public static Subject Create(string value)
    {
        var validSubjects = new[] { "Math", "Science", "English", "History", "Geography", "Arts" };

        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Subject cannot be empty");

        if (!validSubjects.Contains(value, StringComparer.OrdinalIgnoreCase))
            throw new DomainException($"Invalid subject. Must be one of: {string.Join(", ", validSubjects)}");

        return new Subject(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

// GradeLevel.cs
public class GradeLevel : ValueObject
{
    public int Value { get; private set; }

    private GradeLevel(int value) => Value = value;

    public static GradeLevel Create(int value)
    {
        if (value < 1 || value > 12)
            throw new DomainException("Grade level must be between 1 and 12");

        return new GradeLevel(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

// InviteCode.cs
public class InviteCode : ValueObject
{
    public string Value { get; private set; }

    private InviteCode(string value) => Value = value;

    public static InviteCode Generate()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var code = new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        return new InviteCode(code);
    }

    public static InviteCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 6)
            throw new DomainException("Invite code must be 6 characters");

        return new InviteCode(value.ToUpperInvariant());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
```

### Domain Events

```csharp
public record ClassCreatedDomainEvent(Guid ClassId, string ClassName, Guid TeacherId) : DomainEvent;

public record StudentEnrolledDomainEvent(Guid ClassId, Guid StudentId) : DomainEvent;

public record StudentRemovedDomainEvent(Guid ClassId, Guid StudentId) : DomainEvent;

public record ClassArchivedDomainEvent(Guid ClassId) : DomainEvent;
```

---

## 3. Database Schema

### UserDB Tables (Extends User Service)

#### `Classes` Table

```sql
CREATE TABLE [dbo].[Classes] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(100) NOT NULL,
    [Subject] NVARCHAR(50) NOT NULL,
    [GradeLevel] INT NOT NULL,
    [InviteCode] NVARCHAR(6) NOT NULL,
    [InviteCodeExpiresAt] DATETIME2 NOT NULL,
    [TeacherId] UNIQUEIDENTIFIER NOT NULL,
    [IsArchived] BIT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [FK_Classes_TeacherId] FOREIGN KEY ([TeacherId])
        REFERENCES [Users]([Id]) ON DELETE CASCADE,
    CONSTRAINT [CK_Classes_GradeLevel] CHECK ([GradeLevel] BETWEEN 1 AND 12),
    CONSTRAINT [CK_Classes_Subject] CHECK ([Subject] IN ('Math', 'Science', 'English', 'History', 'Geography', 'Arts'))
);

-- Indexes
CREATE UNIQUE INDEX [IX_Classes_InviteCode] ON [Classes]([InviteCode])
    WHERE [IsDeleted] = 0 AND [IsArchived] = 0;
CREATE INDEX [IX_Classes_TeacherId] ON [Classes]([TeacherId]) WHERE [IsDeleted] = 0;
CREATE INDEX [IX_Classes_Subject] ON [Classes]([Subject]) WHERE [IsDeleted] = 0;
```

#### `ClassMemberships` Table (Updated from User feature)

```sql
-- Already created in 01-auth-user-feature, add indexes
CREATE INDEX [IX_ClassMemberships_UserId_IsActive] ON [ClassMemberships]([UserId], [IsActive]);
CREATE INDEX [IX_ClassMemberships_ClassId_IsActive] ON [ClassMemberships]([ClassId], [IsActive]);
```

### EF Core Configuration

```csharp
public class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.ToTable("Classes");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasConversion(
                name => name.Value,
                value => ClassName.Create(value))
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Subject)
            .HasConversion(
                subject => subject.Value,
                value => Subject.Create(value))
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.GradeLevel)
            .HasConversion(
                grade => grade.Value,
                value => GradeLevel.Create(value))
            .IsRequired();

        builder.Property(c => c.InviteCode)
            .HasConversion(
                code => code.Value,
                value => InviteCode.Create(value))
            .HasMaxLength(6)
            .IsRequired();

        builder.HasIndex(c => c.InviteCode)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0 AND [IsArchived] = 0");

        builder.Property(c => c.InviteCodeExpiresAt).IsRequired();
        builder.Property(c => c.TeacherId).IsRequired();
        builder.Property(c => c.IsArchived).HasDefaultValue(false);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.HasMany(c => c.Memberships)
            .WithOne()
            .HasForeignKey("ClassId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(c => c.DomainEvents);
    }
}
```

---

## 4. Application Layer

### Commands

#### `CreateClassCommand`

```csharp
public record CreateClassCommand(
    string Name,
    string Subject,
    int GradeLevel,
    Guid TeacherId
) : IRequest<Result<ClassDto>>;

public class CreateClassCommandValidator : AbstractValidator<CreateClassCommand>
{
    public CreateClassCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Class name is required")
            .MaximumLength(100);

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Subject is required")
            .Must(subject => new[] { "Math", "Science", "English", "History", "Geography", "Arts" }
                .Contains(subject, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Invalid subject");

        RuleFor(x => x.GradeLevel)
            .InclusiveBetween(1, 12).WithMessage("Grade level must be between 1 and 12");
    }
}

public class CreateClassCommandHandler : IRequestHandler<CreateClassCommand, Result<ClassDto>>
{
    private readonly IClassRepository _classRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<CreateClassCommandHandler> _logger;

    public async Task<Result<ClassDto>> Handle(CreateClassCommand request, CancellationToken ct)
    {
        // Validate teacher exists
        var teacher = await _userRepository.GetByIdAsync(request.TeacherId, ct);
        if (teacher == null || teacher.Role != UserRole.Teacher)
        {
            return Result.Failure<ClassDto>("Only teachers can create classes");
        }

        var classEntity = Class.Create(
            request.Name,
            request.Subject,
            request.GradeLevel,
            request.TeacherId
        );

        await _classRepository.AddAsync(classEntity, ct);
        await _classRepository.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Class created. ClassId={ClassId}, Name={Name}, TeacherId={TeacherId}",
            classEntity.Id, classEntity.Name.Value, request.TeacherId
        );

        return Result.Success(ClassDto.FromEntity(classEntity));
    }
}
```

#### `JoinClassCommand`

```csharp
public record JoinClassCommand(
    Guid StudentId,
    string InviteCode
) : IRequest<Result<ClassDto>>;

public class JoinClassCommandHandler : IRequestHandler<JoinClassCommand, Result<ClassDto>>
{
    private readonly IClassRepository _classRepository;
    private readonly IUserRepository _userRepository;

    public async Task<Result<ClassDto>> Handle(JoinClassCommand request, CancellationToken ct)
    {
        // Validate student exists
        var student = await _userRepository.GetByIdAsync(request.StudentId, ct);
        if (student == null || student.Role != UserRole.Student)
        {
            return Result.Failure<ClassDto>("Only students can join classes");
        }

        // Find class by invite code
        var classEntity = await _classRepository.FindByInviteCodeAsync(request.InviteCode, ct);
        if (classEntity == null)
        {
            return Result.Failure<ClassDto>("Invalid invite code");
        }

        // Validate invite code not expired
        if (!classEntity.IsInviteCodeValid())
        {
            return Result.Failure<ClassDto>("Invite code expired or class archived");
        }

        // Enroll student
        var enrollResult = classEntity.EnrollStudent(request.StudentId);
        if (!enrollResult.IsSuccess)
        {
            return Result.Failure<ClassDto>(enrollResult.Error);
        }

        await _classRepository.UpdateAsync(classEntity, ct);
        await _classRepository.SaveChangesAsync(ct);

        return Result.Success(ClassDto.FromEntity(classEntity));
    }
}
```

### Queries

#### `GetClassesByTeacherQuery`

```csharp
public record GetClassesByTeacherQuery(Guid TeacherId) : IRequest<Result<List<ClassDto>>>;

public class GetClassesByTeacherQueryHandler : IRequestHandler<GetClassesByTeacherQuery, Result<List<ClassDto>>>
{
    private readonly IClassRepository _classRepository;

    public async Task<Result<List<ClassDto>>> Handle(GetClassesByTeacherQuery request, CancellationToken ct)
    {
        var classes = await _classRepository.GetByTeacherIdAsync(request.TeacherId, ct);
        var dtos = classes.Select(ClassDto.FromEntity).ToList();
        return Result.Success(dtos);
    }
}
```

#### `GetClassesByStudentQuery`

```csharp
public record GetClassesByStudentQuery(Guid StudentId) : IRequest<Result<List<ClassDto>>>;

public class GetClassesByStudentQueryHandler : IRequestHandler<GetClassesByStudentQuery, Result<List<ClassDto>>>
{
    private readonly IClassRepository _classRepository;

    public async Task<Result<List<ClassDto>>> Handle(GetClassesByStudentQuery request, CancellationToken ct)
    {
        var classes = await _classRepository.GetByStudentIdAsync(request.StudentId, ct);
        var dtos = classes.Select(ClassDto.FromEntity).ToList();
        return Result.Success(dtos);
    }
}
```

### DTOs

```csharp
public record ClassDto(
    Guid Id,
    string Name,
    string Subject,
    int GradeLevel,
    string InviteCode,
    DateTime InviteCodeExpiresAt,
    Guid TeacherId,
    int EnrolledStudentsCount,
    bool IsArchived,
    DateTime CreatedAt
)
{
    public static ClassDto FromEntity(Class classEntity) => new(
        classEntity.Id,
        classEntity.Name.Value,
        classEntity.Subject.Value,
        classEntity.GradeLevel.Value,
        classEntity.InviteCode.Value,
        classEntity.InviteCodeExpiresAt,
        classEntity.TeacherId,
        classEntity.Memberships.Count(m => m.IsActive && m.Role == "Student"),
        classEntity.IsArchived,
        classEntity.CreatedAt
    );
}
```

---

## 5. API Endpoints

### `ClassesController.cs`

```csharp
[ApiController]
[Route("api/classes")]
[Authorize]
public class ClassesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClassesController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Create a new class (Teacher only)
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "TeacherOnly")]
    [ProducesResponseType(typeof(ClassDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateClassCommand command, CancellationToken ct)
    {
        // Extract teacher ID from JWT claims
        var cognitoId = User.FindFirstValue("sub");
        var teacher = await _mediator.Send(new GetUserByCognitoIdQuery(cognitoId!), ct);

        if (!teacher.IsSuccess)
            return Unauthorized();

        var result = await _mediator.Send(command with { TeacherId = teacher.Value.Id }, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : BadRequest(result.Error);
    }

    /// <summary>
    /// Join a class with invite code (Student only)
    /// </summary>
    [HttpPost("join")]
    [Authorize(Policy = "StudentOnly")]
    [ProducesResponseType(typeof(ClassDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Join([FromBody] JoinClassCommand command, CancellationToken ct)
    {
        var cognitoId = User.FindFirstValue("sub");
        var student = await _mediator.Send(new GetUserByCognitoIdQuery(cognitoId!), ct);

        if (!student.IsSuccess)
            return Unauthorized();

        var result = await _mediator.Send(command with { StudentId = student.Value.Id }, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Get all classes for current teacher
    /// </summary>
    [HttpGet("my-classes")]
    [Authorize]
    [ProducesResponseType(typeof(List<ClassDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyClasses(CancellationToken ct)
    {
        var cognitoId = User.FindFirstValue("sub");
        var user = await _mediator.Send(new GetUserByCognitoIdQuery(cognitoId!), ct);

        if (!user.IsSuccess)
            return Unauthorized();

        var result = user.Value.Role == UserRole.Teacher
            ? await _mediator.Send(new GetClassesByTeacherQuery(user.Value.Id), ct)
            : await _mediator.Send(new GetClassesByStudentQuery(user.Value.Id), ct);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Get class by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ClassDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var query = new GetClassByIdQuery(id);
        var result = await _mediator.Send(query, ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }
}
```

---

## 6. Implementation Tasks

### Task 2.1: Domain Layer ⏸️

- [ ] **2.1.1** Create `Class` entity with factory method
- [ ] **2.1.2** Create `ClassMembership` entity
- [ ] **2.1.3** Create value objects: `ClassName`, `Subject`, `GradeLevel`, `InviteCode`
- [ ] **2.1.4** Create domain events
- [ ] **2.1.5** Create `IClassRepository` interface
- [ ] **2.1.6** Write unit tests for domain logic

### Task 2.2: Infrastructure Layer ⏸️

- [ ] **2.2.1** Update `UserDbContext` to include `DbSet<Class>`
- [ ] **2.2.2** Create `ClassConfiguration` for EF Core mapping
- [ ] **2.2.3** Create migration: `dotnet ef migrations add AddClasses`
- [ ] **2.2.4** Apply migration
- [ ] **2.2.5** Implement `ClassRepository`
- [ ] **2.2.6** Write integration tests

### Task 2.3: Application Layer ⏸️

- [ ] **2.3.1** Create `CreateClassCommand` with handler
- [ ] **2.3.2** Create `JoinClassCommand` with handler
- [ ] **2.3.3** Create queries (GetByTeacher, GetByStudent, GetById)
- [ ] **2.3.4** Create `ClassDto`
- [ ] **2.3.5** Add validators
- [ ] **2.3.6** Write unit tests

### Task 2.4: API Layer ⏸️

- [ ] **2.4.1** Create `ClassesController`
- [ ] **2.4.2** Implement `POST /api/classes`
- [ ] **2.4.3** Implement `POST /api/classes/join`
- [ ] **2.4.4** Implement `GET /api/classes/my-classes`
- [ ] **2.4.5** Implement `GET /api/classes/{id}`
- [ ] **2.4.6** Add authorization policies
- [ ] **2.4.7** Write API tests

## 7. Validation Checklist

- [ ] Teacher can create class
- [ ] Invite code generated (6 chars, unique)
- [ ] Student can join with valid invite code
- [ ] Expired invite codes rejected
- [ ] Duplicate enrollment prevented
- [ ] Teacher sees all their classes
- [ ] Student sees enrolled classes
- [ ] Class archived successfully

---

**Milestone Status:** Ready for Implementation ✅  
**Blocked By:** 01-auth-user-feature  
**Blocking:** 03-content-library-feature  
**Estimated Completion:** 12-16 hours
