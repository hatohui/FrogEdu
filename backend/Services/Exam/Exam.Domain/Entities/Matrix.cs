using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Exam.Domain.Entities;

/// <summary>
/// Matrix defines a reusable blueprint for exam structure - how many questions per topic/cognitive level.
/// A matrix can be attached to multiple exams as a template.
/// </summary>
public sealed class Matrix : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid SubjectId { get; private set; }
    public int Grade { get; private set; }

    private readonly List<MatrixTopic> _matrixTopics = new();
    public IReadOnlyCollection<MatrixTopic> MatrixTopics => _matrixTopics.AsReadOnly();

    private Matrix() { }

    private Matrix(string name, string? description, Guid subjectId, int grade)
    {
        Name = name;
        Description = description;
        SubjectId = subjectId;
        Grade = grade;
    }

    public static Matrix Create(
        string name,
        string? description,
        Guid subjectId,
        int grade,
        string userId
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (subjectId == Guid.Empty)
            throw new ArgumentException("Subject ID cannot be empty", nameof(subjectId));

        if (grade < 1 || grade > 5)
            throw new ArgumentException("Grade must be between 1 and 5", nameof(grade));

        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be empty", nameof(userId));

        var matrix = new Matrix(name, description, subjectId, grade);
        matrix.MarkAsCreated(Guid.Parse(userId));
        return matrix;
    }

    public void Update(string name, string? description, string userId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
        Description = description;
        MarkAsUpdated();
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

    public void ClearMatrixTopics()
    {
        _matrixTopics.Clear();
    }

    public int GetTotalQuestionCount()
    {
        return _matrixTopics.Sum(mt => mt.Quantity);
    }
}
