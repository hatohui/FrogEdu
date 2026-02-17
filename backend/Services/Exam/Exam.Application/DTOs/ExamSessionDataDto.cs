using FrogEdu.Exam.Domain.Enums;

namespace FrogEdu.Exam.Application.DTOs;

/// <summary>
/// DTO for returning exam data needed by exam sessions (grading + student display).
/// Includes question and answer GUIDs required for submission and scoring.
/// </summary>
public sealed record ExamSessionDataDto(
    Guid Id,
    string Name,
    string Description,
    int QuestionCount,
    double TotalPoints,
    IReadOnlyList<SessionQuestionDto> Questions
);

public sealed record SessionQuestionDto(
    Guid Id,
    string Content,
    double Points,
    QuestionType QuestionType,
    string? ImageUrl,
    IReadOnlyList<SessionAnswerDto> Answers
);

public sealed record SessionAnswerDto(Guid Id, string Content, bool IsCorrect);
