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

/// <summary>
/// Extended user DTO with subscription information for API responses
/// </summary>
public sealed record UserWithSubscriptionDto
{
    public Guid Id { get; init; }
    public string CognitoId { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public Guid RoleId { get; init; }
    public string? AvatarUrl { get; init; }
    public bool IsEmailVerified { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public SubscriptionInfoDto Subscription { get; init; } = null!;
}

/// <summary>
/// Subscription information DTO
/// </summary>
public sealed record SubscriptionInfoDto
{
    public string Plan { get; init; } = "free";
    public long ExpiresAt { get; init; }
    public bool HasActiveSubscription { get; init; }
}
