namespace FrogEdu.Exam.Application.DTOs;

public sealed record UpdateMatrixRequest(
    string? Name,
    string? Description,
    List<MatrixTopicDto> MatrixTopics
);
