using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetMatrixById;

public sealed class GetMatrixByIdQueryHandler : IRequestHandler<GetMatrixByIdQuery, MatrixDto?>
{
    private readonly IMatrixRepository _matrixRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly ITopicRepository _topicRepository;

    public GetMatrixByIdQueryHandler(
        IMatrixRepository matrixRepository,
        ISubjectRepository subjectRepository,
        ITopicRepository topicRepository
    )
    {
        _matrixRepository = matrixRepository;
        _subjectRepository = subjectRepository;
        _topicRepository = topicRepository;
    }

    public async Task<MatrixDto?> Handle(
        GetMatrixByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var matrix = await _matrixRepository.GetByIdAsync(request.MatrixId, cancellationToken);

        if (matrix is null)
            return null;

        // Verify user has access (owns the matrix)
        var userId = Guid.Parse(request.UserId);
        if (matrix.CreatedBy != userId)
            return null;

        // Fetch subject name
        var subject = await _subjectRepository.GetByIdAsync(matrix.SubjectId, cancellationToken);

        // Fetch all topics for this matrix
        var topicIds = matrix.MatrixTopics.Select(mt => mt.TopicId).Distinct().ToList();
        var topics = new Dictionary<Guid, string>();
        foreach (var topicId in topicIds)
        {
            var topic = await _topicRepository.GetByIdAsync(topicId, cancellationToken);
            if (topic != null)
            {
                topics[topicId] = topic.Title;
            }
        }

        return new MatrixDto(
            matrix.Id,
            matrix.Name,
            matrix.Description,
            matrix.SubjectId,
            subject?.Name,
            matrix.Grade,
            matrix
                .MatrixTopics.Select(mt => new MatrixTopicDto(
                    mt.TopicId,
                    topics.GetValueOrDefault(mt.TopicId),
                    mt.CognitiveLevel,
                    mt.Quantity
                ))
                .ToList(),
            matrix.GetTotalQuestionCount(),
            matrix.CreatedAt
        );
    }
}
