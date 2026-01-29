using FrogEdu.Exam.Domain.Enums;
using FrogEdu.Exam.Domain.ValueObjects;
using FrogEdu.Shared.Kernel;
using FrogEdu.Shared.Kernel.Exceptions;

namespace FrogEdu.Exam.Domain.Entities;

/// <summary>
/// Page entity representing a single page in a chapter
/// </summary>
public class Page : Entity
{
    public int PageNumber { get; private set; }
    public string S3Key { get; private set; } = default!;
    public string? Description { get; private set; }
    public Guid ChapterId { get; private set; }
    public Guid? LessonId { get; private set; } // Optional - pages can belong to lessons

    // Navigation property
    public Chapter Chapter { get; private set; } = null!;

    private Page() { } // For EF Core

    private Page(
        int pageNumber,
        string s3Key,
        string? description,
        Guid chapterId,
        Guid? lessonId = null
    )
    {
        if (pageNumber <= 0)
            throw new ValidationException(nameof(PageNumber), "Page number must be greater than 0");

        if (string.IsNullOrWhiteSpace(s3Key))
            throw new ValidationException(nameof(S3Key), "S3 key cannot be empty");

        PageNumber = pageNumber;
        S3Key = s3Key;
        Description = description;
        ChapterId = chapterId;
        LessonId = lessonId;
    }

    public static Page Create(
        int pageNumber,
        string s3Key,
        string? description,
        Guid chapterId,
        Guid? lessonId = null
    )
    {
        return new Page(pageNumber, s3Key, description, chapterId, lessonId);
    }

    public void UpdateS3Key(string s3Key)
    {
        if (string.IsNullOrWhiteSpace(s3Key))
            throw new ValidationException(nameof(S3Key), "S3 key cannot be empty");

        S3Key = s3Key;
        UpdateTimestamp();
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
        UpdateTimestamp();
    }
}
