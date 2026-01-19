using FrogEdu.Shared.Kernel;

namespace FrogEdu.Content.Domain.Entities;

/// <summary>
/// Represents media assets (images, videos, PDFs, audio)
/// </summary>
public class Asset : Entity
{
    public Guid? LessonId { get; private set; }
    public AssetKind Kind { get; private set; }
    public string Uri { get; private set; } = default!;
    public string MimeType { get; private set; } = default!;
    public long ByteSize { get; private set; }
    public string? Title { get; private set; }
    public string? Description { get; private set; }

    private Asset() { } // EF Core

    public Asset(
        AssetKind kind,
        string uri,
        string mimeType,
        long byteSize,
        Guid? lessonId = null,
        string? title = null,
        string? description = null
    )
    {
        Kind = kind;
        Uri = uri;
        MimeType = mimeType;
        ByteSize = byteSize;
        LessonId = lessonId;
        Title = title;
        Description = description;
    }

    public void UpdateMetadata(string? title, string? description)
    {
        Title = title;
        Description = description;
        MarkAsUpdated();
    }
}

public enum AssetKind
{
    Image = 0,
    Video = 1,
    Pdf = 2,
    Audio = 3,
    Other = 99,
}
