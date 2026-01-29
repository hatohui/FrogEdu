using FrogEdu.Shared.Kernel;

namespace FrogEdu.Exam.Domain.Entities;

/// <summary>
/// Represents a tag for categorizing lessons
/// </summary>
public class Tag : Entity
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }

    private readonly List<LessonTag> _lessons = new();
    public IReadOnlyCollection<LessonTag> Lessons => _lessons.AsReadOnly();

    private Tag() { } // EF Core

    public Tag(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }

    public void UpdateDetails(string name, string? description)
    {
        Name = name;
        Description = description;
        MarkAsUpdated();
    }
}

/// <summary>
/// Join entity for many-to-many relationship between Lesson and Tag
/// </summary>
public class LessonTag
{
    public Guid LessonId { get; set; }
    public Guid TagId { get; set; }
    public Lesson Lesson { get; set; } = default!;
    public Tag Tag { get; set; } = default!;
}


