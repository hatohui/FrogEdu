using FrogEdu.Shared.Kernel;

namespace FrogEdu.User.Domain.ValueObjects;

/// <summary>
/// User's full name value object
/// </summary>
public sealed class FullName : ValueObject
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullNameValue => $"{FirstName} {LastName}";

    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    /// <summary>
    /// Create a full name
    /// </summary>
    public static FullName Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        return new FullName(firstName.Trim(), lastName.Trim());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }

    public override string ToString() => FullNameValue;
}
