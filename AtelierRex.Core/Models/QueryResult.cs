namespace AtelierRex.Core;

public record QueryResult(
    IReadOnlyList<IChunk> Chunks,
    int TotalFilesSearched,
    int TotalChunksSearched);
