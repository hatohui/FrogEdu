namespace FrogEdu.Exam.Application.Queries.GetExams;

public sealed record ExamDto(
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
    string? AccessCode,
    int QuestionCount,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public sealed record GetExamsResponse(List<ExamDto> Exams);
