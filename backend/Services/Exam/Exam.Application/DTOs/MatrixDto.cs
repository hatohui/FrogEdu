namespace FrogEdu.Exam.Application.DTOs;

public sealed record MatrixDto(
    Guid Id,
    Guid ExamId,
    List<MatrixTopicDto> MatrixTopics,
    int TotalQuestionCount,
    DateTime CreatedAt
);
