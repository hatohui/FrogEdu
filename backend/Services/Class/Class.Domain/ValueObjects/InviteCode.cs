using System.Text.RegularExpressions;
using FrogEdu.Shared.Kernel;

namespace FrogEdu.Class.Domain.ValueObjects;

/// <summary>
/// Value Object representing a 6-digit invite code for classroom enrollment
/// </summary>
public sealed partial class InviteCode : ValueObject
{
    public string Value { get; private set; }

    private InviteCode(string value) => Value = value;

    public static InviteCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Invite code cannot be empty", nameof(value));

        var normalized = value.Trim().ToUpperInvariant();

        if (!InviteCodeRegex().IsMatch(normalized))
            throw new ArgumentException(
                "Invite code must be exactly 6 alphanumeric characters",
                nameof(value)
            );

        return new InviteCode(normalized);
    }

    public static InviteCode Generate()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; // Exclude confusing characters: 0, O, 1, I
        var random = new Random();
        var code = new string(
            Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray()
        );

        return new InviteCode(code);
    }

    [GeneratedRegex(@"^[A-Z0-9]{6}$", RegexOptions.Compiled)]
    private static partial Regex InviteCodeRegex();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
