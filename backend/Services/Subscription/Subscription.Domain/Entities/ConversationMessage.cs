using FrogEdu.Shared.Kernel;
using FrogEdu.Shared.Kernel.Exceptions;

namespace FrogEdu.Subscription.Domain.Entities;

/// <summary>
/// ConversationMessage entity representing a single message in a tutor session
/// </summary>
public class ConversationMessage : Entity
{
    public Guid TutorSessionId { get; private set; }
    public string Role { get; private set; } = default!; // "user" or "assistant"
    public string Content { get; private set; } = default!;
    public int TokenCount { get; private set; }
    public DateTime Timestamp { get; private set; }

    // Navigation property
    public TutorSession TutorSession { get; private set; } = null!;

    private ConversationMessage() { } // For EF Core

    private ConversationMessage(Guid tutorSessionId, string role, string content, int tokenCount)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new ValidationException(nameof(Role), "Role cannot be empty");

        if (role != "user" && role != "assistant")
            throw new ValidationException(nameof(Role), "Role must be 'user' or 'assistant'");

        if (string.IsNullOrWhiteSpace(content))
            throw new ValidationException(nameof(Content), "Content cannot be empty");

        if (tokenCount < 0)
            throw new ValidationException(nameof(TokenCount), "Token count cannot be negative");

        TutorSessionId = tutorSessionId;
        Role = role;
        Content = content;
        TokenCount = tokenCount;
        Timestamp = DateTime.UtcNow;
    }

    public static ConversationMessage Create(
        Guid tutorSessionId,
        string role,
        string content,
        int tokenCount
    )
    {
        return new ConversationMessage(tutorSessionId, role, content, tokenCount);
    }
}
