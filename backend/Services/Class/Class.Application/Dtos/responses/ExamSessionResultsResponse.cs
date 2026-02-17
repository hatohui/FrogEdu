namespace FrogEdu.Class.Application.Dtos;

/// <summary>
/// Response for teacher/admin to see session results with all attempts
/// </summary>
public sealed record ExamSessionResultsResponse(
    Guid SessionId,
    Guid ExamId,
    Guid ClassId,
    DateTime StartTime,
    DateTime EndTime,
    int TotalAttempts,
    double AverageScore,
    double HighestScore,
    double LowestScore,
    List<AttemptSummaryDto> Attempts
);

public sealed record AttemptSummaryDto(
    Guid AttemptId,
    Guid StudentId,
    string StudentName,
    int AttemptNumber,
    double Score,
    double TotalPoints,
    double ScorePercentage,
    string Status,
    DateTime StartedAt,
    DateTime? SubmittedAt
);
