using FrogEdu.Shared.Kernel;

namespace FrogEdu.Assessment.Domain.Entities;

/// <summary>
/// Represents a section within an exam
/// </summary>
public class ExamSection : Entity
{
    public Guid ExamId { get; private set; }
    public int OrderIndex { get; private set; }
    public string Title { get; private set; } = default!;
    public string? Instructions { get; private set; }

    private readonly List<ExamQuestion> _questions = new();
    public IReadOnlyCollection<ExamQuestion> Questions => _questions.AsReadOnly();

    private ExamSection() { } // EF Core

    public ExamSection(Guid examId, int orderIndex, string title, string? instructions = null)
    {
        ExamId = examId;
        OrderIndex = orderIndex;
        Title = title;
        Instructions = instructions;
    }

    public void UpdateDetails(string title, string? instructions, int orderIndex)
    {
        Title = title;
        Instructions = instructions;
        OrderIndex = orderIndex;
        MarkAsUpdated();
    }
}
