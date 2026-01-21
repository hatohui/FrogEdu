using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Domain.Entities;
using FrogEdu.User.Domain.Repositories;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetTeacherClasses;

/// <summary>
/// Handler for GetTeacherClassesQuery
/// </summary>
public sealed class GetTeacherClassesQueryHandler
    : IRequestHandler<GetTeacherClassesQuery, IReadOnlyList<ClassDto>>
{
    private readonly IClassRepository _classRepository;
    private readonly IUserRepository _userRepository;

    public GetTeacherClassesQueryHandler(
        IClassRepository classRepository,
        IUserRepository userRepository
    )
    {
        _classRepository = classRepository;
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<ClassDto>> Handle(
        GetTeacherClassesQuery request,
        CancellationToken cancellationToken
    )
    {
        var classes = await _classRepository.GetByTeacherIdAsync(
            request.TeacherId,
            cancellationToken
        );

        // Get teacher info
        var teacher = await _userRepository.GetByIdAsync(request.TeacherId, cancellationToken);
        var teacherName = teacher?.FullName?.ToString();

        var result = classes
            .Where(c => request.IncludeArchived || !c.IsArchived)
            .Select(c => MapToDto(c, teacherName))
            .ToList();

        return result;
    }

    private static ClassDto MapToDto(Class c, string? teacherName)
    {
        return new ClassDto(
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
            InviteCode: c.InviteCode?.Value,
            InviteCodeExpiresAt: c.InviteCode?.ExpiresAt,
            IsArchived: c.IsArchived,
            CreatedAt: c.CreatedAt,
            UpdatedAt: c.UpdatedAt
        );
    }
}
