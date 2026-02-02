using FrogEdu.Exam.Domain.Enums;

namespace FrogEdu.Exam.Application.DTOs;

public sealed record UpdateQuestionRequest(
    string Content,
    double Point,
    QuestionType Type,
    CognitiveLevel CognitiveLevel,
    string? MediaUrl
);
