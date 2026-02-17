namespace FrogEdu.Class.Application.Dtos;

public sealed record StudentExamAttemptResponse(
    Guid Id,
    Guid ExamSessionId,
    Guid StudentId,
    DateTime StartedAt,
    DateTime? SubmittedAt,
    double Score,
    double TotalPoints,
    double ScorePercentage,
    int AttemptNumber,
    string Status,
    List<StudentAnswerResponse>? Answers
);

public sealed record StudentAnswerResponse(
    Guid Id,
    Guid QuestionId,
    string SelectedAnswerIds,
    double Score,
    bool IsCorrect,
    bool IsPartiallyCorrect
);
