using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class QueryResultTests
{
    private static IChunk MakeChunk(string tag) => new StubChunk(FourCC.Parse(tag));

    private sealed record StubChunk(FourCC Tag) : IChunk
    {
        public long Offset => 0;
        public long Size => 0;
        public ReadOnlyMemory<byte> Raw => ReadOnlyMemory<byte>.Empty;
        public IReadOnlyList<IChunk> Children => [];
        public bool IsKnown => false;
    }

    [Fact]
    public void QueryResult_exposes_Chunks_TotalFilesSearched_and_TotalChunksSearched()
    {
        var chunk = MakeChunk("Lscr");
        var result = new QueryResult([chunk], TotalFilesSearched: 3, TotalChunksSearched: 150);

        Assert.Single(result.Chunks);
        Assert.Same(chunk, result.Chunks[0]);
        Assert.Equal(3, result.TotalFilesSearched);
        Assert.Equal(150, result.TotalChunksSearched);
    }

    [Fact]
    public void Empty_query_result_has_no_chunks_and_zero_counts()
    {
        var result = new QueryResult([], TotalFilesSearched: 0, TotalChunksSearched: 0);
        Assert.Empty(result.Chunks);
        Assert.Equal(0, result.TotalFilesSearched);
        Assert.Equal(0, result.TotalChunksSearched);
    }

    [Fact]
    public void QueryResult_preserves_order_of_matched_chunks()
    {
        var first = MakeChunk("Lscr");
        var second = MakeChunk("BITD");
        var result = new QueryResult([first, second], TotalFilesSearched: 1, TotalChunksSearched: 2);

        Assert.Equal(2, result.Chunks.Count);
        Assert.Same(first, result.Chunks[0]);
        Assert.Same(second, result.Chunks[1]);
    }
}
