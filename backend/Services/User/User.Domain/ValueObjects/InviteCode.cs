using FrogEdu.Shared.Kernel;

namespace FrogEdu.User.Domain.ValueObjects;

/// <summary>
/// Represents a 6-character alphanumeric invite code for class enrollment
/// </summary>
public sealed class InviteCode : ValueObject
{
    private const int CodeLength = 6;
    private const string AllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public string Value { get; }
    public DateTime ExpiresAt { get; }

    private InviteCode(string value, DateTime expiresAt)
    {
        Value = value;
        ExpiresAt = expiresAt;
    }

    /// <summary>
    /// Generate a new random invite code
    /// </summary>
    /// <param name="expiresInDays">Number of days until the code expires (default: 7)</param>
    public static InviteCode Generate(int expiresInDays = 7)
    {
        var random = new Random();
        var code = new char[CodeLength];

        for (int i = 0; i < CodeLength; i++)
        {
            code[i] = AllowedCharacters[random.Next(AllowedCharacters.Length)];
        }

        return new InviteCode(new string(code), DateTime.UtcNow.AddDays(expiresInDays));
    }

    /// <summary>
    /// Create an invite code from an existing value (for hydration from database)
    /// </summary>
    public static InviteCode Create(string value, DateTime expiresAt)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Invite code cannot be empty", nameof(value));

        if (value.Length != CodeLength)
            throw new ArgumentException(
                $"Invite code must be exactly {CodeLength} characters",
                nameof(value)
            );

        if (!value.All(c => AllowedCharacters.Contains(c)))
            throw new ArgumentException("Invite code contains invalid characters", nameof(value));

        return new InviteCode(value.ToUpperInvariant(), expiresAt);
    }

    /// <summary>
    /// Check if the invite code has expired
    /// </summary>
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;

    /// <summary>
    /// Validate the code format
    /// </summary>
    public static bool IsValidFormat(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length != CodeLength)
            return false;

        return code.ToUpperInvariant().All(c => AllowedCharacters.Contains(c));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
