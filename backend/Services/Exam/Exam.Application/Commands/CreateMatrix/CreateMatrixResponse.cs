namespace FrogEdu.Exam.Application.Commands.CreateMatrix;

public sealed record CreateMatrixResponse(
    Guid Id,
    Guid ExamId,
    int TotalQuestions,
    DateTime CreatedAt
);
