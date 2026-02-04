namespace FrogEdu.Exam.Application.DTOs;

public sealed record CreateMatrixResponse(
    Guid Id,
    string Name,
    Guid SubjectId,
    int Grade,
    int TotalQuestions,
    DateTime CreatedAt
);
