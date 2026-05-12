namespace AtelierRex.Core;

public interface IFile
{
    string Path { get; }
    FileFormat Format { get; }
    IChunk Root { get; }
    IReadOnlyList<IChunk> Chunks { get; }
    FileMetadata Metadata { get; }
}
