namespace FrogEdu.User.Application.DTOs;

/// <summary>
/// User profile data transfer object
/// </summary>
public sealed record UserDto(
    Guid Id,
    string CognitoId,
    string Email,
    string FirstName,
    string LastName,
    string Role,
    string? AvatarUrl,
    bool IsEmailVerified,
    DateTime? LastLoginAt,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

/// <summary>
/// Simplified user DTO for listings
/// </summary>
public sealed record UserSummaryDto(
    Guid Id,
    string Email,
    string FullName,
    string Role,
    string? AvatarUrl
);
