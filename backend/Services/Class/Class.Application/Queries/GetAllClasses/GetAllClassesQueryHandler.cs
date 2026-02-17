using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Enums;
using FrogEdu.Class.Domain.Repositories;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetAllClasses;

public sealed class GetAllClassesQueryHandler
    : IRequestHandler<GetAllClassesQuery, IReadOnlyList<ClassSummaryResponse>>
{
    private readonly IClassRoomRepository _classRoomRepository;

    public GetAllClassesQueryHandler(IClassRoomRepository classRoomRepository)
    {
        _classRoomRepository = classRoomRepository;
    }

    public async Task<IReadOnlyList<ClassSummaryResponse>> Handle(
        GetAllClassesQuery request,
        CancellationToken cancellationToken
    )
    {
        var classes = await _classRoomRepository.GetAllAsync(cancellationToken);

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
