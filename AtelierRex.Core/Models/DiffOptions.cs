namespace AtelierRex.Core;

public enum DiffStrategy { Structural, Content, Semantic }

public record DiffOptions
{
    public DiffStrategy Strategy { get; init; } = DiffStrategy.Structural;
}
