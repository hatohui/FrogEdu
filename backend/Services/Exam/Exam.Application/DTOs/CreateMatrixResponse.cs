namespace FrogEdu.Exam.Application.DTOs;

public sealed record CreateMatrixResponse(
    Guid Id,
    Guid ExamId,
    int TotalQuestions,
    DateTime CreatedAt
);
