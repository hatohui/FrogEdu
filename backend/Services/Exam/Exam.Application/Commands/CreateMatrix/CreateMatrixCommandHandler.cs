using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Entities;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.CreateMatrix;

public sealed class CreateMatrixCommandHandler
    : IRequestHandler<CreateMatrixCommand, CreateMatrixResponse>
{
    private readonly IMatrixRepository _matrixRepository;
    private readonly IExamRepository _examRepository;
    private readonly ITopicRepository _topicRepository;

    public CreateMatrixCommandHandler(
        IMatrixRepository matrixRepository,
        IExamRepository examRepository,
        ITopicRepository topicRepository
    )
    {
        _matrixRepository = matrixRepository;
        _examRepository = examRepository;
        _topicRepository = topicRepository;
    }

    public async Task<CreateMatrixResponse> Handle(
        CreateMatrixCommand request,
        CancellationToken cancellationToken
    )
    {
        // Verify exam exists
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);
        if (exam is null)
            throw new InvalidOperationException($"Exam with ID {request.ExamId} not found");

        // Verify all topics exist
        foreach (var matrixTopic in request.MatrixTopics)
        {
            var topic = await _topicRepository.GetByIdAsync(matrixTopic.TopicId, cancellationToken);
            if (topic is null)
                throw new InvalidOperationException(
                    $"Topic with ID {matrixTopic.TopicId} not found"
                );
        }

        var matrix = Matrix.Create(request.ExamId, request.UserId);

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
            matrix.ExamId,
            matrix.GetTotalQuestionCount(),
            matrix.CreatedAt
        );
    }
}
