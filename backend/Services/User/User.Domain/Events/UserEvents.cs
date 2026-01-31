using FrogEdu.Shared.Kernel;

namespace FrogEdu.User.Domain.Events;

public sealed record UserCreatedDomainEvent(Guid UserId, string Email, Guid RoleId) : DomainEvent;

public sealed record UserProfileUpdatedDomainEvent(Guid UserId, string FirstName, string LastName)
    : DomainEvent;

public sealed record UserAvatarUpdatedDomainEvent(Guid UserId, string AvatarUrl) : DomainEvent;

public sealed record UserLoggedInDomainEvent(Guid UserId, DateTime LoginTime) : DomainEvent;
