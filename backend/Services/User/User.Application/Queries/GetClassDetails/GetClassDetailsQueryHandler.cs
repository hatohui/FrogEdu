using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Domain.Entities;
using FrogEdu.User.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Application.Queries.GetClassDetails;

/// <summary>
/// Handler for GetClassDetailsQuery
/// </summary>
public sealed class GetClassDetailsQueryHandler
    : IRequestHandler<GetClassDetailsQuery, Result<ClassDetailsDto>>
{
    private readonly IClassRepository _classRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetClassDetailsQueryHandler> _logger;

    public GetClassDetailsQueryHandler(
        IClassRepository classRepository,
        IUserRepository userRepository,
        ILogger<GetClassDetailsQueryHandler> logger
    )
    {
        _classRepository = classRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<ClassDetailsDto>> Handle(
        GetClassDetailsQuery request,
        CancellationToken cancellationToken
    )
    {
        var classEntity = await _classRepository.GetByIdWithEnrollmentsAsync(
            request.ClassId,
            cancellationToken
        );
        if (classEntity is null)
        {
            return Result<ClassDetailsDto>.Failure("Class not found");
        }

        // Check if requester has access (is teacher or enrolled student)
        var isTeacher = classEntity.HomeroomTeacherId == request.RequesterId;
        var isEnrolled = classEntity.Enrollments.Any(e => e.UserId == request.RequesterId);

        if (!isTeacher && !isEnrolled)
        {
            _logger.LogWarning(
                "Unauthorized class access attempt: {RequesterId} for class {ClassId}",
                request.RequesterId,
                request.ClassId
            );
            return Result<ClassDetailsDto>.Failure("You do not have access to this class");
        }

        // Get teacher info
        var teacher = await _userRepository.GetByIdAsync(
            classEntity.HomeroomTeacherId,
            cancellationToken
        );
        var teacherName = teacher?.FullName?.ToString();

        // Get member details
        var members = new List<ClassMemberDto>();
        foreach (var enrollment in classEntity.Enrollments)
        {
            var user = await _userRepository.GetByIdAsync(enrollment.UserId, cancellationToken);
            if (user != null)
            {
                members.Add(
                    new ClassMemberDto(
                        UserId: user.Id,
                        FullName: user.FullName?.ToString() ?? "Unknown",
                        AvatarUrl: user.AvatarUrl,
                        Role: enrollment.Role.ToString(),
                        JoinedAt: enrollment.EnrolledAt
                    )
                );
            }
        }

        // Only show invite code to teachers
        var inviteCode = isTeacher ? classEntity.InviteCode?.Value : null;
        var inviteCodeExpiresAt = isTeacher ? classEntity.InviteCode?.ExpiresAt : null;

        var result = new ClassDetailsDto(
            Id: classEntity.Id,
            Name: classEntity.Name,
            Subject: classEntity.Subject,
            Grade: classEntity.Grade,
            HomeroomTeacherId: classEntity.HomeroomTeacherId,
            TeacherName: teacherName,
            School: classEntity.School,
            Description: classEntity.Description,
            MaxStudents: classEntity.MaxStudents,
            InviteCode: inviteCode,
            InviteCodeExpiresAt: inviteCodeExpiresAt,
            IsArchived: classEntity.IsArchived,
            CreatedAt: classEntity.CreatedAt,
            UpdatedAt: classEntity.UpdatedAt,
            Members: members.AsReadOnly()
        );

        return Result<ClassDetailsDto>.Success(result);
    }
}
