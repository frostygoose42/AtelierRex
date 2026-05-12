namespace AtelierRex.Core;

public interface IChunk
{
    FourCC Tag { get; }
    long Offset { get; }
    long Size { get; }
    ReadOnlyMemory<byte> Raw { get; }
    IReadOnlyList<IChunk> Children { get; }
    bool IsKnown { get; }
}
