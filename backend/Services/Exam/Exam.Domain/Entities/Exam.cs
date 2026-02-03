using FrogEdu.Exam.Domain.Events;
using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Exam.Domain.Entities;

public sealed class Exam : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public Guid SubjectId { get; private set; }
    public int Grade { get; private set; }
    public bool IsDraft { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<ExamQuestion> _examQuestions = new();
    public IReadOnlyCollection<ExamQuestion> ExamQuestions => _examQuestions.AsReadOnly();

    private Exam() { }

    private Exam(string name, string description, Guid subjectId, int grade)
    {
        Name = name;
        Description = description;
        SubjectId = subjectId;
        Grade = grade;
        IsDraft = true;
        IsActive = false;
    }

    public static Exam Create(
        string name,
        string description,
        Guid subjectId,
        int grade,
        string userId
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        if (subjectId == Guid.Empty)
            throw new ArgumentException("Subject ID cannot be empty", nameof(subjectId));
        if (grade < 1 || grade > 5)
            throw new ArgumentException(
                "Grade must be between 1 and 5 (primary school)",
                nameof(grade)
            );

        var exam = new Exam(name, description, subjectId, grade);

        // Pass userId to ensure CreatedBy is set
        if (Guid.TryParse(userId, out var createdByGuid))
        {
            exam.MarkAsCreated(createdByGuid);
        }
        else
        {
            exam.MarkAsCreated();
        }

        exam.AddDomainEvent(new ExamCreatedDomainEvent(exam.Id, exam.Name));
        return exam;
    }

    public void Update(string name, string description, string userId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));

        Name = name;
        Description = description;
        MarkAsUpdated();
    }

    public void Publish(string userId)
    {
        if (!IsDraft)
            throw new InvalidOperationException("Exam is already published");
        if (_examQuestions.Count == 0)
            throw new InvalidOperationException("Cannot publish exam without questions");

        IsDraft = false;
        IsActive = true;
        MarkAsUpdated();
        AddDomainEvent(new ExamPublishedDomainEvent(Id, Name));
    }

    public void Archive(string userId)
    {
        if (IsDraft)
            throw new InvalidOperationException("Cannot archive a draft exam");

        IsActive = false;
        MarkAsUpdated();
        AddDomainEvent(new ExamArchivedDomainEvent(Id, Name));
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
}
