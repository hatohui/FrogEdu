using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.DetachMatrixFromExam;

public sealed class DetachMatrixFromExamCommandHandler
    : IRequestHandler<DetachMatrixFromExamCommand>
{
    private readonly IExamRepository _examRepository;

    public DetachMatrixFromExamCommandHandler(IExamRepository examRepository)
    {
        _examRepository = examRepository;
    }

    public async Task Handle(
        DetachMatrixFromExamCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = Guid.Parse(request.UserId);

        // Get exam and verify ownership
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);
        if (exam is null)
            throw new InvalidOperationException($"Exam with ID {request.ExamId} not found");

        if (exam.CreatedBy != userId)
            throw new UnauthorizedAccessException("You are not authorized to modify this exam");

        // Detach the matrix from the exam
        exam.DetachMatrix();

        _examRepository.Update(exam);
        await _examRepository.SaveChangesAsync(cancellationToken);
    }
}
