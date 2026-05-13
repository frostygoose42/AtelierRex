using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class IQueryEngineTests
{
    private sealed class AnyTagQuery : IQuery
    {
        public FourCC Tag { get; }
        public AnyTagQuery(FourCC tag) => Tag = tag;
    }

    private sealed class StubChunk : IChunk
    {
        public required FourCC Tag { get; init; }
        public long Offset => 0;
        public long Size => 0;
        public ReadOnlyMemory<byte> Raw => ReadOnlyMemory<byte>.Empty;
        public IReadOnlyList<IChunk> Children => [];
        public bool IsKnown => false;
    }

    private sealed class StubFile : IFile
    {
        public string Path => "test.mmm";
        public FileFormat Format => default;
        public IChunk Root => new StubChunk { Tag = FourCC.Parse("ROOT") };
        public IReadOnlyList<IChunk> Chunks { get; init; } = [];
        public FileMetadata Metadata => default;
    }

    // Returns every chunk from every file that matches the query tag
    private sealed class TagMatchingQueryEngine : IQueryEngine
    {
        public QueryResult Query(IQuery query, IEnumerable<IFile> files)
        {
            var fileList = files.ToList();
            var tag = query is AnyTagQuery q ? q.Tag : default;
            var matched = fileList.SelectMany(f => f.Chunks)
                                  .Where(c => c.Tag == tag)
                                  .ToList();
            int totalChunks = fileList.SelectMany(f => f.Chunks).Count();
            return new QueryResult(matched, TotalFilesSearched: fileList.Count, TotalChunksSearched: totalChunks);
        }
    }

    [Fact]
    public void Query_against_no_files_returns_an_empty_result()
    {
        IQueryEngine engine = new TagMatchingQueryEngine();
        var result = engine.Query(new AnyTagQuery(FourCC.Parse("Lscr")), []);

        Assert.Empty(result.Chunks);
        Assert.Equal(0, result.TotalFilesSearched);
        Assert.Equal(0, result.TotalChunksSearched);
    }

    [Fact]
    public void Query_returns_only_chunks_matching_the_requested_tag()
    {
        var lscr = new StubChunk { Tag = FourCC.Parse("Lscr") };
        var bitd = new StubChunk { Tag = FourCC.Parse("BITD") };
        var file = new StubFile { Chunks = [lscr, bitd] };

        IQueryEngine engine = new TagMatchingQueryEngine();
        var result = engine.Query(new AnyTagQuery(FourCC.Parse("Lscr")), [file]);

        Assert.Single(result.Chunks);
        Assert.Same(lscr, result.Chunks[0]);
    }

    [Fact]
    public void Query_reports_total_files_searched_regardless_of_match_count()
    {
        var file1 = new StubFile { Chunks = [new StubChunk { Tag = FourCC.Parse("BITD") }] };
        var file2 = new StubFile { Chunks = [new StubChunk { Tag = FourCC.Parse("BITD") }] };

        IQueryEngine engine = new TagMatchingQueryEngine();
        var result = engine.Query(new AnyTagQuery(FourCC.Parse("Lscr")), [file1, file2]);

        Assert.Equal(2, result.TotalFilesSearched);
        Assert.Empty(result.Chunks);
    }

    [Fact]
    public void Query_reports_total_chunks_searched_across_all_files()
    {
        var file = new StubFile
        {
            Chunks = [
                new StubChunk { Tag = FourCC.Parse("Lscr") },
                new StubChunk { Tag = FourCC.Parse("BITD") },
                new StubChunk { Tag = FourCC.Parse("mmap") },
            ]
        };

        IQueryEngine engine = new TagMatchingQueryEngine();
        var result = engine.Query(new AnyTagQuery(FourCC.Parse("Lscr")), [file]);

        Assert.Equal(3, result.TotalChunksSearched);
    }
}
