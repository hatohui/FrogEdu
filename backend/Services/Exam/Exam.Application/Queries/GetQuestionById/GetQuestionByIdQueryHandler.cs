using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetQuestionById;

public sealed class GetQuestionByIdQueryHandler
    : IRequestHandler<GetQuestionByIdQuery, QuestionDto?>
{
    private readonly IQuestionRepository _questionRepository;

    public GetQuestionByIdQueryHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<QuestionDto?> Handle(
        GetQuestionByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var question = await _questionRepository.GetByIdAsync(
            request.QuestionId,
            cancellationToken
        );

        if (question is null)
            return null;

        // Only return if question is public or user owns it
        var userId = Guid.Parse(request.UserId);
        if (!question.IsPublic && question.CreatedBy != userId)
            return null;

        return new QuestionDto(
            question.Id,
            question.Content,
            question.Point,
            question.Type,
            question.CognitiveLevel,
            question.Source,
            question.TopicId,
            question.MediaUrl,
            question.IsPublic,
            question.Answers.Count,
            question.CreatedAt
        );
    }
}
