using FrogEdu.Shared.Kernel;

namespace FrogEdu.Exam.Domain.Entities;

/// <summary>
/// Exam aggregate root - represents a generated exam
/// </summary>
public sealed class Exam : Entity
{
    public Guid TeacherId { get; private set; }
    public Guid? ClassId { get; private set; }
    public string MatrixConfig { get; private set; } = null!;
    public string? PdfUrl { get; private set; }
    public ExamStatus Status { get; private set; }

    private readonly List<ExamQuestion> _examQuestions = new();
    public IReadOnlyCollection<ExamQuestion> ExamQuestions => _examQuestions.AsReadOnly();

    private Exam() { }

    private Exam(Guid teacherId, Guid? classId, string matrixConfig)
    {
        TeacherId = teacherId;
        ClassId = classId;
        MatrixConfig = matrixConfig;
        Status = ExamStatus.Draft;
    }

    public static Exam Create(Guid teacherId, Guid? classId, string matrixConfig)
    {
        return new Exam(teacherId, classId, matrixConfig);
    }

    public void AddQuestion(Guid questionId, int orderIndex)
    {
        var examQuestion = ExamQuestion.Create(Id, questionId, orderIndex);
        _examQuestions.Add(examQuestion);
    }

    public void RemoveQuestion(Guid questionId)
    {
        var examQuestion = _examQuestions.FirstOrDefault(eq => eq.QuestionId == questionId);
        if (examQuestion != null)
            _examQuestions.Remove(examQuestion);
    }

    public void SetPdfUrl(string pdfUrl)
    {
        PdfUrl = pdfUrl;
        Status = ExamStatus.Finalized;
        UpdateTimestamp();
    }

    public void Publish()
    {
        Status = ExamStatus.Published;
        UpdateTimestamp();
    }
}

public enum ExamStatus
{
    Draft = 1,
    Finalized = 2,
    Published = 3,
    Archived = 4,
}
