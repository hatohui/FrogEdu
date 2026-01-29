using FrogEdu.Shared.Kernel;

namespace FrogEdu.Class.Domain.Entities;

/// <summary>
/// Represents a student's submission for an exam
/// </summary>
public class Submission : Entity
{
    public Guid ExamId { get; private set; }
    public Guid StudentId { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? SubmittedAt { get; private set; }
    public decimal? Score { get; private set; }
    public SubmissionStatus Status { get; private set; }

    private readonly List<Answer> _answers = new();
    public IReadOnlyCollection<Answer> Answers => _answers.AsReadOnly();

    private Submission() { } // EF Core

    public Submission(Guid examId, Guid studentId)
    {
        ExamId = examId;
        StudentId = studentId;
        StartedAt = DateTime.UtcNow;
        Status = SubmissionStatus.InProgress;
    }

    public void Submit()
    {
        if (Status == SubmissionStatus.Submitted)
            throw new InvalidOperationException("Submission already submitted");

        SubmittedAt = DateTime.UtcNow;
        Status = SubmissionStatus.Submitted;
        MarkAsUpdated();
    }

    public void Grade(decimal score)
    {
        if (Status != SubmissionStatus.Submitted)
            throw new InvalidOperationException("Cannot grade submission that is not submitted");

        Score = score;
        Status = SubmissionStatus.Graded;
        MarkAsUpdated();
    }

    public Answer AddAnswer(Guid questionId, Guid[]? selectedOptionIds, string? answerText)
    {
        var answer = new Answer(Id, questionId, selectedOptionIds, answerText);
        _answers.Add(answer);
        MarkAsUpdated();
        return answer;
    }
}

public enum SubmissionStatus
{
    InProgress = 0,
    Submitted = 1,
    Graded = 2,
}
