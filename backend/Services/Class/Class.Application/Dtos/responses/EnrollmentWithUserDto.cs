namespace FrogEdu.Class.Application.Dtos;

public sealed record EnrollmentWithUserDto(
    Guid Id,
    Guid StudentId,
    string StudentFirstName,
    string StudentLastName,
    string? StudentAvatarUrl,
    DateTime JoinedAt,
    string Status
);
