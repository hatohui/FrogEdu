using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.AttachMatrixToExam;

public sealed class AttachMatrixToExamCommandHandler : IRequestHandler<AttachMatrixToExamCommand>
{
    private readonly IExamRepository _examRepository;
    private readonly IMatrixRepository _matrixRepository;

    public AttachMatrixToExamCommandHandler(
        IExamRepository examRepository,
        IMatrixRepository matrixRepository
    )
    {
        _examRepository = examRepository;
        _matrixRepository = matrixRepository;
    }

    public async Task Handle(AttachMatrixToExamCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(request.UserId);

        // Get exam and verify ownership
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);
        if (exam is null)
            throw new InvalidOperationException($"Exam with ID {request.ExamId} not found");

        if (exam.CreatedBy != userId)
            throw new UnauthorizedAccessException("You are not authorized to modify this exam");

        // Verify matrix exists
        var matrix = await _matrixRepository.GetByIdAsync(request.MatrixId, cancellationToken);
        if (matrix is null)
            throw new InvalidOperationException($"Matrix with ID {request.MatrixId} not found");

        // Attach the matrix to the exam
        exam.AttachMatrix(request.MatrixId);

        _examRepository.Update(exam);
        await _examRepository.SaveChangesAsync(cancellationToken);
    }
}
