using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Enums;
using FrogEdu.Class.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Queries.GetMyClasses;

public sealed class GetMyClassesQueryHandler
    : IRequestHandler<GetMyClassesQuery, IReadOnlyList<ClassSummaryResponse>>
{
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly ILogger<GetMyClassesQueryHandler> _logger;

    public GetMyClassesQueryHandler(
        IClassRoomRepository classRoomRepository,
        ILogger<GetMyClassesQueryHandler> logger
    )
    {
        _classRoomRepository = classRoomRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ClassSummaryResponse>> Handle(
        GetMyClassesQuery request,
        CancellationToken cancellationToken
    )
    {
        if (!Guid.TryParse(request.UserId, out var userId))
        {
            _logger.LogWarning("Invalid UserId format: {UserId}", request.UserId);
            return [];
        }

        var classes = request.Role switch
        {
            "Teacher" => await _classRoomRepository.GetByTeacherIdAsync(userId, cancellationToken),
            "Student" => await _classRoomRepository.GetByStudentIdAsync(userId, cancellationToken),
            _ => await _classRoomRepository.GetByTeacherIdAsync(userId, cancellationToken),
        };

        return classes
            .Select(c => new ClassSummaryResponse(
                c.Id,
                c.Name,
                c.Grade,
                c.InviteCode.Value,
                c.MaxStudents,
                c.BannerUrl,
                c.IsActive,
                c.TeacherId,
                c.CreatedAt,
                c.Enrollments.Count(e => e.Status == EnrollmentStatus.Active),
                c.Assignments.Count
            ))
            .ToList();
    }
}
