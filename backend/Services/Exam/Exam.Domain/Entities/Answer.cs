using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Exam.Domain.Entities;

public sealed class Answer : Entity
{
    public string Content { get; private set; } = null!;
    public bool IsCorrect { get; private set; }
    public double Point { get; private set; }
    public string? Explanation { get; private set; }
    public Guid QuestionId { get; private set; }

    private Answer() { }

    private Answer(
        string content,
        bool isCorrect,
        double point,
        Guid questionId,
        string? explanation = null
    )
    {
        Content = content;
        IsCorrect = isCorrect;
        Point = point;
        QuestionId = questionId;
        Explanation = explanation;
    }

    public static Answer Create(
        string content,
        bool isCorrect,
        double point,
        Guid questionId,
        string? explanation = null
    )
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty", nameof(content));
        if (point < 0)
            throw new ArgumentException("Point cannot be negative", nameof(point));
        if (questionId == Guid.Empty)
            throw new ArgumentException("Question ID cannot be empty", nameof(questionId));

        return new Answer(content, isCorrect, point, questionId, explanation);
    }

    public void MarkAsCorrect()
    {
        IsCorrect = true;
    }

    public void MarkAsIncorrect()
    {
        IsCorrect = false;
    }

    public void Update(string content, bool isCorrect, double point, string? explanation = null)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty", nameof(content));
        if (point < 0)
            throw new ArgumentException("Point cannot be negative", nameof(point));

        Content = content;
        IsCorrect = isCorrect;
        Point = point;
        Explanation = explanation;
    }
}
