using FrogEdu.Exam.Domain.Enums;
using FrogEdu.Exam.Domain.ValueObjects;
using FrogEdu.Shared.Kernel;
using FrogEdu.Shared.Kernel.Exceptions;

namespace FrogEdu.Exam.Domain.Entities;

/// <summary>
/// Chapter entity representing a chapter within a textbook
/// </summary>
public class Chapter : Entity
{
    public int ChapterNumber { get; private set; }
    public string Title { get; private set; } = default!;
    public string? Description { get; private set; }
    public Guid TextbookId { get; private set; }

    // Navigation properties
    public Textbook Textbook { get; private set; } = null!;
    private readonly List<Page> _pages = new();
    public IReadOnlyCollection<Page> Pages => _pages.AsReadOnly();

    private Chapter() { } // For EF Core

    private Chapter(int chapterNumber, string title, string? description, Guid textbookId)
    {
        if (chapterNumber <= 0)
            throw new ValidationException(
                nameof(ChapterNumber),
                "Chapter number must be greater than 0"
            );

        if (string.IsNullOrWhiteSpace(title))
            throw new ValidationException(nameof(Title), "Chapter title cannot be empty");

        ChapterNumber = chapterNumber;
        Title = title;
        Description = description;
        TextbookId = textbookId;
    }

    public static Chapter Create(
        int chapterNumber,
        string title,
        string? description,
        Guid textbookId
    )
    {
        return new Chapter(chapterNumber, title, description, textbookId);
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ValidationException(nameof(Title), "Chapter title cannot be empty");

        Title = title;
        UpdateTimestamp();
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
        UpdateTimestamp();
    }

    public Page AddPage(int pageNumber, string s3Key, string? description)
    {
        // Check if page number already exists
        if (_pages.Any(p => p.PageNumber == pageNumber))
            throw new BusinessRuleViolationException(
                $"Page number {pageNumber} already exists in chapter {ChapterNumber}"
            );

        var page = Page.Create(pageNumber, s3Key, description, Id);
        _pages.Add(page);
        UpdateTimestamp();
        return page;
    }

    public void RemovePage(Guid pageId)
    {
        var page = _pages.FirstOrDefault(p => p.Id == pageId);
        if (page == null)
            throw new NotFoundException(nameof(Page), pageId);

        _pages.Remove(page);
        UpdateTimestamp();
    }
}
