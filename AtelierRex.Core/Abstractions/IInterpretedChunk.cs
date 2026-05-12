namespace AtelierRex.Core;

public interface IInterpretedChunk
{
    IChunk Source { get; }
    string Summary { get; }
}
