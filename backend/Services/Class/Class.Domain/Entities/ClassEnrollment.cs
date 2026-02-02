using FrogEdu.Class.Domain.Enums;
using FrogEdu.Class.Domain.Events;
using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Class.Domain.Entities;

public sealed class ClassEnrollment : Entity
{
    public Guid ClassId { get; private set; }
    public Guid StudentId { get; private set; }
    public DateTime JoinedAt { get; private set; }
    public EnrollmentStatus Status { get; private set; }

    private ClassEnrollment() { }

    private ClassEnrollment(Guid classId, Guid studentId)
    {
        ClassId = classId;
        StudentId = studentId;
        JoinedAt = DateTime.UtcNow;
        Status = EnrollmentStatus.Active;
    }

    public static ClassEnrollment Create(Guid classId, Guid studentId)
    {
        if (classId == Guid.Empty)
            throw new ArgumentException("Class ID cannot be empty", nameof(classId));
        if (studentId == Guid.Empty)
            throw new ArgumentException("Student ID cannot be empty", nameof(studentId));

        var enrollment = new ClassEnrollment(classId, studentId);
        enrollment.AddDomainEvent(new StudentEnrolledDomainEvent(classId, studentId));
        return enrollment;
    }

    public void KickStudent()
    {
        if (Status == EnrollmentStatus.Kicked)
            throw new InvalidOperationException("Student is already kicked");

        Status = EnrollmentStatus.Kicked;
        AddDomainEvent(new StudentKickedDomainEvent(ClassId, StudentId));
    }

    public void Withdraw()
    {
        if (Status == EnrollmentStatus.Withdrawn)
            throw new InvalidOperationException("Student has already withdrawn");

        Status = EnrollmentStatus.Withdrawn;
    }

    public void Activate()
    {
        if (Status == EnrollmentStatus.Kicked)
            throw new InvalidOperationException("Cannot activate a kicked student");

        Status = EnrollmentStatus.Active;
    }

    public void Deactivate()
    {
        Status = EnrollmentStatus.Inactive;
    }
}
