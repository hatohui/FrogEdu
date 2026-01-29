using FrogEdu.Shared.Kernel;

namespace FrogEdu.Exam.Domain.Entities;

/// <summary>
/// ExamQuestion entity - represents the many-to-many relationship between Exam and Question
/// </summary>
public sealed class ExamQuestion : Entity
{
    public Guid ExamId { get; private set; }
    public Guid QuestionId { get; private set; }
    public int OrderIndex { get; private set; }

    // Navigation properties
    public Exam Exam { get; private set; } = null!;
    public Question Question { get; private set; } = null!;

    private ExamQuestion() { }

    private ExamQuestion(Guid examId, Guid questionId, int orderIndex)
    {
        ExamId = examId;
        QuestionId = questionId;
        OrderIndex = orderIndex;
    }

    public static ExamQuestion Create(Guid examId, Guid questionId, int orderIndex)
    {
        return new ExamQuestion(examId, questionId, orderIndex);
    }

    public void UpdateOrder(int newOrderIndex)
    {
        OrderIndex = newOrderIndex;
        UpdateTimestamp();
    }
}
