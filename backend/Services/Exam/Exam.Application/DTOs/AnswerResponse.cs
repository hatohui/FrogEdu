namespace FrogEdu.Exam.Application.DTOs;

public sealed record AnswerResponse(
    Guid Id,
    string Content,
    bool IsCorrect,
    double Point,
    string? Explanation
);
