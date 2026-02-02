using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Exam.Domain.Entities;

public sealed class Answer : Entity
{
    public string Content { get; private set; } = null!;
    public bool IsCorrect { get; private set; }
    public string? Explanation { get; private set; }
    public Guid QuestionId { get; private set; }

    private Answer() { }

    private Answer(string content, bool isCorrect, Guid questionId, string? explanation = null)
    {
        Content = content;
        IsCorrect = isCorrect;
        QuestionId = questionId;
        Explanation = explanation;
    }

    public static Answer Create(
        string content,
        bool isCorrect,
        Guid questionId,
        string? explanation = null
    )
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty", nameof(content));
        if (questionId == Guid.Empty)
            throw new ArgumentException("Question ID cannot be empty", nameof(questionId));

        return new Answer(content, isCorrect, questionId, explanation);
    }

    public void MarkAsCorrect()
    {
        IsCorrect = true;
    }

    public void MarkAsIncorrect()
    {
        IsCorrect = false;
    }

    public void Update(string content, bool isCorrect, string? explanation = null)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty", nameof(content));

        Content = content;
        IsCorrect = isCorrect;
        Explanation = explanation;
    }
}
