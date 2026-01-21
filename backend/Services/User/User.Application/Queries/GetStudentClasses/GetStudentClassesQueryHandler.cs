using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Domain.Entities;
using FrogEdu.User.Domain.Repositories;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetStudentClasses;

/// <summary>
/// Handler for GetStudentClassesQuery
/// </summary>
public sealed class GetStudentClassesQueryHandler
    : IRequestHandler<GetStudentClassesQuery, IReadOnlyList<ClassDto>>
{
    private readonly IClassRepository _classRepository;
    private readonly IUserRepository _userRepository;

    public GetStudentClassesQueryHandler(
        IClassRepository classRepository,
        IUserRepository userRepository
    )
    {
        _classRepository = classRepository;
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<ClassDto>> Handle(
        GetStudentClassesQuery request,
        CancellationToken cancellationToken
    )
    {
        var classes = await _classRepository.GetByStudentIdAsync(
            request.StudentId,
            cancellationToken
        );

        var result = new List<ClassDto>();
        foreach (var c in classes.Where(c => !c.IsArchived))
        {
            var teacher = await _userRepository.GetByIdAsync(
                c.HomeroomTeacherId,
                cancellationToken
            );
            var teacherName = teacher?.FullName?.ToString();

            result.Add(
                new ClassDto(
                    Id: c.Id,
                    Name: c.Name,
                    Subject: c.Subject,
                    Grade: c.Grade,
                    HomeroomTeacherId: c.HomeroomTeacherId,
                    TeacherName: teacherName,
                    School: c.School,
                    Description: c.Description,
                    MaxStudents: c.MaxStudents,
                    StudentCount: c.Enrollments.Count(e => e.Role == EnrollmentRole.Student),
                    InviteCode: null, // Students don't see invite codes
                    InviteCodeExpiresAt: null,
                    IsArchived: c.IsArchived,
                    CreatedAt: c.CreatedAt,
                    UpdatedAt: c.UpdatedAt
                )
            );
        }

        return result;
    }
}
