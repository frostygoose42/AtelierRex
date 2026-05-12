namespace AtelierRex.Core;

public enum DiffNodeType
{
    Identical,
    Modified,
    Added,
    Removed,
    Moved,
    TypeChanged,
}

public record DiffNode(DiffNodeType Type, IChunk? Left, IChunk? Right, string Path);

public record DiffResult(IReadOnlyList<DiffNode> Nodes, int TotalChunks, int ChangedChunks)
{
    public bool HasChanges => ChangedChunks > 0;
}
