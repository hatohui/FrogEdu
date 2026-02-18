using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Enums;
using FrogEdu.Class.Domain.Repositories;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetStudentExamSessions;

public sealed class GetStudentExamSessionsQueryHandler
    : IRequestHandler<GetStudentExamSessionsQuery, IReadOnlyList<ExamSessionResponse>>
{
    private readonly IExamSessionRepository _examSessionRepository;
    private readonly IClassEnrollmentRepository _enrollmentRepository;
    private readonly IStudentExamAttemptRepository _attemptRepository;

    public GetStudentExamSessionsQueryHandler(
        IExamSessionRepository examSessionRepository,
        IClassEnrollmentRepository enrollmentRepository,
        IStudentExamAttemptRepository attemptRepository
    )
    {
        _examSessionRepository = examSessionRepository;
        _enrollmentRepository = enrollmentRepository;
        _attemptRepository = attemptRepository;
    }

    public async Task<IReadOnlyList<ExamSessionResponse>> Handle(
        GetStudentExamSessionsQuery request,
        CancellationToken cancellationToken
    )
    {
        if (!Guid.TryParse(request.StudentId, out var studentId))
            return [];

        // Always filter by the user's own active enrollments, regardless of role.
        var enrollments = await _enrollmentRepository.GetByStudentIdAsync(
            studentId,
            cancellationToken
        );

        var classIds = enrollments
            .Where(e => e.Status == EnrollmentStatus.Active)
            .Select(e => e.ClassId)
            .ToList();

        if (classIds.Count == 0)
            return [];

        IReadOnlyList<ExamSession> sessions;

        if (request.UpcomingOnly)
        {
            sessions = await _examSessionRepository.GetUpcomingSessionsForStudentAsync(
                classIds,
                cancellationToken
            );
        }
        else
        {
            sessions = await _examSessionRepository.GetAllSessionsForStudentAsync(
                classIds,
                cancellationToken
            );
        }

        // Deduplicate: per (ClassId, ExamId) keep the most recently created session.
        // This prevents duplicate rows when a teacher accidentally creates two sessions
        // for the same exam in the same class.
        var deduplicated = sessions
            .GroupBy(s => (s.ClassId, s.ExamId))
            .Select(g => g.OrderByDescending(s => s.CreatedAt).First())
            .ToList();

        // Batch-load attempt counts so we avoid N+1 queries.
        var sessionIds = deduplicated.Select(s => s.Id).ToList();
        var attemptCounts = await _attemptRepository.GetAttemptCountsForStudentAsync(
            studentId,
            sessionIds,
            cancellationToken
        );

        return deduplicated
            .Select(s =>
            {
                attemptCounts.TryGetValue(s.Id, out var count);
                return new ExamSessionResponse(
                    s.Id,
                    s.ClassId,
                    s.ExamId,
                    s.StartTime,
                    s.EndTime,
                    s.RetryTimes,
                    s.IsRetryable,
                    s.IsActive,
                    s.ShouldShuffleQuestions,
                    s.ShouldShuffleAnswers,
                    s.AllowPartialScoring,
                    s.IsCurrentlyActive(),
                    s.IsUpcoming(),
                    s.HasEnded(),
                    count,
                    s.CreatedAt
                );
            })
            .ToList();
    }
}
