using FrogEdu.Shared.Kernel;

namespace FrogEdu.AI.Domain.Entities;

/// <summary>
/// Represents events that occur during a tutor session (for analytics)
/// </summary>
public class SessionEvent : Entity
{
    public Guid SessionId { get; private set; }
    public string EventType { get; private set; } = default!;
    public string? Payload { get; private set; } // JSON

    private SessionEvent() { } // EF Core

    public SessionEvent(Guid sessionId, string eventType, string? payload = null)
    {
        SessionId = sessionId;
        EventType = eventType;
        Payload = payload;
    }
}
