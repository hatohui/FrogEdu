namespace FrogEdu.Class.Application.Dtos;

public sealed record AssignmentResponse(
    Guid Id,
    Guid ClassId,
    Guid ExamId,
    DateTime StartDate,
    DateTime DueDate,
    bool IsMandatory,
    int Weight,
    bool IsActive,
    bool IsOverdue
);
