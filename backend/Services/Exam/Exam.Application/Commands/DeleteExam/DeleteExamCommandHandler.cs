using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.DeleteExam;

public sealed class DeleteExamCommandHandler : IRequestHandler<DeleteExamCommand, Unit>
{
    private readonly IExamRepository _examRepository;

    public DeleteExamCommandHandler(IExamRepository examRepository)
    {
        _examRepository = examRepository;
    }

    public async Task<Unit> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
    {
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);
        if (exam is null)
            throw new InvalidOperationException($"Exam with ID {request.ExamId} not found");

        // Verify the user owns this exam (CreatedBy should match UserId)
        if (exam.CreatedBy != Guid.Parse(request.UserId))
            throw new UnauthorizedAccessException("You are not authorized to delete this exam");

        _examRepository.Delete(exam);
        await _examRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
