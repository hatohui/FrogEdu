using FrogEdu.Shared.Kernel;

namespace FrogEdu.User.Domain.Events;

/// <summary>
/// Event raised when a new user is created
/// </summary>
public sealed record UserCreatedDomainEvent(Guid UserId, string Email, string Role) : DomainEvent;

/// <summary>
/// Event raised when a user's profile is updated
/// </summary>
public sealed record UserProfileUpdatedDomainEvent(Guid UserId, string FirstName, string LastName)
    : DomainEvent;

/// <summary>
/// Event raised when a user's avatar is updated
/// </summary>
public sealed record UserAvatarUpdatedDomainEvent(Guid UserId, string AvatarUrl) : DomainEvent;

/// <summary>
/// Event raised when a user logs in
/// </summary>
public sealed record UserLoggedInDomainEvent(Guid UserId, DateTime LoginTime) : DomainEvent;
