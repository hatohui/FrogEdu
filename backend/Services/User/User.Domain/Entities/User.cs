using FrogEdu.Shared.Kernel.Auditing;
using FrogEdu.Shared.Kernel.Deletion;
using FrogEdu.Shared.Kernel.Primitives;
using FrogEdu.User.Domain.Enums;
using FrogEdu.User.Domain.Events;
using FrogEdu.User.Domain.ValueObjects;

namespace FrogEdu.User.Domain.Entities;

public sealed class User : AuditableSoftdeletableEntity
{
    public CognitoUserId CognitoId { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public Guid RoleId { get; private set; }
    public string? AvatarUrl { get; private set; }
    public bool IsEmailVerified { get; private set; }

    private User() { }

    private User(
        CognitoUserId cognitoId,
        Email email,
        string firstName,
        string lastName,
        Guid roleId
    )
        : base()
    {
        CognitoId = cognitoId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        RoleId = roleId;
        IsEmailVerified = false;
    }

    public static User Create(
        string cognitoId,
        string email,
        string firstName,
        string lastName,
        Guid roleId
    )
    {
        var user = new User(
            CognitoUserId.Create(cognitoId),
            Email.Create(email),
            firstName,
            lastName,
            roleId
        );

        user.AddDomainEvent(new UserCreatedDomainEvent(user.Id, user.Email.Value, user.RoleId));

        return user;
    }

    public void UpdateAvatar(string avatarUrl)
    {
        if (string.IsNullOrWhiteSpace(avatarUrl))
            throw new ArgumentException("Avatar URL cannot be empty", nameof(avatarUrl));

        AvatarUrl = avatarUrl;
        MarkAsUpdated();
        AddDomainEvent(new UserAvatarUpdatedDomainEvent(Id, avatarUrl));
    }

    public void VerifyEmail()
    {
        if (IsEmailVerified)
            return;

        IsEmailVerified = true;
    }

    public void ChangeRole(Guid newRoleId)
    {
        if (RoleId == newRoleId)
            return;

        RoleId = newRoleId;
        MarkAsUpdated();
    }

    public void UpdateProfile(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
        MarkAsUpdated();
        AddDomainEvent(new UserProfileUpdatedDomainEvent(Id, firstName, lastName));
    }
}
