namespace FrogEdu.Class.Application.Dtos.requests;

public sealed record UpdateExamSessionRequest(
    DateTime StartTime,
    DateTime EndTime,
    int RetryTimes,
    bool IsRetryable,
    bool ShouldShuffleQuestions,
    bool ShouldShuffleAnswers,
    bool AllowPartialScoring
);
