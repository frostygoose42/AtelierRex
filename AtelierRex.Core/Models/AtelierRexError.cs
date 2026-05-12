namespace AtelierRex.Core;

public readonly struct AtelierRexError
{
    public ErrorCode Code { get; }
    public string Message { get; }
    public string Context { get; }
    public Exception? InnerException { get; }

    public AtelierRexError(ErrorCode code, string message, string context = "", Exception? innerException = null)
    {
        Code = code;
        Message = message;
        Context = context;
        InnerException = innerException;
    }

    public override string ToString() => string.IsNullOrEmpty(Context)
        ? $"[{Code}] {Message}"
        : $"[{Code}] {Message} — {Context}";
}
