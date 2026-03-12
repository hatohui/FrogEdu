namespace FrogEdu.Class.Application.Interfaces;

public interface IAIServiceClient
{
    /// <summary>
    /// Grade a student's essay answer using AI.
    /// Returns score (clamped to [0, maxPoints]) and feedback text.
    /// Returns null if the AI service is unavailable.
    /// </summary>
    Task<EssayGradingResult?> GradeEssayAsync(
        string questionContent,
        string gradingRubric,
        string studentAnswer,
        double maxPoints,
        int grade,
        string subject,
        string language = "vi",
        CancellationToken cancellationToken = default
    );
}

public sealed record EssayGradingResult(double Score, string Feedback, double ScorePercentage);
