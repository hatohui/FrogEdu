using FrogEdu.Exam.Domain.Enums;

namespace FrogEdu.Exam.Application.DTOs;

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
