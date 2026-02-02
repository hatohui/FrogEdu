using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.DeleteQuestion;

public sealed class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Unit>
{
    private readonly IQuestionRepository _questionRepository;

    public DeleteQuestionCommandHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<Unit> Handle(
        DeleteQuestionCommand request,
        CancellationToken cancellationToken
    )
    {
        var question = await _questionRepository.GetByIdAsync(
            request.QuestionId,
            cancellationToken
        );
        if (question is null)
            throw new InvalidOperationException($"Question with ID {request.QuestionId} not found");

        // Verify the user owns this question
        if (question.CreatedBy != Guid.Parse(request.UserId))
            throw new UnauthorizedAccessException("You are not authorized to delete this question");

        _questionRepository.Delete(question);
        await _questionRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
