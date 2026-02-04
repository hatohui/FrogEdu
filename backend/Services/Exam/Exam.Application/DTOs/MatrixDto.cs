namespace FrogEdu.Exam.Application.DTOs;

public sealed record MatrixDto(
    Guid Id,
    string Name,
    string? Description,
    Guid SubjectId,
    int Grade,
    List<MatrixTopicDto> MatrixTopics,
    int TotalQuestionCount,
    DateTime CreatedAt
);
