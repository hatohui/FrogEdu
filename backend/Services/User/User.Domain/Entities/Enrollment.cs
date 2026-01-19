using FrogEdu.Shared.Kernel;

namespace FrogEdu.User.Domain.Entities;

/// <summary>
/// Represents a user's enrollment in a class
/// </summary>
public class Enrollment : Entity
{
    public Guid UserId { get; private set; }
    public Guid ClassId { get; private set; }
    public EnrollmentRole Role { get; private set; }
    public DateTime EnrolledAt { get; private set; }

    private Enrollment() { } // EF Core

    public Enrollment(Guid userId, Guid classId, EnrollmentRole role)
    {
        UserId = userId;
        ClassId = classId;
        Role = role;
        EnrolledAt = DateTime.UtcNow;
    }
}

public enum EnrollmentRole
{
    Student = 0,
    Teacher = 1,
}
