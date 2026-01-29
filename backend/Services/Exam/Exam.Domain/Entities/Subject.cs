using FrogEdu.Shared.Kernel;

namespace FrogEdu.Exam.Domain.Entities;

/// <summary>
/// Represents an academic subject (e.g., Math, Literature, Science)
/// </summary>
public class Subject : Entity
{
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public short Grade { get; private set; }
    public string? Description { get; private set; }

    private readonly List<Textbook> _textbooks = new();
    public IReadOnlyCollection<Textbook> Textbooks => _textbooks.AsReadOnly();

    private Subject() { } // EF Core

    public Subject(string code, string name, short grade, string? description = null)
    {
        Code = code;
        Name = name;
        Grade = grade;
        Description = description;
    }

    public void UpdateDetails(string name, string? description)
    {
        Name = name;
        Description = description;
        MarkAsUpdated();
    }
}


