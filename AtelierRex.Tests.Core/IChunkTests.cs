using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class IChunkTests
{
    private sealed class StubChunk : IChunk
    {
        public required FourCC Tag { get; init; }
        public long Offset { get; init; }
        public long Size { get; init; }
        public ReadOnlyMemory<byte> Raw { get; init; }
        public IReadOnlyList<IChunk> Children { get; init; } = [];
        public bool IsKnown { get; init; }
    }

    [Fact]
    public void Tag_is_readable_as_FourCC()
    {
        IChunk chunk = new StubChunk { Tag = FourCC.Parse("RIFX") };
        Assert.Equal("RIFX", chunk.Tag.Value);
    }

    [Fact]
    public void Offset_is_readable_as_long()
    {
        IChunk chunk = new StubChunk { Tag = FourCC.Parse("mmap"), Offset = 8L };
        Assert.Equal(8L, chunk.Offset);
    }

    [Fact]
    public void Size_is_readable_as_long()
    {
        IChunk chunk = new StubChunk { Tag = FourCC.Parse("mmap"), Size = 1024L };
        Assert.Equal(1024L, chunk.Size);
    }

    [Fact]
    public void Raw_is_readable_as_ReadOnlyMemory_of_byte()
    {
        var bytes = new byte[] { 0x52, 0x49, 0x46, 0x58 };
        IChunk chunk = new StubChunk { Tag = FourCC.Parse("mmap"), Raw = bytes };
        Assert.Equal(bytes, chunk.Raw.ToArray());
    }

    [Fact]
    public void Children_returns_an_empty_list_for_a_leaf_chunk()
    {
        IChunk chunk = new StubChunk { Tag = FourCC.Parse("BITD") };
        Assert.Empty(chunk.Children);
    }

    [Fact]
    public void Children_returns_nested_chunks_for_a_container_chunk()
    {
        var child = new StubChunk { Tag = FourCC.Parse("mmap") };
        IChunk chunk = new StubChunk
        {
            Tag = FourCC.Parse("RIFX"),
            Children = [child]
        };

        Assert.Single(chunk.Children);
        Assert.Equal("mmap", chunk.Children[0].Tag.Value);
    }

    [Fact]
    public void IsKnown_is_false_for_an_unrecognised_chunk()
    {
        IChunk chunk = new StubChunk { Tag = FourCC.Parse("????"), IsKnown = false };
        Assert.False(chunk.IsKnown);
    }

    [Fact]
    public void IsKnown_is_true_for_a_recognised_chunk()
    {
        IChunk chunk = new StubChunk { Tag = FourCC.Parse("Lscr"), IsKnown = true };
        Assert.True(chunk.IsKnown);
    }
}
