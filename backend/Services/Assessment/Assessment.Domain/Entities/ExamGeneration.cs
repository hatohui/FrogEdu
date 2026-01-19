using FrogEdu.Shared.Kernel;

namespace FrogEdu.Assessment.Domain.Entities;

/// <summary>
/// Represents an AI-generated exam request and result
/// </summary>
public class ExamGeneration : Entity
{
    public Guid ExamId { get; private set; }
    public Guid GeneratedBy { get; private set; }
    public ExamGenerationStatus Status { get; private set; }
    public string Prompt { get; private set; } = default!;
    public string? ResultUri { get; private set; }
    public string? Error { get; private set; }

    private ExamGeneration() { } // EF Core

    public ExamGeneration(Guid examId, Guid generatedBy, string prompt)
    {
        ExamId = examId;
        GeneratedBy = generatedBy;
        Prompt = prompt;
        Status = ExamGenerationStatus.Pending;
    }

    public void StartProcessing()
    {
        Status = ExamGenerationStatus.Processing;
        MarkAsUpdated();
    }

    public void Complete(string resultUri)
    {
        Status = ExamGenerationStatus.Completed;
        ResultUri = resultUri;
        MarkAsUpdated();
    }

    public void Fail(string error)
    {
        Status = ExamGenerationStatus.Failed;
        Error = error;
        MarkAsUpdated();
    }
}

public enum ExamGenerationStatus
{
    Pending = 0,
    Processing = 1,
    Completed = 2,
    Failed = 3,
}
