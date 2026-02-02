using FrogEdu.Exam.Domain.Enums;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.CreateQuestion;

public sealed record AnswerDto(string Content, bool IsCorrect, string? Explanation);

public sealed record CreateQuestionCommand(
    string Content,
    double Point,
    QuestionType Type,
    CognitiveLevel CognitiveLevel,
    QuestionSource Source,
    Guid TopicId,
    string? MediaUrl,
    bool IsPublic,
    List<AnswerDto> Answers,
    string UserId
) : IRequest<CreateQuestionResponse>;
