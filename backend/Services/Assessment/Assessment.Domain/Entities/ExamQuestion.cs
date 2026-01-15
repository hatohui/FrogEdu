using FrogEdu.Assessment.Domain.Enums;
using FrogEdu.Assessment.Domain.ValueObjects;
using FrogEdu.Shared.Kernel;
using FrogEdu.Shared.Kernel.Exceptions;

namespace FrogEdu.Assessment.Domain.Entities;

/// <summary>
/// Join entity representing a question in an exam with metadata
/// </summary>
public class ExamQuestion : Entity
{
    public Guid ExamPaperId { get; private set; }
    public Guid QuestionId { get; private set; }
    public int OrderIndex { get; private set; }
    public decimal Points { get; private set; }

    // Navigation properties
    public ExamPaper ExamPaper { get; private set; } = null!;
    public Question Question { get; private set; } = null!;

    private ExamQuestion() { } // For EF Core

    private ExamQuestion(Guid examPaperId, Guid questionId, int orderIndex, decimal points)
    {
        if (orderIndex < 0)
            throw new ValidationException(nameof(OrderIndex), "Order index cannot be negative");

        if (points <= 0)
            throw new ValidationException(nameof(Points), "Points must be greater than 0");

        ExamPaperId = examPaperId;
        QuestionId = questionId;
        OrderIndex = orderIndex;
        Points = points;
    }

    public static ExamQuestion Create(
        Guid examPaperId,
        Guid questionId,
        int orderIndex,
        decimal points
    )
    {
        return new ExamQuestion(examPaperId, questionId, orderIndex, points);
    }

    public void UpdateOrder(int orderIndex)
    {
        if (orderIndex < 0)
            throw new ValidationException(nameof(OrderIndex), "Order index cannot be negative");

        OrderIndex = orderIndex;
        UpdateTimestamp();
    }

    public void UpdatePoints(decimal points)
    {
        if (points <= 0)
            throw new ValidationException(nameof(Points), "Points must be greater than 0");

        Points = points;
        UpdateTimestamp();
    }
}
