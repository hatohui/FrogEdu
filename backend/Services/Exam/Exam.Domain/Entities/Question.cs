using FrogEdu.Exam.Domain.Enums;
using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Exam.Domain.Entities;

public sealed class Question : AuditableEntity
{
    public string Content { get; private set; } = null!;
    public double Point { get; private set; }
    public QuestionType Type { get; private set; }
    public string? MediaUrl { get; private set; }
    public CognitiveLevel CognitiveLevel { get; private set; }
    public bool IsPublic { get; private set; }
    public QuestionSource Source { get; private set; }
    public Guid TopicId { get; private set; }

    private readonly List<Answer> _answers = new();
    public IReadOnlyCollection<Answer> Answers => _answers.AsReadOnly();

    private Question() { }

    private Question(
        string content,
        double point,
        QuestionType type,
        CognitiveLevel cognitiveLevel,
        QuestionSource source,
        Guid topicId,
        string? mediaUrl = null,
        bool isPublic = false
    )
    {
        Content = content;
        Point = point;
        Type = type;
        CognitiveLevel = cognitiveLevel;
        Source = source;
        TopicId = topicId;
        MediaUrl = mediaUrl;
        IsPublic = isPublic;
    }

    public static Question Create(
        string content,
        double point,
        QuestionType type,
        CognitiveLevel cognitiveLevel,
        QuestionSource source,
        Guid topicId,
        string userId,
        string? mediaUrl = null,
        bool isPublic = false
    )
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty", nameof(content));
        if (point <= 0)
            throw new ArgumentException("Point must be greater than 0", nameof(point));
        if (topicId == Guid.Empty)
            throw new ArgumentException("Topic ID cannot be empty", nameof(topicId));

        var question = new Question(
            content,
            point,
            type,
            cognitiveLevel,
            source,
            topicId,
            mediaUrl,
            isPublic
        );
        question.MarkAsCreated();
        return question;
    }

    public void Update(
        string content,
        double point,
        QuestionType type,
        CognitiveLevel cognitiveLevel,
        string? mediaUrl,
        string userId
    )
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty", nameof(content));
        if (point <= 0)
            throw new ArgumentException("Point must be greater than 0", nameof(point));

        Content = content;
        Point = point;
        Type = type;
        CognitiveLevel = cognitiveLevel;
        MediaUrl = mediaUrl;
        MarkAsUpdated();
    }

    public void MakePublic()
    {
        IsPublic = true;
    }

    public void MakePrivate()
    {
        IsPublic = false;
    }

    public void AddAnswer(Answer answer)
    {
        _answers.Add(answer);
    }
}
