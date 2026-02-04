namespace FrogEdu.Exam.Application.DTOs;

public sealed record AnswerDto(string Content, bool IsCorrect, double Point, string? Explanation);
