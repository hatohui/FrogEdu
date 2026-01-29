using FrogEdu.Shared.Kernel;

namespace FrogEdu.Exam.Domain.Entities;

/// <summary>
/// Represents a lesson within a chapter
/// </summary>
public class Lesson : Entity
{
    public Guid ChapterId { get; private set; }
    public int OrderIndex { get; private set; }
    public string Title { get; private set; } = default!;
    public string? Summary { get; private set; }
    public int? DurationMinutes { get; private set; }
    public string? LearningObjectives { get; private set; }

    private readonly List<Page> _pages = new();
    public IReadOnlyCollection<Page> Pages => _pages.AsReadOnly();

    private readonly List<LessonTag> _tags = new();
    public IReadOnlyCollection<LessonTag> Tags => _tags.AsReadOnly();

    private Lesson() { } // EF Core

    public Lesson(
        Guid chapterId,
        string title,
        int orderIndex,
        string? summary = null,
        int? durationMinutes = null
    )
    {
        ChapterId = chapterId;
        Title = title;
        OrderIndex = orderIndex;
        Summary = summary;
        DurationMinutes = durationMinutes;
    }

    public void UpdateDetails(
        string title,
        string? summary,
        int orderIndex,
        int? durationMinutes,
        string? learningObjectives
    )
    {
        Title = title;
        Summary = summary;
        OrderIndex = orderIndex;
        DurationMinutes = durationMinutes;
        LearningObjectives = learningObjectives;
        MarkAsUpdated();
    }

    public Page AddPage(
        int orderIndex,
        string? contentMarkdown = null,
        string? contentHtml = null,
        string? mediaUrl = null
    )
    {
        // For now, create a simple page with S3Key (simplified approach)
        // In production, you'd upload to S3 first and get the S3Key
        var s3Key = $"lessons/{Id}/pages/{orderIndex}";
        var page = Page.Create(orderIndex, s3Key, contentMarkdown, ChapterId, Id);
        _pages.Add(page);
        MarkAsUpdated();
        return page;
    }

    public void AddTag(Guid tagId)
    {
        if (_tags.Any(t => t.TagId == tagId))
            return;

        _tags.Add(new LessonTag { LessonId = Id, TagId = tagId });
        MarkAsUpdated();
    }
}


