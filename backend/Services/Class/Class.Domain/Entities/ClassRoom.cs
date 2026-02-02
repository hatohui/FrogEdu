using FrogEdu.Class.Domain.Events;
using FrogEdu.Class.Domain.ValueObjects;
using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Class.Domain.Entities;

public sealed class ClassRoom : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string Grade { get; private set; } = null!;
    public InviteCode InviteCode { get; private set; } = null!;
    public int MaxStudents { get; private set; }
    public string? BannerUrl { get; private set; }
    public bool IsActive { get; private set; }
    public Guid TeacherId { get; private set; }

    private readonly List<ClassEnrollment> _enrollments = new();
    public IReadOnlyCollection<ClassEnrollment> Enrollments => _enrollments.AsReadOnly();

    private readonly List<Assignment> _assignments = new();
    public IReadOnlyCollection<Assignment> Assignments => _assignments.AsReadOnly();

    private ClassRoom() { }

    private ClassRoom(
        string name,
        string grade,
        int maxStudents,
        Guid teacherId,
        string? bannerUrl = null
    )
    {
        Name = name;
        Grade = grade;
        MaxStudents = maxStudents;
        TeacherId = teacherId;
        BannerUrl = bannerUrl;
        InviteCode = ValueObjects.InviteCode.Generate();
        IsActive = true;
    }

    public static ClassRoom Create(
        string name,
        string grade,
        int maxStudents,
        Guid teacherId,
        string userId,
        string? bannerUrl = null
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(grade))
            throw new ArgumentException("Grade cannot be empty", nameof(grade));
        if (maxStudents <= 0)
            throw new ArgumentException("Max students must be greater than 0", nameof(maxStudents));
        if (teacherId == Guid.Empty)
            throw new ArgumentException("Teacher ID cannot be empty", nameof(teacherId));

        var classroom = new ClassRoom(name, grade, maxStudents, teacherId, bannerUrl);
        classroom.MarkAsCreated();
        classroom.AddDomainEvent(
            new ClassRoomCreatedDomainEvent(
                classroom.Id,
                classroom.Name,
                classroom.InviteCode.Value
            )
        );
        return classroom;
    }

    public void Update(string name, string grade, int maxStudents, string? bannerUrl, string userId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(grade))
            throw new ArgumentException("Grade cannot be empty", nameof(grade));
        if (maxStudents <= 0)
            throw new ArgumentException("Max students must be greater than 0", nameof(maxStudents));

        Name = name;
        Grade = grade;
        MaxStudents = maxStudents;
        BannerUrl = bannerUrl;
        MarkAsUpdated();
    }

    public void RegenerateCode()
    {
        InviteCode = ValueObjects.InviteCode.Generate();
        AddDomainEvent(new InviteCodeRegeneratedDomainEvent(Id, InviteCode.Value));
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public bool CanEnrollStudent()
    {
        var activeEnrollments = _enrollments.Count(e => e.Status == Enums.EnrollmentStatus.Active);
        return IsActive && activeEnrollments < MaxStudents;
    }

    public void EnrollStudent(Guid studentId)
    {
        if (!CanEnrollStudent())
            throw new InvalidOperationException(
                "Cannot enroll student: classroom is full or inactive"
            );

        if (
            _enrollments.Any(e =>
                e.StudentId == studentId && e.Status == Enums.EnrollmentStatus.Active
            )
        )
            throw new InvalidOperationException("Student is already enrolled in this classroom");

        var enrollment = ClassEnrollment.Create(Id, studentId);
        _enrollments.Add(enrollment);
        AddDomainEvent(new StudentEnrolledDomainEvent(Id, studentId));
    }

    public void AddAssignment(Assignment assignment)
    {
        _assignments.Add(assignment);
    }
}
