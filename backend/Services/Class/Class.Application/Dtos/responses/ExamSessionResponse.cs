namespace FrogEdu.Class.Application.Dtos;

public sealed record ExamSessionResponse(
    Guid Id,
    Guid ClassId,
    Guid ExamId,
    DateTime StartTime,
    DateTime EndTime,
    int RetryTimes,
    bool IsRetryable,
    bool IsActive,
    bool ShouldShuffleQuestions,
    bool ShouldShuffleAnswers,
    bool AllowPartialScoring,
    bool IsCurrentlyActive,
    bool IsUpcoming,
    bool HasEnded,
    int AttemptCount,
    DateTime CreatedAt
);
