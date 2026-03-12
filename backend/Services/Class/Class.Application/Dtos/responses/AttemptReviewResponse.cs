namespace FrogEdu.Class.Application.Dtos;

/// <summary>
/// Full review of a student's attempt including question content, correct answers and explanations.
/// Used by both students (to see what they got wrong) and teachers (to review a student's performance).
/// </summary>
public sealed record AttemptReviewResponse(
    Guid Id,
    Guid ExamSessionId,
    Guid StudentId,
    string ExamName,
    DateTime StartedAt,
    DateTime? SubmittedAt,
    double Score,
    double TotalPoints,
    double ScorePercentage,
    int AttemptNumber,
    string Status,
    List<QuestionReviewDto> Questions
);

public sealed record QuestionReviewDto(
    Guid QuestionId,
    string Content,
    string Type,
    double Point,
    string? ImageUrl,
    List<AnswerReviewDto> Answers,
    List<string> StudentSelectedAnswerIds,
    double StudentScore,
    bool IsCorrect,
    bool IsPartiallyCorrect,
    /// <summary>For Essay questions: the student's written text.</summary>
    string? EssayStudentText = null,
    /// <summary>For Essay questions: AI-generated feedback.</summary>
    string? EssayAiFeedback = null
);

public sealed record AnswerReviewDto(
    Guid Id,
    string Content,
    bool IsCorrect,
    string? Explanation,
    bool WasSelectedByStudent
);
