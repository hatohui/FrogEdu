using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetMatrixById;

public sealed class GetMatrixByIdQueryHandler : IRequestHandler<GetMatrixByIdQuery, MatrixDto?>
{
    private readonly IMatrixRepository _matrixRepository;

    public GetMatrixByIdQueryHandler(IMatrixRepository matrixRepository)
    {
        _matrixRepository = matrixRepository;
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

        return new MatrixDto(
            matrix.Id,
            matrix.ExamId,
            matrix
                .MatrixTopics.Select(mt => new MatrixTopicDto(
                    mt.TopicId,
                    mt.CognitiveLevel,
                    mt.Quantity
                ))
                .ToList(),
            matrix.GetTotalQuestionCount(),
            matrix.CreatedAt
        );
    }
}
