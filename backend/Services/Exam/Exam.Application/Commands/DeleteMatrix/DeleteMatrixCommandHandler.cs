using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.DeleteMatrix;

public sealed class DeleteMatrixCommandHandler : IRequestHandler<DeleteMatrixCommand, Unit>
{
    private readonly IMatrixRepository _matrixRepository;

    public DeleteMatrixCommandHandler(IMatrixRepository matrixRepository)
    {
        _matrixRepository = matrixRepository;
    }

    public async Task<Unit> Handle(DeleteMatrixCommand request, CancellationToken cancellationToken)
    {
        var matrix = await _matrixRepository.GetByIdAsync(request.MatrixId, cancellationToken);
        if (matrix is null)
            throw new InvalidOperationException($"Matrix with ID {request.MatrixId} not found");

        // Verify the user owns this matrix
        if (matrix.CreatedBy != Guid.Parse(request.UserId))
            throw new UnauthorizedAccessException("You are not authorized to delete this matrix");

        _matrixRepository.Delete(matrix);
        await _matrixRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
