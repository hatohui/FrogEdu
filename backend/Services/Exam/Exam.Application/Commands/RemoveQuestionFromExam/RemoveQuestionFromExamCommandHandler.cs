using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.RemoveQuestionFromExam;

public sealed class RemoveQuestionFromExamCommandHandler
    : IRequestHandler<RemoveQuestionFromExamCommand>
{
    private readonly IExamRepository _examRepository;

    public RemoveQuestionFromExamCommandHandler(IExamRepository examRepository)
    {
        _examRepository = examRepository;
    }

    public async Task Handle(
        RemoveQuestionFromExamCommand request,
        CancellationToken cancellationToken
    )
    {
        var exam = await _examRepository.GetByIdAsync(request.ExamId, cancellationToken);

        if (exam is null)
            throw new InvalidOperationException($"Exam with ID {request.ExamId} not found");

        // Verify user has permission (must be creator)
        var userId = Guid.Parse(request.UserId);
        if (exam.CreatedBy != userId)
        {
            throw new UnauthorizedAccessException("You do not have permission to modify this exam");
        }

        // Verify exam is still a draft
        if (!exam.IsDraft)
        {
            throw new InvalidOperationException(
                "Cannot remove questions from a published exam. Please archive and create a new version."
            );
        }

        exam.RemoveQuestion(request.QuestionId);

        _examRepository.Update(exam);
        await _examRepository.SaveChangesAsync(cancellationToken);
    }
}
