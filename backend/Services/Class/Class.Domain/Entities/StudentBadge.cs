using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Class.Domain.Entities;

public sealed class StudentBadge : Entity
{
    public Guid StudentId { get; private set; }
    public Guid BadgeId { get; private set; }
    public Guid ClassId { get; private set; }
    public Guid? ExamSessionId { get; private set; }
    public Guid? AwardedByTeacherId { get; private set; }
    public string? CustomPraise { get; private set; }
    public DateTime AwardedAt { get; private set; }

    private StudentBadge() { }

    private StudentBadge(
        Guid studentId,
        Guid badgeId,
        Guid classId,
        Guid? examSessionId,
        Guid? awardedByTeacherId,
        string? customPraise
    )
    {
        StudentId = studentId;
        BadgeId = badgeId;
        ClassId = classId;
        ExamSessionId = examSessionId;
        AwardedByTeacherId = awardedByTeacherId;
        CustomPraise = customPraise;
        AwardedAt = DateTime.UtcNow;
    }

    public static StudentBadge Create(
        Guid studentId,
        Guid badgeId,
        Guid classId,
        Guid? examSessionId = null,
        Guid? awardedByTeacherId = null,
        string? customPraise = null
    )
    {
        if (studentId == Guid.Empty)
            throw new ArgumentException("Student ID cannot be empty", nameof(studentId));
        if (badgeId == Guid.Empty)
            throw new ArgumentException("Badge ID cannot be empty", nameof(badgeId));
        if (classId == Guid.Empty)
            throw new ArgumentException("Class ID cannot be empty", nameof(classId));

        return new StudentBadge(
            studentId,
            badgeId,
            classId,
            examSessionId,
            awardedByTeacherId,
            customPraise
        );
    }

    public void UpdatePraise(string customPraise)
    {
        CustomPraise = customPraise;
    }
}
