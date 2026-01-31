using FrogEdu.Shared.Kernel;

namespace FrogEdu.User.Domain.ValueObjects;

public sealed class CognitoUserId : ValueObject
{
    public string Value { get; private set; }

    private CognitoUserId(string value) => Value = value;

    public static CognitoUserId Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Cognito user ID cannot be empty", nameof(value));

        if (!Guid.TryParse(value, out _))
            throw new ArgumentException(
                "Invalid Cognito user ID format. Must be a valid GUID.",
                nameof(value)
            );

        return new CognitoUserId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
