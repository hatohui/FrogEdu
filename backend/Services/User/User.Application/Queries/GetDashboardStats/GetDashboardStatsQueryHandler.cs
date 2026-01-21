using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Domain.Entities;
using FrogEdu.User.Domain.Repositories;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetDashboardStats;

/// <summary>
/// Handler for GetDashboardStatsQuery
/// </summary>
public sealed class GetDashboardStatsQueryHandler
    : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    private readonly IClassRepository _classRepository;

    public GetDashboardStatsQueryHandler(IClassRepository classRepository)
    {
        _classRepository = classRepository;
    }

    public async Task<DashboardStatsDto> Handle(
        GetDashboardStatsQuery request,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<Class> classes;

        if (request.IsTeacher)
        {
            classes = await _classRepository.GetByTeacherIdAsync(request.UserId, cancellationToken);
        }
        else
        {
            classes = await _classRepository.GetByStudentIdAsync(request.UserId, cancellationToken);
        }

        var activeClasses = classes.Where(c => !c.IsArchived).ToList();
        var classCount = activeClasses.Count;
        var studentCount = activeClasses
            .SelectMany(c => c.Enrollments)
            .Where(e => e.Role == EnrollmentRole.Student)
            .Select(e => e.UserId)
            .Distinct()
            .Count();

        // ExamCount and ContentItemCount would come from other services
        // For now, return 0 (to be integrated via API calls later)
        return new DashboardStatsDto(
            ClassCount: classCount,
            StudentCount: studentCount,
            ExamCount: 0,
            ContentItemCount: 0
        );
    }
}
