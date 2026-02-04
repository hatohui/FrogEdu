using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Entities;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.CreateMatrix;

public sealed class CreateMatrixCommandHandler
    : IRequestHandler<CreateMatrixCommand, CreateMatrixResponse>
{
    private readonly IMatrixRepository _matrixRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly ITopicRepository _topicRepository;

    public CreateMatrixCommandHandler(
        IMatrixRepository matrixRepository,
        ISubjectRepository subjectRepository,
        ITopicRepository topicRepository
    )
    {
        _matrixRepository = matrixRepository;
        _subjectRepository = subjectRepository;
        _topicRepository = topicRepository;
    }

    public async Task<CreateMatrixResponse> Handle(
        CreateMatrixCommand request,
        CancellationToken cancellationToken
    )
    {
        // Verify subject exists
        var subject = await _subjectRepository.GetByIdAsync(request.SubjectId, cancellationToken);
        if (subject is null)
            throw new InvalidOperationException($"Subject with ID {request.SubjectId} not found");

        // Verify all topics exist and belong to the subject
        foreach (var matrixTopic in request.MatrixTopics)
        {
            var topic = await _topicRepository.GetByIdAsync(matrixTopic.TopicId, cancellationToken);
            if (topic is null)
                throw new InvalidOperationException(
                    $"Topic with ID {matrixTopic.TopicId} not found"
                );
        }

        var matrix = Matrix.Create(
            request.Name,
            request.Description,
            request.SubjectId,
            request.Grade,
            request.UserId
        );

        foreach (var matrixTopicDto in request.MatrixTopics)
        {
            var matrixTopic = MatrixTopic.Create(
                matrix.Id,
                matrixTopicDto.TopicId,
                matrixTopicDto.CognitiveLevel,
                matrixTopicDto.Quantity
            );
            matrix.AddMatrixTopic(matrixTopic);
        }

        await _matrixRepository.AddAsync(matrix, cancellationToken);
        await _matrixRepository.SaveChangesAsync(cancellationToken);

        return new CreateMatrixResponse(
            matrix.Id,
            matrix.Name,
            matrix.SubjectId,
            matrix.Grade,
            matrix.GetTotalQuestionCount(),
            matrix.CreatedAt
        );
    }
}
