namespace FrogEdu.Class.Application.Dtos.requests;

public sealed record AssignExamRequest(
    Guid ExamId,
    DateTime StartDate,
    DateTime DueDate,
    bool IsMandatory = true,
    int Weight = 100,
    int RetryTimes = 0,
    bool IsRetryable = false,
    bool ShouldShuffleQuestions = false,
    bool ShouldShuffleAnswers = false,
    bool AllowPartialScoring = true
);
