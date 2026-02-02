using FrogEdu.Exam.Domain.Events;
using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Exam.Domain.Entities;

public sealed class Exam : AuditableEntity
{
    public string Title { get; private set; } = null!;
    public int Duration { get; private set; } // in minutes
    public string? AccessCode { get; private set; }
    public int PassScore { get; private set; }
    public int MaxAttempts { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public bool ShouldShuffleQuestions { get; private set; }
    public bool ShouldShuffleAnswerOptions { get; private set; }
    public bool IsDraft { get; private set; }
    public bool IsActive { get; private set; }
    public Guid TopicId { get; private set; }

    private readonly List<ExamQuestion> _examQuestions = new();
    public IReadOnlyCollection<ExamQuestion> ExamQuestions => _examQuestions.AsReadOnly();

    private Exam() { }

    private Exam(
        string title,
        int duration,
        int passScore,
        int maxAttempts,
        DateTime startTime,
        DateTime endTime,
        Guid topicId,
        bool shouldShuffleQuestions = false,
        bool shouldShuffleAnswerOptions = false
    )
    {
        Title = title;
        Duration = duration;
        PassScore = passScore;
        MaxAttempts = maxAttempts;
        StartTime = startTime;
        EndTime = endTime;
        TopicId = topicId;
        ShouldShuffleQuestions = shouldShuffleQuestions;
        ShouldShuffleAnswerOptions = shouldShuffleAnswerOptions;
        IsDraft = true;
        IsActive = false;
    }

    public static Exam Create(
        string title,
        int duration,
        int passScore,
        int maxAttempts,
        DateTime startTime,
        DateTime endTime,
        Guid topicId,
        string userId,
        bool shouldShuffleQuestions = false,
        bool shouldShuffleAnswerOptions = false
    )
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
        if (duration <= 0)
            throw new ArgumentException("Duration must be greater than 0", nameof(duration));
        if (passScore < 0 || passScore > 100)
            throw new ArgumentException("Pass score must be between 0 and 100", nameof(passScore));
        if (maxAttempts <= 0)
            throw new ArgumentException("Max attempts must be greater than 0", nameof(maxAttempts));
        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time", nameof(startTime));
        if (topicId == Guid.Empty)
            throw new ArgumentException("Topic ID cannot be empty", nameof(topicId));

        var exam = new Exam(
            title,
            duration,
            passScore,
            maxAttempts,
            startTime,
            endTime,
            topicId,
            shouldShuffleQuestions,
            shouldShuffleAnswerOptions
        );

        // Pass userId to ensure CreatedBy is set
        if (Guid.TryParse(userId, out var createdByGuid))
        {
            exam.MarkAsCreated(createdByGuid);
        }
        else
        {
            exam.MarkAsCreated();
        }

        exam.AddDomainEvent(new ExamCreatedDomainEvent(exam.Id, exam.Title));
        return exam;
    }

    public void Update(
        string title,
        int duration,
        int passScore,
        int maxAttempts,
        DateTime startTime,
        DateTime endTime,
        string userId
    )
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
        if (duration <= 0)
            throw new ArgumentException("Duration must be greater than 0", nameof(duration));
        if (passScore < 0 || passScore > 100)
            throw new ArgumentException("Pass score must be between 0 and 100", nameof(passScore));
        if (maxAttempts <= 0)
            throw new ArgumentException("Max attempts must be greater than 0", nameof(maxAttempts));
        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time", nameof(startTime));

        Title = title;
        Duration = duration;
        PassScore = passScore;
        MaxAttempts = maxAttempts;
        StartTime = startTime;
        EndTime = endTime;
        MarkAsUpdated();
    }

    public void Publish(string accessCode, string userId)
    {
        if (!IsDraft)
            throw new InvalidOperationException("Exam is already published");
        if (_examQuestions.Count == 0)
            throw new InvalidOperationException("Cannot publish exam without questions");

        IsDraft = false;
        IsActive = true;
        AccessCode = accessCode;
        MarkAsUpdated();
        AddDomainEvent(new ExamPublishedDomainEvent(Id, Title, accessCode));
    }

    public void Archive(string userId)
    {
        if (IsDraft)
            throw new InvalidOperationException("Cannot archive a draft exam");

        IsActive = false;
        MarkAsUpdated();
        AddDomainEvent(new ExamArchivedDomainEvent(Id, Title));
    }

    public void AddQuestion(Guid questionId)
    {
        if (_examQuestions.Any(eq => eq.QuestionId == questionId))
            throw new InvalidOperationException("Question already added to exam");

        _examQuestions.Add(ExamQuestion.Create(Id, questionId));
    }

    public void RemoveQuestion(Guid questionId)
    {
        var examQuestion = _examQuestions.FirstOrDefault(eq => eq.QuestionId == questionId);
        if (examQuestion is null)
            throw new InvalidOperationException("Question not found in exam");

        _examQuestions.Remove(examQuestion);
    }

    public void SetShuffleSettings(bool shuffleQuestions, bool shuffleAnswers, string userId)
    {
        ShouldShuffleQuestions = shuffleQuestions;
        ShouldShuffleAnswerOptions = shuffleAnswers;
        MarkAsUpdated();
    }
}
