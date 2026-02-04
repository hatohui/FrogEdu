using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetMatrices;

public sealed class GetMatricesQueryHandler : IRequestHandler<GetMatricesQuery, GetMatricesResponse>
{
    private readonly IMatrixRepository _matrixRepository;

    public GetMatricesQueryHandler(IMatrixRepository matrixRepository)
    {
        _matrixRepository = matrixRepository;
    }

    public async Task<GetMatricesResponse> Handle(
        GetMatricesQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = Guid.Parse(request.UserId);
        var matrices = await _matrixRepository.GetByCreatorAsync(userId, cancellationToken);

        var matrixDtos = matrices
            .Select(m => new MatrixDto(
                m.Id,
                m.Name,
                m.Description,
                m.SubjectId,
                null, // SubjectName not needed for list view
                m.Grade,
                m.MatrixTopics.Select(mt => new MatrixTopicDto(
                        mt.TopicId,
                        null, // TopicTitle not needed for list view
                        mt.CognitiveLevel,
                        mt.Quantity
                    ))
                    .ToList(),
                m.MatrixTopics.Sum(mt => mt.Quantity),
                m.CreatedAt
            ))
            .ToList();

        return new GetMatricesResponse(matrixDtos);
    }
}
