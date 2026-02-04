using FrogEdu.Exam.Domain.Enums;

namespace FrogEdu.Exam.Application.DTOs;

public sealed record MatrixTopicDto(
    Guid TopicId,
    string? TopicTitle,
    CognitiveLevel CognitiveLevel,
    int Quantity
);
