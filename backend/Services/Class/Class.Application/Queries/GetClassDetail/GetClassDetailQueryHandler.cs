using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Enums;
using FrogEdu.Class.Domain.Repositories;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetClassDetail;

public sealed class GetClassDetailQueryHandler
    : IRequestHandler<GetClassDetailQuery, ClassDetailResponse?>
{
    private readonly IClassRoomRepository _classRoomRepository;

    public GetClassDetailQueryHandler(IClassRoomRepository classRoomRepository)
    {
        _classRoomRepository = classRoomRepository;
    }

    public async Task<ClassDetailResponse?> Handle(
        GetClassDetailQuery request,
        CancellationToken cancellationToken
    )
    {
        var classroom = await _classRoomRepository.GetByIdAsync(request.ClassId, cancellationToken);

        if (classroom is null)
            return null;

        var enrollments = classroom
            .Enrollments.Select(e => new EnrollmentDto(
                e.Id,
                e.StudentId,
                e.JoinedAt,
                e.Status.ToString()
            ))
            .ToList();

        var assignments = classroom
            .Assignments.Select(a => new AssignmentResponse(
                a.Id,
                a.ClassId,
                a.ExamId,
                a.StartDate,
                a.DueDate,
                a.IsMandatory,
                a.Weight,
                a.IsActive(),
                a.IsOverdue()
            ))
            .ToList();

        return new ClassDetailResponse(
            classroom.Id,
            classroom.Name,
            classroom.Grade,
            classroom.InviteCode.Value,
            classroom.MaxStudents,
            classroom.BannerUrl,
            classroom.IsActive,
            classroom.TeacherId,
            classroom.CreatedAt,
            classroom.Enrollments.Count(e => e.Status == EnrollmentStatus.Active),
            enrollments,
            assignments
        );
    }
}
