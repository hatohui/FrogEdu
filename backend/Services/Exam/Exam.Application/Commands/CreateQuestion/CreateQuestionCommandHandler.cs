using FrogEdu.Exam.Domain.Entities;
using FrogEdu.Exam.Domain.Repositories;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.CreateQuestion;

public sealed class CreateQuestionCommandHandler
    : IRequestHandler<CreateQuestionCommand, CreateQuestionResponse>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly ITopicRepository _topicRepository;

    public CreateQuestionCommandHandler(
        IQuestionRepository questionRepository,
        ITopicRepository topicRepository
    )
    {
        _questionRepository = questionRepository;
        _topicRepository = topicRepository;
    }

    public async Task<CreateQuestionResponse> Handle(
        CreateQuestionCommand request,
        CancellationToken cancellationToken
    )
    {
        // Verify topic exists
        var topic = await _topicRepository.GetByIdAsync(request.TopicId, cancellationToken);
        if (topic is null)
            throw new InvalidOperationException($"Topic with ID {request.TopicId} not found");

        var question = Question.Create(
            request.Content,
            request.Point,
            request.Type,
            request.CognitiveLevel,
            request.Source,
            request.TopicId,
            request.UserId,
            request.MediaUrl,
            request.IsPublic
        );

        // Add answers to the question
        foreach (var answerDto in request.Answers)
        {
            var answer = Answer.Create(
                answerDto.Content,
                answerDto.IsCorrect,
                question.Id,
                answerDto.Explanation
            );
            question.AddAnswer(answer);
        }

        await _questionRepository.AddAsync(question, cancellationToken);
        await _questionRepository.SaveChangesAsync(cancellationToken);

        return new CreateQuestionResponse(
            question.Id,
            question.Content,
            question.Point,
            question.Type,
            question.CognitiveLevel,
            question.Source,
            question.TopicId,
            question.MediaUrl,
            question.IsPublic,
            question
                .Answers.Select(a => new AnswerResponse(
                    a.Id,
                    a.Content,
                    a.IsCorrect,
                    a.Explanation
                ))
                .ToList(),
            question.CreatedAt
        );
    }
}
