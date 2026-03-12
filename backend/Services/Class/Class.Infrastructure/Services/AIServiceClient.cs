using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FrogEdu.Class.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Infrastructure.Services;

public sealed class AIServiceClient : IAIServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AIServiceClient> _logger;
    private readonly string _aiServiceUrl;

    public AIServiceClient(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<AIServiceClient> logger
    )
    {
        _httpClient = httpClient;
        _logger = logger;

        _aiServiceUrl =
            configuration["Services:AIService:Url"]
            ?? Environment.GetEnvironmentVariable("AI_SERVICE_URL")
            ?? "http://localhost:8000/api/ai";
    }

    public async Task<EssayGradingResult?> GradeEssayAsync(
        string questionContent,
        string gradingRubric,
        string studentAnswer,
        double maxPoints,
        int grade,
        string subject,
        string language = "vi",
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var payload = new
            {
                question_content = questionContent,
                grading_rubric = gradingRubric,
                student_answer = studentAnswer,
                max_points = maxPoints,
                grade,
                subject,
                language,
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                $"{_aiServiceUrl}/essay/grade",
                content,
                cancellationToken
            );

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "AI service returned {StatusCode} when grading essay for question",
                    response.StatusCode
                );
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<GradeEssayApiResponse>(
                cancellationToken: cancellationToken
            );

            if (result is null)
                return null;

            return new EssayGradingResult(result.Score, result.Feedback, result.ScorePercentage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling AI service to grade essay");
            return null;
        }
    }

    private sealed record GradeEssayApiResponse(
        [property: JsonPropertyName("score")] double Score,
        [property: JsonPropertyName("feedback")] string Feedback,
        [property: JsonPropertyName("score_percentage")] double ScorePercentage
    );
}
