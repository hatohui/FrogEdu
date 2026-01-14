using FrogEdu.Shared.Kernel;
using FrogEdu.Shared.Kernel.Exceptions;

namespace FrogEdu.AI.Domain.Entities;

/// <summary>
/// TutorSession aggregate root representing an AI tutoring conversation
/// </summary>
public class TutorSession : Entity
{
    public Guid StudentId { get; private set; }
    public int GradeLevel { get; private set; }
    public Guid? TextbookId { get; private set; }
    public Guid? ChapterId { get; private set; }
    public int TotalTokensUsed { get; private set; }
    public DateTime LastActivityAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    private readonly List<ConversationMessage> _messages = new();
    public IReadOnlyCollection<ConversationMessage> Messages => _messages.AsReadOnly();

    public int MessageCount => _messages.Count;

    private TutorSession() { } // For EF Core

    private TutorSession(Guid studentId, int gradeLevel, Guid? textbookId, Guid? chapterId)
    {
        if (gradeLevel < 1 || gradeLevel > 5)
            throw new ValidationException(
                nameof(GradeLevel),
                "Grade level must be between 1 and 5"
            );

        StudentId = studentId;
        GradeLevel = gradeLevel;
        TextbookId = textbookId;
        ChapterId = chapterId;
        TotalTokensUsed = 0;
        LastActivityAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddMinutes(60); // 60-minute session timeout
        IsActive = true;
    }

    public static TutorSession Create(
        Guid studentId,
        int gradeLevel,
        Guid? textbookId = null,
        Guid? chapterId = null
    )
    {
        return new TutorSession(studentId, gradeLevel, textbookId, chapterId);
    }

    public ConversationMessage AddMessage(string role, string content, int tokenCount)
    {
        if (!IsActive)
            throw new BusinessRuleViolationException("Cannot add messages to an inactive session");

        if (MessageCount >= 20)
            throw new BusinessRuleViolationException(
                "Session has reached maximum message limit (20)"
            );

        if (IsExpired())
            throw new BusinessRuleViolationException("Session has expired");

        var message = ConversationMessage.Create(Id, role, content, tokenCount);
        _messages.Add(message);

        TotalTokensUsed += tokenCount;
        LastActivityAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddMinutes(60); // Reset expiration on activity

        UpdateTimestamp();
        return message;
    }

    public void End()
    {
        IsActive = false;
        UpdateTimestamp();
    }

    public bool IsExpired()
    {
        return ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
    }

    public void UpdateContext(Guid textbookId, Guid chapterId)
    {
        TextbookId = textbookId;
        ChapterId = chapterId;
        UpdateTimestamp();
    }
}
