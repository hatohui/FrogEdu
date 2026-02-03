namespace FrogEdu.Exam.Application.DTOs;

public sealed record ExamDto(
    Guid Id,
    string Name,
    string Description,
    Guid SubjectId,
    int Grade,
    bool IsDraft,
    bool IsActive,
    int QuestionCount,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
