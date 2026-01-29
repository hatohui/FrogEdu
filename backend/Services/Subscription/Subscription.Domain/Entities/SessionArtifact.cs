using FrogEdu.Shared.Kernel;

namespace FrogEdu.Subscription.Domain.Entities;

/// <summary>
/// Represents artifacts generated during a tutor session
/// </summary>
public class SessionArtifact : Entity
{
    public Guid SessionId { get; private set; }
    public ArtifactType ArtifactType { get; private set; }
    public string Uri { get; private set; } = default!;
    public string? Metadata { get; private set; } // JSON

    private SessionArtifact() { } // EF Core

    public SessionArtifact(
        Guid sessionId,
        ArtifactType artifactType,
        string uri,
        string? metadata = null
    )
    {
        SessionId = sessionId;
        ArtifactType = artifactType;
        Uri = uri;
        Metadata = metadata;
    }

    public void UpdateMetadata(string metadata)
    {
        Metadata = metadata;
        MarkAsUpdated();
    }
}

public enum ArtifactType
{
    Summary = 0,
    Quiz = 1,
    Flashcard = 2,
    StudyGuide = 3,
    Other = 99,
}

