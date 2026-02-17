using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Application.Interfaces;
using FrogEdu.Class.Domain.Enums;
using FrogEdu.Class.Domain.Repositories;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetClassDetail;

public sealed class GetClassDetailQueryHandler
    : IRequestHandler<GetClassDetailQuery, ClassDetailResponse?>
{
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly IUserServiceClient _userServiceClient;

    public GetClassDetailQueryHandler(
        IClassRoomRepository classRoomRepository,
        IUserServiceClient userServiceClient
    )
    {
        _classRoomRepository = classRoomRepository;
        _userServiceClient = userServiceClient;
    }

    public async Task<ClassDetailResponse?> Handle(
        GetClassDetailQuery request,
        CancellationToken cancellationToken
    )
    {
        var classroom = await _classRoomRepository.GetByIdAsync(request.ClassId, cancellationToken);

        if (classroom is null)
            return null;

        // Fetch user details for all enrolled students
        var studentIds = classroom.Enrollments.Select(e => e.StudentId).Distinct().ToList();
        var users = await _userServiceClient.GetUsersByIdsAsync(studentIds, cancellationToken);
        var userDict = users.ToDictionary(u => u.Id);

        var enrollments = classroom
            .Enrollments.Select(e =>
            {
                var user = userDict.GetValueOrDefault(e.StudentId);
                return new EnrollmentWithUserDto(
                    e.Id,
                    e.StudentId,
                    user?.FirstName ?? "Unknown",
                    user?.LastName ?? "User",
                    user?.AvatarUrl,
                    e.JoinedAt,
                    e.Status.ToString()
                );
            })
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
