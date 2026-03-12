using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Application.Interfaces;
using FrogEdu.Class.Domain.Repositories;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetAttemptReview;

public sealed class GetAttemptReviewQueryHandler
    : IRequestHandler<GetAttemptReviewQuery, AttemptReviewResponse?>
{
    private readonly IStudentExamAttemptRepository _attemptRepository;
    private readonly IExamSessionRepository _examSessionRepository;
    private readonly IExamServiceClient _examServiceClient;

    public GetAttemptReviewQueryHandler(
        IStudentExamAttemptRepository attemptRepository,
        IExamSessionRepository examSessionRepository,
        IExamServiceClient examServiceClient
    )
    {
        _attemptRepository = attemptRepository;
        _examSessionRepository = examSessionRepository;
        _examServiceClient = examServiceClient;
    }

    public async Task<AttemptReviewResponse?> Handle(
        GetAttemptReviewQuery request,
        CancellationToken cancellationToken
    )
    {
        var attempt = await _attemptRepository.GetByIdWithAnswersAsync(
            request.AttemptId,
            cancellationToken
        );

        if (attempt is null)
            return null;

        var session = await _examSessionRepository.GetByIdAsync(
            attempt.ExamSessionId,
            cancellationToken
        );

        if (session is null)
            return null;

        var examData = await _examServiceClient.GetExamWithAnswersAsync(
            session.ExamId,
            cancellationToken
        );

        // Build a lookup of student answers keyed by questionId
        var studentAnswerMap = attempt.Answers.ToDictionary(a => a.QuestionId, a => a);

        var questionReviews = (examData?.Questions ?? [])
            .Select(q =>
            {
                studentAnswerMap.TryGetValue(q.Id, out var studentAnswer);

                var selectedIds = studentAnswer is not null
                    ? studentAnswer
                        .SelectedAnswerIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .ToList()
                    : new List<string>();

                var selectedIdSet = selectedIds.ToHashSet(StringComparer.OrdinalIgnoreCase);

                var answerReviews = q
                    .Answers.Select(a => new AnswerReviewDto(
                        Id: a.Id,
                        Content: a.Content,
                        IsCorrect: a.IsCorrect,
                        Explanation: a.Explanation,
                        WasSelectedByStudent: selectedIdSet.Contains(a.Id.ToString())
                    ))
                    .ToList();

                return new QuestionReviewDto(
                    QuestionId: q.Id,
                    Content: q.Content,
                    Type: q.Type,
                    Point: q.Point,
                    ImageUrl: null,
                    Answers: answerReviews,
                    StudentSelectedAnswerIds: selectedIds,
                    StudentScore: studentAnswer?.Score ?? 0,
                    IsCorrect: studentAnswer?.IsCorrect ?? false,
                    IsPartiallyCorrect: studentAnswer?.IsPartiallyCorrect ?? false
                );
            })
            .ToList();

        return new AttemptReviewResponse(
            Id: attempt.Id,
            ExamSessionId: attempt.ExamSessionId,
            StudentId: attempt.StudentId,
            ExamName: examData?.Name ?? "Unknown Exam",
            StartedAt: attempt.StartedAt,
            SubmittedAt: attempt.SubmittedAt,
            Score: attempt.Score,
            TotalPoints: attempt.TotalPoints,
            ScorePercentage: attempt.GetScorePercentage(),
            AttemptNumber: attempt.AttemptNumber,
            Status: attempt.Status.ToString(),
            Questions: questionReviews
        );
    }
}
