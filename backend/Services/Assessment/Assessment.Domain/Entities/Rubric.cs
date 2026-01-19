using FrogEdu.Shared.Kernel;

namespace FrogEdu.Assessment.Domain.Entities;

/// <summary>
/// Represents a grading rubric for essay/short answer questions
/// </summary>
public class Rubric : Entity
{
    public Guid QuestionId { get; private set; }
    public string Criteria { get; private set; } = default!; // JSON
    public string? Description { get; private set; }

    private Rubric() { } // EF Core

    public Rubric(Guid questionId, string criteria, string? description = null)
    {
        QuestionId = questionId;
        Criteria = criteria;
        Description = description;
    }

    public void UpdateCriteria(string criteria, string? description)
    {
        Criteria = criteria;
        Description = description;
        MarkAsUpdated();
    }
}
