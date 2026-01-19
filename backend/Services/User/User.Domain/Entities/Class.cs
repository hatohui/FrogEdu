using FrogEdu.Shared.Kernel;

namespace FrogEdu.User.Domain.Entities;

/// <summary>
/// Represents a classroom
/// </summary>
public class Class : Entity
{
    public string Name { get; private set; } = default!;
    public short Grade { get; private set; }
    public Guid HomeroomTeacherId { get; private set; }
    public string? School { get; private set; }
    public string? Description { get; private set; }
    public int? MaxStudents { get; private set; }

    private readonly List<Enrollment> _enrollments = new();
    public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();

    private Class() { } // EF Core

    public Class(
        string name,
        short grade,
        Guid homeroomTeacherId,
        string? school = null,
        string? description = null,
        int? maxStudents = null
    )
    {
        if (grade < 1 || grade > 12)
            throw new ArgumentException("Grade must be between 1 and 12", nameof(grade));

        Name = name;
        Grade = grade;
        HomeroomTeacherId = homeroomTeacherId;
        School = school;
        Description = description;
        MaxStudents = maxStudents;
    }

    public void UpdateDetails(
        string name,
        short grade,
        string? school,
        string? description,
        int? maxStudents
    )
    {
        if (grade < 1 || grade > 12)
            throw new ArgumentException("Grade must be between 1 and 12", nameof(grade));

        Name = name;
        Grade = grade;
        School = school;
        Description = description;
        MaxStudents = maxStudents;
        MarkAsUpdated();
    }

    public Enrollment AddStudent(Guid userId)
    {
        if (_enrollments.Any(e => e.UserId == userId && e.Role == EnrollmentRole.Student))
            throw new InvalidOperationException("Student is already enrolled in this class");

        if (
            MaxStudents.HasValue
            && _enrollments.Count(e => e.Role == EnrollmentRole.Student) >= MaxStudents.Value
        )
            throw new InvalidOperationException("Class has reached maximum capacity");

        var enrollment = new Enrollment(userId, Id, EnrollmentRole.Student);
        _enrollments.Add(enrollment);
        MarkAsUpdated();
        return enrollment;
    }

    public Enrollment AddTeacher(Guid userId)
    {
        if (_enrollments.Any(e => e.UserId == userId && e.Role == EnrollmentRole.Teacher))
            throw new InvalidOperationException("Teacher is already assigned to this class");

        var enrollment = new Enrollment(userId, Id, EnrollmentRole.Teacher);
        _enrollments.Add(enrollment);
        MarkAsUpdated();
        return enrollment;
    }
}
