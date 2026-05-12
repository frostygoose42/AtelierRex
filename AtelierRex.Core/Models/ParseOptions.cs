namespace AtelierRex.Core;

public record ParseOptions
{
    public bool GracefulDegradation { get; init; } = true;
    public int MaxDepth { get; init; } = 64;
    public long MaxFileSize { get; init; } = 1024L * 1024 * 512; // 512 MB
    public bool IncludeRawBytes { get; init; } = true;
    public bool InterpretOnParse { get; init; } = false;
    public CancellationToken CancellationToken { get; init; }
}
