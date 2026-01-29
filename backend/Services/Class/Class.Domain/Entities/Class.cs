using FrogEdu.Shared.Kernel;

namespace FrogEdu.Class.Domain.Entities;

/// <summary>
/// Class aggregate root - represents a classroom with teacher and students
/// </summary>
public sealed class Class : Entity
{
    public Guid TeacherId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Subject { get; private set; } = null!;
    public int GradeLevel { get; private set; }
    public string InviteCode { get; private set; } = null!;
    public DateTime? InviteCodeExpiry { get; private set; }
    public bool IsArchived { get; private set; }

    private readonly List<ClassMembership> _memberships = new();
    public IReadOnlyCollection<ClassMembership> Memberships => _memberships.AsReadOnly();

    private Class() { }

    private Class(Guid teacherId, string name, string subject, int gradeLevel)
    {
        TeacherId = teacherId;
        Name = name;
        Subject = subject;
        GradeLevel = gradeLevel;
        InviteCode = GenerateInviteCode();
        InviteCodeExpiry = DateTime.UtcNow.AddDays(30);
        IsArchived = false;
    }

    public static Class Create(Guid teacherId, string name, string subject, int gradeLevel)
    {
        return new Class(teacherId, name, subject, gradeLevel);
    }

    public void EnrollStudent(Guid studentId)
    {
        if (_memberships.Any(m => m.StudentId == studentId))
            return;

        var membership = ClassMembership.Create(Id, studentId);
        _memberships.Add(membership);
    }

    public void RemoveStudent(Guid studentId)
    {
        var membership = _memberships.FirstOrDefault(m => m.StudentId == studentId);
        if (membership != null)
            _memberships.Remove(membership);
    }

    public void Archive()
    {
        IsArchived = true;
        UpdateTimestamp();
    }

    public void RegenerateInviteCode()
    {
        InviteCode = GenerateInviteCode();
        InviteCodeExpiry = DateTime.UtcNow.AddDays(30);
        UpdateTimestamp();
    }

    private static string GenerateInviteCode()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        var random = new Random();
        return new string(
            Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray()
        );
    }
}
