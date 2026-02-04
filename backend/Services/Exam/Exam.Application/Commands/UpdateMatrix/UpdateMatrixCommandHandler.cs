using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.UpdateMatrix;

public sealed class UpdateMatrixCommandHandler : IRequestHandler<UpdateMatrixCommand, Unit>
{
    private readonly IMatrixRepository _matrixRepository;

    public UpdateMatrixCommandHandler(IMatrixRepository matrixRepository)
    {
        _matrixRepository = matrixRepository;
    }

    public async Task<Unit> Handle(UpdateMatrixCommand request, CancellationToken cancellationToken)
    {
        var matrix = await _matrixRepository.GetByIdAsync(request.MatrixId, cancellationToken);
        if (matrix is null)
            throw new InvalidOperationException($"Matrix with ID {request.MatrixId} not found");

        if (matrix.CreatedBy != Guid.Parse(request.UserId))
            throw new UnauthorizedAccessException("You are not authorized to update this matrix");

        // Update matrix properties if provided
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            matrix.Update(request.Name, request.Description, request.UserId);
        }

        // Clear and rebuild matrix topics
        matrix.ClearMatrixTopics();

        foreach (var matrixTopicDto in request.MatrixTopics)
        {
            var matrixTopic = Domain.Entities.MatrixTopic.Create(
                matrix.Id,
                matrixTopicDto.TopicId,
                matrixTopicDto.CognitiveLevel,
                matrixTopicDto.Quantity
            );
            matrix.AddMatrixTopic(matrixTopic);
        }

        _matrixRepository.Update(matrix);
        await _matrixRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
