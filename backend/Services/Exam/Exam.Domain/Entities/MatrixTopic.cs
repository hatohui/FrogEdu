using FrogEdu.Exam.Domain.Enums;

namespace FrogEdu.Exam.Domain.Entities;

/// <summary>
/// Defines how many questions of a specific cognitive level should be included from a topic
/// </summary>
public sealed class MatrixTopic
{
    public Guid MatrixId { get; private set; }
    public Guid TopicId { get; private set; }
    public CognitiveLevel CognitiveLevel { get; private set; }
    public int Quantity { get; private set; }

    private MatrixTopic() { }

    private MatrixTopic(Guid matrixId, Guid topicId, CognitiveLevel cognitiveLevel, int quantity)
    {
        MatrixId = matrixId;
        TopicId = topicId;
        CognitiveLevel = cognitiveLevel;
        Quantity = quantity;
    }

    public static MatrixTopic Create(
        Guid matrixId,
        Guid topicId,
        CognitiveLevel cognitiveLevel,
        int quantity
    )
    {
        if (matrixId == Guid.Empty)
            throw new ArgumentException("Matrix ID cannot be empty", nameof(matrixId));
        if (topicId == Guid.Empty)
            throw new ArgumentException("Topic ID cannot be empty", nameof(topicId));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

        return new MatrixTopic(matrixId, topicId, cognitiveLevel, quantity);
    }

    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

        Quantity = quantity;
    }
}
