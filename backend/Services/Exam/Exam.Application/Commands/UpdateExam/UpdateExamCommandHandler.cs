using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.UpdateExam;

public sealed class UpdateExamCommandHandler : IRequestHandler<UpdateExamCommand, Unit>
{
    private readonly IExamRepository _examRepository;

    public UpdateExamCommandHandler(IExamRepository examRepository)
    {
        _examRepository = examRepository;
    }

    public async Task<Unit> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
    {
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);
        if (exam is null)
            throw new InvalidOperationException($"Exam with ID {request.ExamId} not found");

        exam.Update(request.Name, request.Description, request.UserId);

        _examRepository.Update(exam);
        await _examRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
