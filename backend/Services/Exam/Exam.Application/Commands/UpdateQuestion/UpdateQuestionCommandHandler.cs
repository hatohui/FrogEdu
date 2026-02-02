using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.UpdateQuestion;

public sealed class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Unit>
{
    private readonly IQuestionRepository _questionRepository;

    public UpdateQuestionCommandHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<Unit> Handle(
        UpdateQuestionCommand request,
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
            throw new UnauthorizedAccessException("You are not authorized to update this question");

        question.Update(
            request.Content,
            request.Point,
            request.Type,
            request.CognitiveLevel,
            request.MediaUrl,
            request.UserId
        );

        _questionRepository.Update(question);
        await _questionRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
