using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Application.Interfaces;
using FrogEdu.Class.Domain.Repositories;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetSessionResults;

public sealed class GetSessionResultsQueryHandler
    : IRequestHandler<GetSessionResultsQuery, ExamSessionResultsResponse?>
{
    private readonly IExamSessionRepository _examSessionRepository;
    private readonly IStudentExamAttemptRepository _attemptRepository;
    private readonly IUserServiceClient _userServiceClient;

    public GetSessionResultsQueryHandler(
        IExamSessionRepository examSessionRepository,
        IStudentExamAttemptRepository attemptRepository,
        IUserServiceClient userServiceClient
    )
    {
        _examSessionRepository = examSessionRepository;
        _attemptRepository = attemptRepository;
        _userServiceClient = userServiceClient;
    }

    public async Task<ExamSessionResultsResponse?> Handle(
        GetSessionResultsQuery request,
        CancellationToken cancellationToken
    )
    {
        var session = await _examSessionRepository.GetByIdAsync(
            request.ExamSessionId,
            cancellationToken
        );

        if (session is null)
            return null;

        var attempts = await _attemptRepository.GetBySessionIdAsync(
            request.ExamSessionId,
            cancellationToken
        );

        // Get user info for all students who attempted
        var studentIds = attempts.Select(a => a.StudentId).Distinct().ToList();
        var users = await _userServiceClient.GetUsersByIdsAsync(studentIds, cancellationToken);
        var userMap = users.ToDictionary(u => u.Id, u => u);

        var attemptSummaries = attempts
            .Select(a =>
            {
                var user = userMap.GetValueOrDefault(a.StudentId);
                var name = user is not null
                    ? $"{user.FirstName} {user.LastName}"
                    : "Unknown Student";

                return new AttemptSummaryDto(
                    a.Id,
                    a.StudentId,
                    name,
                    a.AttemptNumber,
                    a.Score,
                    a.TotalPoints,
                    a.GetScorePercentage(),
                    a.Status.ToString(),
                    a.StartedAt,
                    a.SubmittedAt
                );
            })
            .OrderBy(a => a.StudentName)
            .ThenBy(a => a.AttemptNumber)
            .ToList();

        var submittedAttempts = attempts
            .Where(a => a.Status != Domain.Enums.AttemptStatus.InProgress)
            .ToList();

        return new ExamSessionResultsResponse(
            session.Id,
            session.ExamId,
            session.ClassId,
            session.StartTime,
            session.EndTime,
            attempts.Count,
            submittedAttempts.Count > 0
                ? Math.Round(submittedAttempts.Average(a => a.GetScorePercentage()), 2)
                : 0,
            submittedAttempts.Count > 0 ? submittedAttempts.Max(a => a.GetScorePercentage()) : 0,
            submittedAttempts.Count > 0 ? submittedAttempts.Min(a => a.GetScorePercentage()) : 0,
            attemptSummaries
        );
    }
}
