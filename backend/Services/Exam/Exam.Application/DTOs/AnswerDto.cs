namespace FrogEdu.Exam.Application.DTOs;

public sealed record AnswerDto(string Content, bool IsCorrect, string? Explanation);
