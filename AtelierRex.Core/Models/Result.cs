namespace AtelierRex.Core;

public readonly struct Result<T>
{
    private readonly T _value;
    private readonly AtelierRexError _error;

    public bool IsSuccess { get; }

    public T Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("Cannot access Value on a failed Result.");

    public AtelierRexError Error => !IsSuccess
        ? _error
        : throw new InvalidOperationException("Cannot access Error on a successful Result.");

    private Result(T value)
    {
        IsSuccess = true;
        _value = value;
        _error = default;
    }

    private Result(AtelierRexError error)
    {
        IsSuccess = false;
        _value = default!;
        _error = error;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(AtelierRexError error) => new(error);
}

public readonly struct Result
{
    private readonly AtelierRexError _error;

    public bool IsSuccess { get; }

    public AtelierRexError Error => !IsSuccess
        ? _error
        : throw new InvalidOperationException("Cannot access Error on a successful Result.");

    private Result(bool isSuccess, AtelierRexError error)
    {
        IsSuccess = isSuccess;
        _error = error;
    }

    public static Result Success() => new(true, default);
    public static Result Failure(AtelierRexError error) => new(false, error);
}
