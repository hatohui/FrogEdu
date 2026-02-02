using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Exam.Domain.Entities;

public sealed class Topic : AuditableEntity
{
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public bool IsCurriculum { get; private set; }
    public Guid SubjectId { get; private set; }

    private readonly List<Question> _questions = new();
    public IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();

    private Topic() { }

    private Topic(string title, string description, bool isCurriculum, Guid subjectId)
    {
        Title = title;
        Description = description;
        IsCurriculum = isCurriculum;
        SubjectId = subjectId;
    }

    public static Topic Create(
        string title,
        string description,
        bool isCurriculum,
        Guid subjectId,
        string userId
    )
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
        if (subjectId == Guid.Empty)
            throw new ArgumentException("Subject ID cannot be empty", nameof(subjectId));

        var topic = new Topic(title, description, isCurriculum, subjectId);
        topic.MarkAsCreated();
        return topic;
    }

    public void Update(string title, string description, string userId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        Title = title;
        Description = description;
        MarkAsUpdated();
    }

    public void MarkAsCurriculum()
    {
        IsCurriculum = true;
    }
}
