using System.Net.Http.Json;
using FrogEdu.Class.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Infrastructure.Services;

public sealed class ExamServiceClient : IExamServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExamServiceClient> _logger;
    private readonly string _examServiceUrl;

    public ExamServiceClient(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ExamServiceClient> logger
    )
    {
        _httpClient = httpClient;
        _logger = logger;

        _examServiceUrl =
            configuration["Services:ExamService:Url"]
            ?? Environment.GetEnvironmentVariable("EXAM_SERVICE_URL")
            ?? "http://localhost:5002/api/exams";
    }

    public async Task<ExamWithQuestionsDto?> GetExamWithAnswersAsync(
        Guid examId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"{_examServiceUrl}/exams/{examId}/session-data",
                cancellationToken
            );

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Failed to fetch exam {ExamId} from Exam service. Status: {StatusCode}",
                    examId,
                    response.StatusCode
                );
                return null;
            }

            var sessionData = await response.Content.ReadFromJsonAsync<ExamSessionDataResponse>(
                cancellationToken: cancellationToken
            );

            if (sessionData is null)
                return null;

            var questions = sessionData
                .Questions.Select(q => new ExamQuestionDto(
                    q.Id,
                    q.Content,
                    q.Points,
                    MapQuestionType(q.QuestionType),
                    q.Answers.Select(a => new ExamAnswerDto(a.Id, a.Content, a.IsCorrect)).ToList()
                ))
                .ToList();

            return new ExamWithQuestionsDto(
                sessionData.Id,
                sessionData.Name,
                sessionData.QuestionCount,
                questions
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching exam {ExamId} from Exam service", examId);
            return null;
        }
    }

    private static string MapQuestionType(int questionType) =>
        questionType switch
        {
            1 => "MultipleChoice",
            2 => "MultipleAnswer",
            3 => "TrueFalse",
            4 => "Essay",
            5 => "FillInTheBlank",
            _ => "Unknown",
        };

    // Internal response models matching GET /exams/{examId}/session-data
    private sealed record ExamSessionDataResponse(
        Guid Id,
        string Name,
        string Description,
        int QuestionCount,
        double TotalPoints,
        List<SessionQuestionResponse> Questions
    );

    private sealed record SessionQuestionResponse(
        Guid Id,
        string Content,
        double Points,
        int QuestionType,
        string? ImageUrl,
        List<SessionAnswerResponse> Answers
    );

    private sealed record SessionAnswerResponse(Guid Id, string Content, bool IsCorrect);
}
