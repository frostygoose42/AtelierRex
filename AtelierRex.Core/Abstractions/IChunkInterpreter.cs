namespace AtelierRex.Core;

public interface IChunkInterpreter
{
    bool CanInterpret(FourCC tag, FileFormat format);
    Result<IInterpretedChunk> Interpret(IChunk chunk);
}
