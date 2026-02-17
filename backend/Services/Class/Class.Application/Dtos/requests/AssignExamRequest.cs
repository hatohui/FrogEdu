namespace FrogEdu.Class.Application.Dtos.requests;

public sealed record AssignExamRequest(
    Guid ExamId,
    DateTime StartDate,
    DateTime DueDate,
    bool IsMandatory = true,
    int Weight = 100
);
