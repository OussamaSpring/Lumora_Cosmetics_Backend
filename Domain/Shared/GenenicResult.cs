
namespace Domain.Shared;

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(T? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        Value = value;
    }
    public static Result<T> Success(T value) => new Result<T>(value, true, Error.None);
    public static Result<T?> Failure(Error error) => new(default(T), false, error);
}
