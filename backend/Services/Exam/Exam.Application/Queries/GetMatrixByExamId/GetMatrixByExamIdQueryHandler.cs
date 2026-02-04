using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetMatrixByExamId;

public sealed class GetMatrixByExamIdQueryHandler
    : IRequestHandler<GetMatrixByExamIdQuery, MatrixDto?>
{
    private readonly IMatrixRepository _matrixRepository;
    private readonly IExamRepository _examRepository;

    public GetMatrixByExamIdQueryHandler(
        IMatrixRepository matrixRepository,
        IExamRepository examRepository
    )
    {
        _matrixRepository = matrixRepository;
        _examRepository = examRepository;
    }

    public async Task<MatrixDto?> Handle(
        GetMatrixByExamIdQuery request,
        CancellationToken cancellationToken
    )
    {
        // Get the exam and check access
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);
        if (exam is null)
            return null;

        var userId = Guid.Parse(request.UserId);
        if (exam.CreatedBy != userId && !exam.IsActive)
        {
            // Only the creator can view draft exams
            return null;
        }

        // If exam has no matrix attached, return null
        if (exam.MatrixId is null)
            return null;

        // Get the attached matrix
        var matrix = await _matrixRepository.GetByIdAsync(exam.MatrixId.Value, cancellationToken);

        if (matrix is null)
            return null;

        var matrixTopics = matrix
            .MatrixTopics.Select(mt => new MatrixTopicDto(
                mt.TopicId,
                mt.CognitiveLevel,
                mt.Quantity
            ))
            .ToList();

        return new MatrixDto(
            matrix.Id,
            matrix.Name,
            matrix.Description,
            matrix.SubjectId,
            matrix.Grade,
            matrixTopics,
            matrix.GetTotalQuestionCount(),
            matrix.CreatedAt
        );
    }
}
