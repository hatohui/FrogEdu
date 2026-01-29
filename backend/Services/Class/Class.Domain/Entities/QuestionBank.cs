using FrogEdu.Shared.Kernel;

namespace FrogEdu.Class.Domain.Entities;

/// <summary>
/// Represents a question bank (collection of questions)
/// </summary>
public class QuestionBank : Entity
{
    public Guid OwnerId { get; private set; }
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public bool IsPublic { get; private set; }

    private readonly List<Question> _questions = new();
    public IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();

    private QuestionBank() { } // EF Core

    public QuestionBank(
        Guid ownerId,
        string name,
        string? description = null,
        bool isPublic = false
    )
    {
        OwnerId = ownerId;
        Name = name;
        Description = description;
        IsPublic = isPublic;
    }

    public void UpdateDetails(string name, string? description, bool isPublic)
    {
        Name = name;
        Description = description;
        IsPublic = isPublic;
        MarkAsUpdated();
    }

    public void AddQuestion(Question question)
    {
        _questions.Add(question);
        MarkAsUpdated();
    }
}
