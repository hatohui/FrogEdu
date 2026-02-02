namespace FrogEdu.Exam.Application.Commands.CreateExam;

public sealed record CreateExamResponse(
    Guid Id,
    string Title,
    int Duration,
    int PassScore,
    int MaxAttempts,
    DateTime StartTime,
    DateTime EndTime,
    Guid TopicId,
    bool IsDraft,
    bool IsActive,
    DateTime CreatedAt
);
