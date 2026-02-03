using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Exam.Domain.Entities;

/// <summary>
/// Matrix defines the blueprint for exam structure - how many questions per topic/cognitive level
/// </summary>
public sealed class Matrix : AuditableEntity
{
    public Guid ExamId { get; private set; }

    private readonly List<MatrixTopic> _matrixTopics = new();
    public IReadOnlyCollection<MatrixTopic> MatrixTopics => _matrixTopics.AsReadOnly();

    private Matrix() { }

    private Matrix(Guid examId)
    {
        ExamId = examId;
    }

    public static Matrix Create(Guid examId, string userId)
    {
        if (examId == Guid.Empty)
            throw new ArgumentException("Exam ID cannot be empty", nameof(examId));

        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be empty", nameof(userId));

        var matrix = new Matrix(examId);
        matrix.MarkAsCreated(Guid.Parse(userId));
        return matrix;
    }

    public void AddMatrixTopic(MatrixTopic matrixTopic)
    {
        _matrixTopics.Add(matrixTopic);
    }

    public void RemoveMatrixTopic(Guid topicId, Enums.CognitiveLevel cognitiveLevel)
    {
        var matrixTopic = _matrixTopics.FirstOrDefault(mt =>
            mt.TopicId == topicId && mt.CognitiveLevel == cognitiveLevel
        );

        if (matrixTopic is not null)
            _matrixTopics.Remove(matrixTopic);
    }

    public int GetTotalQuestionCount()
    {
        return _matrixTopics.Sum(mt => mt.Quantity);
    }
}
