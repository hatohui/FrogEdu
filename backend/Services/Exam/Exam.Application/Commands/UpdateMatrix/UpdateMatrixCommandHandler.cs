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

        _matrixRepository.Delete(matrix);

        var updatedMatrix = Domain.Entities.Matrix.Create(matrix.ExamId, request.UserId);
        foreach (var matrixTopicDto in request.MatrixTopics)
        {
            var matrixTopic = Domain.Entities.MatrixTopic.Create(
                updatedMatrix.Id,
                matrixTopicDto.TopicId,
                matrixTopicDto.CognitiveLevel,
                matrixTopicDto.Quantity
            );
            updatedMatrix.AddMatrixTopic(matrixTopic);
        }

        await _matrixRepository.AddAsync(updatedMatrix, cancellationToken);
        await _matrixRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
