using FrogEdu.Shared.Kernel;

namespace FrogEdu.Class.Domain.Entities;

/// <summary>
/// ClassMembership entity - represents a student's enrollment in a class
/// </summary>
public sealed class ClassMembership : Entity
{
    public Guid ClassId { get; private set; }
    public Guid StudentId { get; private set; }
    public DateTime JoinedAt { get; private set; }

    // Navigation property
    public Class Class { get; private set; } = null!;

    private ClassMembership() { }

    private ClassMembership(Guid classId, Guid studentId)
    {
        ClassId = classId;
        StudentId = studentId;
        JoinedAt = DateTime.UtcNow;
    }

    public static ClassMembership Create(Guid classId, Guid studentId)
    {
        return new ClassMembership(classId, studentId);
    }
}
