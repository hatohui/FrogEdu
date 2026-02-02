namespace FrogEdu.Exam.Application.DTOs;

public sealed record AnswerResponse(Guid Id, string Content, bool IsCorrect, string? Explanation);
