namespace FrogEdu.Exam.Application.DTOs;

public sealed record CreateMatrixRequest(Guid ExamId, List<MatrixTopicDto> MatrixTopics);
