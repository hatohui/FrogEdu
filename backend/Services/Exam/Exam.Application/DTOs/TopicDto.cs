namespace FrogEdu.Exam.Application.DTOs;

public sealed record TopicDto(
    Guid Id,
    string Title,
    string Description,
    bool IsCurriculum,
    Guid SubjectId
);
