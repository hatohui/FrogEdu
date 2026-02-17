namespace FrogEdu.Class.Application.Dtos.requests;

public sealed record CreateExamSessionRequest(
    Guid ExamId,
    DateTime StartTime,
    DateTime EndTime,
    int RetryTimes,
    bool IsRetryable,
    bool ShouldShuffleQuestions,
    bool ShouldShuffleAnswers,
    bool AllowPartialScoring = true
);
