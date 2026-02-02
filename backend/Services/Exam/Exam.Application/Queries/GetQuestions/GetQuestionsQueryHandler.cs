using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetQuestions;

public sealed class GetQuestionsQueryHandler
    : IRequestHandler<GetQuestionsQuery, GetQuestionsResponse>
{
    private readonly IQuestionRepository _questionRepository;

    public GetQuestionsQueryHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<GetQuestionsResponse> Handle(
        GetQuestionsQuery request,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<Domain.Entities.Question> questions;

        if (request.TopicId.HasValue)
        {
            questions = await _questionRepository.GetByTopicIdAsync(
                request.TopicId.Value,
                cancellationToken
            );

            if (request.CognitiveLevel.HasValue)
            {
                questions = questions
                    .Where(q => q.CognitiveLevel == request.CognitiveLevel.Value)
                    .ToList();
            }
        }
        else
        {
            var userId = Guid.Parse(request.UserId);
            questions = await _questionRepository.GetByCreatorAsync(userId, cancellationToken);
        }

        if (request.IsPublic.HasValue)
        {
            questions = questions.Where(q => q.IsPublic == request.IsPublic.Value).ToList();
        }

        var questionDtos = questions
            .Select(q => new QuestionDto(
                q.Id,
                q.Content,
                q.Point,
                q.Type,
                q.CognitiveLevel,
                q.Source,
                q.TopicId,
                q.MediaUrl,
                q.IsPublic,
                q.Answers.Count,
                q.CreatedAt
            ))
            .ToList();

        return new GetQuestionsResponse(questionDtos);
    }
}
