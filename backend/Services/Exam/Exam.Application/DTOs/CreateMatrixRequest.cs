namespace FrogEdu.Exam.Application.DTOs;

public sealed record CreateMatrixRequest(
    string Name,
    string? Description,
    Guid SubjectId,
    int Grade,
    List<MatrixTopicDto> MatrixTopics
);
