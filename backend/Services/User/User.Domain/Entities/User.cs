using FrogEdu.Shared.Kernel;
using FrogEdu.User.Domain.Enums;
using FrogEdu.User.Domain.Events;
using FrogEdu.User.Domain.ValueObjects;

namespace FrogEdu.User.Domain.Entities;

/// <summary>
/// User aggregate root
/// </summary>
public sealed class User : Entity
{
    public CognitoUserId CognitoId { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public FullName FullName { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public string? AvatarUrl { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public bool IsEmailVerified { get; private set; }

    // Private constructor for EF Core
    private User() { }

    private User(CognitoUserId cognitoId, Email email, FullName fullName, UserRole role)
    {
        CognitoId = cognitoId;
        Email = email;
        FullName = fullName;
        Role = role;
        IsEmailVerified = false;
    }

    /// <summary>
    /// Factory method to create a new user
    /// </summary>
    public static User Create(
        string cognitoId,
        string email,
        string firstName,
        string lastName,
        UserRole role
    )
    {
        var user = new User(
            CognitoUserId.Create(cognitoId),
            Email.Create(email),
            FullName.Create(firstName, lastName),
            role
        );

        user.AddDomainEvent(
            new UserCreatedDomainEvent(user.Id, user.Email.Value, user.Role.ToString())
        );

        return user;
    }

    /// <summary>
    /// Update user profile information
    /// </summary>
    public void UpdateProfile(string firstName, string lastName)
    {
        FullName = FullName.Create(firstName, lastName);
        MarkAsUpdated();

        AddDomainEvent(new UserProfileUpdatedDomainEvent(Id, firstName, lastName));
    }

    /// <summary>
    /// Update user avatar
    /// </summary>
    public void UpdateAvatar(string avatarUrl)
    {
        if (string.IsNullOrWhiteSpace(avatarUrl))
            throw new ArgumentException("Avatar URL cannot be empty", nameof(avatarUrl));

        AvatarUrl = avatarUrl;
        MarkAsUpdated();

        AddDomainEvent(new UserAvatarUpdatedDomainEvent(Id, avatarUrl));
    }

    /// <summary>
    /// Record user login
    /// </summary>
    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        MarkAsUpdated();

        AddDomainEvent(new UserLoggedInDomainEvent(Id, LastLoginAt.Value));
    }

    /// <summary>
    /// Verify user email
    /// </summary>
    public void VerifyEmail()
    {
        if (IsEmailVerified)
            return; // Already verified

        IsEmailVerified = true;
        MarkAsUpdated();
    }

    /// <summary>
    /// Change user role (admin only operation)
    /// </summary>
    public void ChangeRole(UserRole newRole)
    {
        if (Role == newRole)
            return;

        Role = newRole;
        MarkAsUpdated();
    }
}
