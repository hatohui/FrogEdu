using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Class.Domain.Entities;

public sealed class StudentAnswer : Entity
{
    public Guid AttemptId { get; private set; }
    public Guid QuestionId { get; private set; }

    /// <summary>
    /// Comma-separated list of selected answer IDs (for MC/MA/TF).
    /// For FillInTheBlank, the student's typed answer text.
    /// </summary>
    public string SelectedAnswerIds { get; private set; } = null!;

    public double Score { get; private set; }
    public bool IsCorrect { get; private set; }
    public bool IsPartiallyCorrect { get; private set; }

    private StudentAnswer() { }

    private StudentAnswer(Guid attemptId, Guid questionId, string selectedAnswerIds)
    {
        AttemptId = attemptId;
        QuestionId = questionId;
        SelectedAnswerIds = selectedAnswerIds;
        Score = 0;
        IsCorrect = false;
        IsPartiallyCorrect = false;
    }

    public static StudentAnswer Create(Guid attemptId, Guid questionId, string selectedAnswerIds)
    {
        if (attemptId == Guid.Empty)
            throw new ArgumentException("Attempt ID cannot be empty", nameof(attemptId));
        if (questionId == Guid.Empty)
            throw new ArgumentException("Question ID cannot be empty", nameof(questionId));

        return new StudentAnswer(attemptId, questionId, selectedAnswerIds);
    }

    public void Grade(double score, bool isCorrect, bool isPartiallyCorrect)
    {
        if (score < 0)
            throw new ArgumentException("Score cannot be negative", nameof(score));

        Score = score;
        IsCorrect = isCorrect;
        IsPartiallyCorrect = isPartiallyCorrect;
    }
}
