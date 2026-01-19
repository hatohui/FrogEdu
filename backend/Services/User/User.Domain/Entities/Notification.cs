using FrogEdu.Shared.Kernel;

namespace FrogEdu.User.Domain.Entities;

/// <summary>
/// Represents a notification for a user
/// </summary>
public class Notification : Entity
{
    public Guid UserId { get; private set; }
    public string Type { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    public string Message { get; private set; } = default!;
    public string? Payload { get; private set; } // JSON
    public bool IsRead { get; private set; }
    public DateTime? ReadAt { get; private set; }

    private Notification() { } // EF Core

    public Notification(
        Guid userId,
        string type,
        string title,
        string message,
        string? payload = null
    )
    {
        UserId = userId;
        Type = type;
        Title = title;
        Message = message;
        Payload = payload;
        IsRead = false;
    }

    public void MarkAsRead()
    {
        if (IsRead)
            return;

        IsRead = true;
        ReadAt = DateTime.UtcNow;
        MarkAsUpdated();
    }

    public void MarkAsUnread()
    {
        IsRead = false;
        ReadAt = null;
        MarkAsUpdated();
    }
}
