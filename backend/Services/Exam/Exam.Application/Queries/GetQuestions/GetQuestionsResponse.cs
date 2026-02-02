using FrogEdu.Exam.Domain.Enums;

namespace FrogEdu.Exam.Application.Queries.GetQuestions;

public sealed record QuestionDto(
    Guid Id,
    string Content,
    double Point,
    QuestionType Type,
    CognitiveLevel CognitiveLevel,
    QuestionSource Source,
    Guid TopicId,
    string? MediaUrl,
    bool IsPublic,
    int AnswerCount,
    DateTime CreatedAt
);

public sealed record GetQuestionsResponse(List<QuestionDto> Questions);
