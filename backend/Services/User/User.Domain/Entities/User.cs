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
    public string? EmailVerificationToken { get; private set; }
    public DateTime? EmailVerificationTokenExpiry { get; private set; }
    public string? PasswordResetToken { get; private set; }
    public DateTime? PasswordResetTokenExpiry { get; private set; }

    private User() { }

    private User(
        CognitoUserId cognitoId,
        Email email,
        string firstName,
        string lastName,
        Guid roleId,
        string? avatarUrl = null
    )
        : base()
    {
        CognitoId = cognitoId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        RoleId = roleId;
        AvatarUrl = avatarUrl;
        IsEmailVerified = false;
    }

    public static User Create(
        string cognitoId,
        string email,
        string firstName,
        string lastName,
        Guid roleId,
        string? avatarUrl = null
    )
    {
        var user = new User(
            CognitoUserId.Create(cognitoId),
            Email.Create(email),
            firstName,
            lastName,
            roleId,
            avatarUrl
        );

        user.Id = Guid.NewGuid();

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
        EmailVerificationToken = null;
        EmailVerificationTokenExpiry = null;
        MarkAsUpdated();
    }

    public void GenerateEmailVerificationToken()
    {
        var bytes = System.Security.Cryptography.RandomNumberGenerator.GetBytes(32);
        EmailVerificationToken = Convert
            .ToBase64String(bytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");
        EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24);
        MarkAsUpdated();
    }

    public bool IsEmailVerificationTokenValid(string token)
    {
        if (string.IsNullOrWhiteSpace(EmailVerificationToken))
            return false;

        if (EmailVerificationTokenExpiry == null || EmailVerificationTokenExpiry < DateTime.UtcNow)
            return false;

        return EmailVerificationToken == token;
    }

    public void GeneratePasswordResetToken()
    {
        var bytes = System.Security.Cryptography.RandomNumberGenerator.GetBytes(32);
        PasswordResetToken = Convert
            .ToBase64String(bytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");
        PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);
        MarkAsUpdated();
    }

    public bool IsPasswordResetTokenValid(string token)
    {
        if (string.IsNullOrWhiteSpace(PasswordResetToken))
            return false;

        if (PasswordResetTokenExpiry == null || PasswordResetTokenExpiry < DateTime.UtcNow)
            return false;

        return PasswordResetToken == token;
    }

    public void ClearPasswordResetToken()
    {
        PasswordResetToken = null;
        PasswordResetTokenExpiry = null;
        MarkAsUpdated();
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
