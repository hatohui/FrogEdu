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
    private readonly IClassRoomRepository _classRoomRepository;

    public GetStudentExamSessionsQueryHandler(
        IExamSessionRepository examSessionRepository,
        IClassEnrollmentRepository enrollmentRepository,
        IClassRoomRepository classRoomRepository
    )
    {
        _examSessionRepository = examSessionRepository;
        _enrollmentRepository = enrollmentRepository;
        _classRoomRepository = classRoomRepository;
    }

    public async Task<IReadOnlyList<ExamSessionResponse>> Handle(
        GetStudentExamSessionsQuery request,
        CancellationToken cancellationToken
    )
    {
        if (!Guid.TryParse(request.StudentId, out var studentId))
            return [];

        List<Guid> classIds;

        if (request.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            // Admins see sessions across all classes
            var allClasses = await _classRoomRepository.GetAllAsync(cancellationToken);
            classIds = allClasses.Select(c => c.Id).ToList();
        }
        else
        {
            // Students/Teachers see sessions only for their enrolled classes
            var enrollments = await _enrollmentRepository.GetByStudentIdAsync(
                studentId,
                cancellationToken
            );

            classIds = enrollments
                .Where(e => e.Status == EnrollmentStatus.Active)
                .Select(e => e.ClassId)
                .ToList();
        }

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
