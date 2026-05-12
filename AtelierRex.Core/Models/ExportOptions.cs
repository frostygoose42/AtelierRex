namespace AtelierRex.Core;

public record ExportOptions
{
    public string OutputPath { get; init; } = string.Empty;
    public bool Overwrite { get; init; } = false;
    public CancellationToken CancellationToken { get; init; }
}
