namespace FrogEdu.Exam.Application.DTOs;

public sealed record CreateExamResponse(
    Guid Id,
    string Name,
    string Description,
    Guid TopicId,
    Guid SubjectId,
    int Grade,
    bool IsDraft,
    bool IsActive,
    DateTime CreatedAt
);
