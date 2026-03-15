using FrogEdu.Class.Domain.Enums;
using FrogEdu.Class.Domain.Repositories;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetClassDashboardStats;

public sealed class GetClassDashboardStatsQueryHandler(
    IClassRoomRepository classRoomRepository,
    IExamSessionRepository examSessionRepository
) : IRequestHandler<GetClassDashboardStatsQuery, ClassDashboardStatsResponse>
{
    private readonly IClassRoomRepository _classRoomRepository = classRoomRepository;
    private readonly IExamSessionRepository _examSessionRepository = examSessionRepository;

    public async Task<ClassDashboardStatsResponse> Handle(
        GetClassDashboardStatsQuery request,
        CancellationToken cancellationToken
    )
    {
        var allClasses = await _classRoomRepository.GetAllAsync(cancellationToken);
        var allSessions = await _examSessionRepository.GetAllAsync(cancellationToken);

        var now = DateTime.UtcNow;
        var activeClasses = allClasses.Count(c => c.IsActive);
        var activeSessions = allSessions.Count(s =>
            s.IsActive && s.StartTime <= now && s.EndTime >= now
        );

        var allAttempts = allSessions.SelectMany(s => s.Attempts).ToList();
        var submittedAttempts = allAttempts.Count(a =>
            a.Status == AttemptStatus.Submitted || a.Status == AttemptStatus.Graded
        );

        return new ClassDashboardStatsResponse(
            TotalClasses: allClasses.Count,
            ActiveClasses: activeClasses,
            TotalExamSessions: allSessions.Count,
            ActiveExamSessions: activeSessions,
            TotalAttempts: allAttempts.Count,
            SubmittedAttempts: submittedAttempts
        );
    }
}
