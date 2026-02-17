using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Class.Domain.Entities;

public sealed class ExamSession : AuditableEntity
{
    public Guid ClassId { get; private set; }
    public Guid ExamId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public int RetryTimes { get; private set; }
    public bool IsRetryable { get; private set; }
    public bool IsActive { get; private set; }
    public bool ShouldShuffleQuestions { get; private set; }
    public bool ShouldShuffleAnswers { get; private set; }
    public bool AllowPartialScoring { get; private set; }

    private readonly List<StudentExamAttempt> _attempts = [];
    public IReadOnlyCollection<StudentExamAttempt> Attempts => _attempts.AsReadOnly();

    private ExamSession() { }

    private ExamSession(
        Guid classId,
        Guid examId,
        DateTime startTime,
        DateTime endTime,
        int retryTimes,
        bool isRetryable,
        bool shouldShuffleQuestions,
        bool shouldShuffleAnswers,
        bool allowPartialScoring
    )
    {
        ClassId = classId;
        ExamId = examId;
        StartTime = startTime;
        EndTime = endTime;
        RetryTimes = retryTimes;
        IsRetryable = isRetryable;
        IsActive = true;
        ShouldShuffleQuestions = shouldShuffleQuestions;
        ShouldShuffleAnswers = shouldShuffleAnswers;
        AllowPartialScoring = allowPartialScoring;
    }

    public static ExamSession Create(
        Guid classId,
        Guid examId,
        DateTime startTime,
        DateTime endTime,
        int retryTimes,
        bool isRetryable,
        bool shouldShuffleQuestions,
        bool shouldShuffleAnswers,
        bool allowPartialScoring,
        Guid createdBy
    )
    {
        if (classId == Guid.Empty)
            throw new ArgumentException("Class ID cannot be empty", nameof(classId));
        if (examId == Guid.Empty)
            throw new ArgumentException("Exam ID cannot be empty", nameof(examId));
        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time", nameof(startTime));
        if (retryTimes < 0)
            throw new ArgumentException("Retry times cannot be negative", nameof(retryTimes));
        if (isRetryable && retryTimes == 0)
            throw new ArgumentException(
                "Retry times must be greater than 0 when retryable",
                nameof(retryTimes)
            );

        var session = new ExamSession(
            classId,
            examId,
            startTime,
            endTime,
            retryTimes,
            isRetryable,
            shouldShuffleQuestions,
            shouldShuffleAnswers,
            allowPartialScoring
        );
        session.MarkAsCreated(createdBy);
        return session;
    }

    public void Update(
        DateTime startTime,
        DateTime endTime,
        int retryTimes,
        bool isRetryable,
        bool shouldShuffleQuestions,
        bool shouldShuffleAnswers,
        bool allowPartialScoring,
        Guid updatedBy
    )
    {
        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time", nameof(startTime));
        if (retryTimes < 0)
            throw new ArgumentException("Retry times cannot be negative", nameof(retryTimes));
        if (isRetryable && retryTimes == 0)
            throw new ArgumentException(
                "Retry times must be greater than 0 when retryable",
                nameof(retryTimes)
            );

        StartTime = startTime;
        EndTime = endTime;
        RetryTimes = retryTimes;
        IsRetryable = isRetryable;
        ShouldShuffleQuestions = shouldShuffleQuestions;
        ShouldShuffleAnswers = shouldShuffleAnswers;
        AllowPartialScoring = allowPartialScoring;
        MarkAsUpdated(updatedBy);
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public bool IsCurrentlyActive()
    {
        var now = DateTime.UtcNow;
        return IsActive && now >= StartTime && now <= EndTime;
    }

    public bool IsUpcoming()
    {
        return IsActive && DateTime.UtcNow < StartTime;
    }

    public bool HasEnded()
    {
        return DateTime.UtcNow > EndTime;
    }

    public bool CanStudentAttempt(Guid studentId)
    {
        if (!IsCurrentlyActive())
            return false;

        var studentAttempts = _attempts.Count(a => a.StudentId == studentId);

        if (!IsRetryable)
            return studentAttempts == 0;

        return studentAttempts < RetryTimes;
    }

    public int GetStudentAttemptCount(Guid studentId)
    {
        return _attempts.Count(a => a.StudentId == studentId);
    }

    public void AddAttempt(StudentExamAttempt attempt)
    {
        _attempts.Add(attempt);
    }
}
