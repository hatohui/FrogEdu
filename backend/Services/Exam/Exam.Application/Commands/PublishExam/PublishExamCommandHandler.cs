using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.PublishExam;

public sealed class PublishExamCommandHandler : IRequestHandler<PublishExamCommand, Unit>
{
    private readonly IExamRepository _examRepository;

    public PublishExamCommandHandler(IExamRepository examRepository)
    {
        _examRepository = examRepository;
    }

    public async Task<Unit> Handle(PublishExamCommand request, CancellationToken cancellationToken)
    {
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);
        if (exam is null)
            throw new InvalidOperationException($"Exam with ID {request.ExamId} not found");

        var userId = Guid.Parse(request.UserId);
        if (!request.IsAdmin && exam.CreatedBy.HasValue && exam.CreatedBy.Value != userId)
            throw new UnauthorizedAccessException("Only the exam creator can publish this exam");

        exam.Publish(request.UserId);

        _examRepository.Update(exam);
        await _examRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
