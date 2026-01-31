namespace FrogEdu.Shared.Kernel.Exceptions;

public class ValidationException : DomainException
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public ValidationException(string field, string error)
        : base($"Validation failed for {field}: {error}")
    {
        Errors = new Dictionary<string, string[]> { { field, new[] { error } } };
    }
}
