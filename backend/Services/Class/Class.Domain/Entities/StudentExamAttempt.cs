using FrogEdu.Class.Domain.Enums;
using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Class.Domain.Entities;

public sealed class StudentExamAttempt : Entity
{
    public Guid ExamSessionId { get; private set; }
    public Guid StudentId { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? SubmittedAt { get; private set; }
    public double Score { get; private set; }
    public double TotalPoints { get; private set; }
    public int AttemptNumber { get; private set; }
    public AttemptStatus Status { get; private set; }

    private readonly List<StudentAnswer> _answers = [];
    public IReadOnlyCollection<StudentAnswer> Answers => _answers.AsReadOnly();

    private StudentExamAttempt() { }

    private StudentExamAttempt(Guid examSessionId, Guid studentId, int attemptNumber)
    {
        ExamSessionId = examSessionId;
        StudentId = studentId;
        StartedAt = DateTime.UtcNow;
        AttemptNumber = attemptNumber;
        Status = AttemptStatus.InProgress;
        Score = 0;
        TotalPoints = 0;
    }

    public static StudentExamAttempt Create(Guid examSessionId, Guid studentId, int attemptNumber)
    {
        if (examSessionId == Guid.Empty)
            throw new ArgumentException("Exam session ID cannot be empty", nameof(examSessionId));
        if (studentId == Guid.Empty)
            throw new ArgumentException("Student ID cannot be empty", nameof(studentId));
        if (attemptNumber <= 0)
            throw new ArgumentException("Attempt number must be positive", nameof(attemptNumber));

        return new StudentExamAttempt(examSessionId, studentId, attemptNumber);
    }

    public void Submit(double score, double totalPoints)
    {
        if (Status != AttemptStatus.InProgress)
            throw new InvalidOperationException("Can only submit an in-progress attempt");

        Score = score;
        TotalPoints = totalPoints;
        SubmittedAt = DateTime.UtcNow;
        Status = AttemptStatus.Submitted;
    }

    public void MarkAsGraded(double score)
    {
        Score = score;
        Status = AttemptStatus.Graded;
    }

    public void MarkAsTimedOut()
    {
        if (Status != AttemptStatus.InProgress)
            throw new InvalidOperationException("Can only time out an in-progress attempt");

        SubmittedAt = DateTime.UtcNow;
        Status = AttemptStatus.TimedOut;
    }

    public void AddAnswer(StudentAnswer answer)
    {
        _answers.Add(answer);
    }

    public double GetScorePercentage()
    {
        if (TotalPoints == 0)
            return 0;
        return Math.Round((Score / TotalPoints) * 100, 2);
    }
}
