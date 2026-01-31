namespace FrogEdu.Shared.Kernel;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }

    protected Result(bool isSuccess, string error)
    {
        if (isSuccess && !string.IsNullOrEmpty(error))
            throw new InvalidOperationException("Success result cannot have an error message");

        if (!isSuccess && string.IsNullOrEmpty(error))
            throw new InvalidOperationException("Failure result must have an error message");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, string.Empty);

    public static Result Failure(string error) => new(false, error);
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, string error)
        : base(isSuccess, error)
    {
        if (isSuccess && value == null)
            throw new InvalidOperationException("Success result must have a value");

        Value = value;
    }

    public static Result<T> Success(T value) => new(true, value, string.Empty);

    public static new Result<T> Failure(string error) => new(false, default, error);
}
