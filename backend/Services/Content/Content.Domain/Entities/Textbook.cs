using FrogEdu.Content.Domain.Enums;
using FrogEdu.Content.Domain.Events;
using FrogEdu.Content.Domain.ValueObjects;
using FrogEdu.Shared.Kernel;
using FrogEdu.Shared.Kernel.Exceptions;

namespace FrogEdu.Content.Domain.Entities;

/// <summary>
/// Textbook aggregate root representing an educational textbook
/// </summary>
public class Textbook : Entity
{
    public string Title { get; private set; }
    public Subject Subject { get; private set; }
    public GradeLevel GradeLevel { get; private set; }
    public string? Publisher { get; private set; }
    public int? PublicationYear { get; private set; }
    public string? Description { get; private set; }
    public string? CoverImageS3Key { get; private set; }

    // Navigation properties
    private readonly List<Chapter> _chapters = new();
    public IReadOnlyCollection<Chapter> Chapters => _chapters.AsReadOnly();

    private Textbook() { } // For EF Core

    private Textbook(
        string title,
        Subject subject,
        GradeLevel gradeLevel,
        string? publisher,
        int? publicationYear,
        string? description,
        string? coverImageS3Key
    )
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ValidationException(nameof(Title), "Textbook title cannot be empty");

        if (!Enum.IsDefined(typeof(GradeLevel), gradeLevel))
            throw new ValidationException(
                nameof(GradeLevel),
                "Invalid grade level. Must be between 1 and 5"
            );

        Title = title;
        Subject = subject;
        GradeLevel = gradeLevel;
        Publisher = publisher;
        PublicationYear = publicationYear;
        Description = description;
        CoverImageS3Key = coverImageS3Key;
    }

    public static Textbook Create(
        string title,
        Subject subject,
        GradeLevel gradeLevel,
        string? publisher = null,
        int? publicationYear = null,
        string? description = null,
        string? coverImageS3Key = null
    )
    {
        var textbook = new Textbook(
            title,
            subject,
            gradeLevel,
            publisher,
            publicationYear,
            description,
            coverImageS3Key
        );

        // Raise domain event
        // Note: In a real implementation, you'd add this to a domain events collection
        // that gets processed by the infrastructure layer

        return textbook;
    }

    public void UpdateDetails(
        string title,
        string? publisher,
        int? publicationYear,
        string? description
    )
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ValidationException(nameof(Title), "Textbook title cannot be empty");

        Title = title;
        Publisher = publisher;
        PublicationYear = publicationYear;
        Description = description;
        UpdateTimestamp();
    }

    public void UpdateCoverImage(string coverImageS3Key)
    {
        if (string.IsNullOrWhiteSpace(coverImageS3Key))
            throw new ValidationException(
                nameof(CoverImageS3Key),
                "Cover image S3 key cannot be empty"
            );

        CoverImageS3Key = coverImageS3Key;
        UpdateTimestamp();
    }

    public Chapter AddChapter(int chapterNumber, string title, string? description)
    {
        // Check if chapter number already exists
        if (_chapters.Any(c => c.ChapterNumber == chapterNumber))
            throw new BusinessRuleViolationException(
                $"Chapter number {chapterNumber} already exists in textbook '{Title}'"
            );

        var chapter = Chapter.Create(chapterNumber, title, description, Id);
        _chapters.Add(chapter);
        UpdateTimestamp();
        return chapter;
    }

    public void RemoveChapter(Guid chapterId)
    {
        var chapter = _chapters.FirstOrDefault(c => c.Id == chapterId);
        if (chapter == null)
            throw new NotFoundException(nameof(Chapter), chapterId);

        _chapters.Remove(chapter);
        UpdateTimestamp();
    }

    public override string ToString() => $"{Title} - {Subject} (Grade {(int)GradeLevel})";
}
