using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class IInterpretedChunkTests
{
    private sealed class LscrChunk : IChunk
    {
        public FourCC Tag => FourCC.Parse("Lscr");
        public long Offset => 128L;
        public long Size => 512L;
        public ReadOnlyMemory<byte> Raw => ReadOnlyMemory<byte>.Empty;
        public IReadOnlyList<IChunk> Children => [];
        public bool IsKnown => true;
    }

    private sealed class StubInterpretedChunk : IInterpretedChunk
    {
        public required IChunk Source { get; init; }
        public required string Summary { get; init; }
    }

    [Fact]
    public void Source_is_the_raw_IChunk_that_was_interpreted()
    {
        var raw = new LscrChunk();
        IInterpretedChunk interpreted = new StubInterpretedChunk
        {
            Source = raw,
            Summary = "Lingo script — 12 handlers"
        };

        Assert.Same(raw, interpreted.Source);
    }

    [Fact]
    public void Summary_is_accessible_as_a_human_readable_string()
    {
        IInterpretedChunk interpreted = new StubInterpretedChunk
        {
            Source = new LscrChunk(),
            Summary = "Lingo script — 12 handlers"
        };

        Assert.Equal("Lingo script — 12 handlers", interpreted.Summary);
    }

    [Fact]
    public void Source_chunk_properties_are_accessible_through_the_interpreted_chunk()
    {
        var raw = new LscrChunk();
        IInterpretedChunk interpreted = new StubInterpretedChunk
        {
            Source = raw,
            Summary = "Lingo script"
        };

        Assert.Equal("Lscr", interpreted.Source.Tag.Value);
        Assert.Equal(128L, interpreted.Source.Offset);
        Assert.Equal(512L, interpreted.Source.Size);
    }
}
