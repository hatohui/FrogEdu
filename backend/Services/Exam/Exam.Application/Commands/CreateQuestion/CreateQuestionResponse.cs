using FrogEdu.Exam.Domain.Enums;

namespace FrogEdu.Exam.Application.Commands.CreateQuestion;

public sealed record AnswerResponse(Guid Id, string Content, bool IsCorrect, string? Explanation);

public sealed record CreateQuestionResponse(
    Guid Id,
    string Content,
    double Point,
    QuestionType Type,
    CognitiveLevel CognitiveLevel,
    QuestionSource Source,
    Guid TopicId,
    string? MediaUrl,
    bool IsPublic,
    List<AnswerResponse> Answers,
    DateTime CreatedAt
);
