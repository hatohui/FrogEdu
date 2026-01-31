namespace FrogEdu.User.Application.DTOs;

public sealed record UserDto(
    Guid Id,
    string CognitoId,
    string Email,
    string FirstName,
    string LastName,
    Guid RoleId,
    string? AvatarUrl,
    bool IsEmailVerified,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public sealed record UserSummaryDto(
    Guid Id,
    string Email,
    string FullName,
    Guid RoleId,
    string? AvatarUrl
);
