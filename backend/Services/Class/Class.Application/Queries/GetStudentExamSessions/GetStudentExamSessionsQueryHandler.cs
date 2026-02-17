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

    public GetStudentExamSessionsQueryHandler(
        IExamSessionRepository examSessionRepository,
        IClassEnrollmentRepository enrollmentRepository
    )
    {
        _examSessionRepository = examSessionRepository;
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task<IReadOnlyList<ExamSessionResponse>> Handle(
        GetStudentExamSessionsQuery request,
        CancellationToken cancellationToken
    )
    {
        if (!Guid.TryParse(request.StudentId, out var studentId))
            return [];

        // Get all classes the student is enrolled in
        var enrollments = await _enrollmentRepository.GetByStudentIdAsync(
            studentId,
            cancellationToken
        );

        var activeClassIds = enrollments
            .Where(e => e.Status == EnrollmentStatus.Active)
            .Select(e => e.ClassId)
            .ToList();

        if (activeClassIds.Count == 0)
            return [];

        IReadOnlyList<ExamSession> sessions;

        if (request.UpcomingOnly)
        {
            sessions = await _examSessionRepository.GetUpcomingSessionsForStudentAsync(
                activeClassIds,
                cancellationToken
            );
        }
        else
        {
            sessions = await _examSessionRepository.GetActiveSessionsForStudentAsync(
                activeClassIds,
                cancellationToken
            );
        }

        return sessions
            .Select(s => new ExamSessionResponse(
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
                0,
                s.CreatedAt
            ))
            .ToList();
    }
}
