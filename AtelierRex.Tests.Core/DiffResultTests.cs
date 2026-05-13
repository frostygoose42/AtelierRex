using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class DiffResultTests
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

    // ── DiffNodeType ───────────────────────────────────────────────────────

    [Theory]
    [InlineData(DiffNodeType.Identical)]
    [InlineData(DiffNodeType.Modified)]
    [InlineData(DiffNodeType.Added)]
    [InlineData(DiffNodeType.Removed)]
    [InlineData(DiffNodeType.Moved)]
    [InlineData(DiffNodeType.TypeChanged)]
    public void All_6_DiffNodeType_values_are_defined(DiffNodeType type)
    {
        Assert.True(Enum.IsDefined(type));
    }

    // ── DiffNode ───────────────────────────────────────────────────────────

    [Fact]
    public void DiffNode_exposes_Type_Left_Right_and_Path()
    {
        var left = MakeChunk("mmap");
        var right = MakeChunk("mmap");
        var node = new DiffNode(DiffNodeType.Identical, left, right, "/root/mmap");

        Assert.Equal(DiffNodeType.Identical, node.Type);
        Assert.Same(left, node.Left);
        Assert.Same(right, node.Right);
        Assert.Equal("/root/mmap", node.Path);
    }

    [Fact]
    public void DiffNode_Left_and_Right_can_be_null_for_Added_and_Removed_nodes()
    {
        var added = new DiffNode(DiffNodeType.Added, null, MakeChunk("Lscr"), "/new");
        var removed = new DiffNode(DiffNodeType.Removed, MakeChunk("Lscr"), null, "/old");

        Assert.Null(added.Left);
        Assert.Null(removed.Right);
    }

    // ── DiffResult ─────────────────────────────────────────────────────────

    [Fact]
    public void HasChanges_returns_false_when_no_chunks_changed()
    {
        var result = new DiffResult([], TotalChunks: 10, ChangedChunks: 0);
        Assert.False(result.HasChanges);
    }

    [Fact]
    public void HasChanges_returns_true_when_at_least_one_chunk_changed()
    {
        var result = new DiffResult([], TotalChunks: 10, ChangedChunks: 1);
        Assert.True(result.HasChanges);
    }

    [Fact]
    public void DiffResult_exposes_Nodes_TotalChunks_and_ChangedChunks()
    {
        var node = new DiffNode(DiffNodeType.Added, null, MakeChunk("Lscr"), "/new");
        var result = new DiffResult([node], TotalChunks: 5, ChangedChunks: 1);

        Assert.Single(result.Nodes);
        Assert.Same(node, result.Nodes[0]);
        Assert.Equal(5, result.TotalChunks);
        Assert.Equal(1, result.ChangedChunks);
    }

    [Fact]
    public void DiffResult_with_zero_total_chunks_and_zero_changed_chunks_has_no_changes()
    {
        var result = new DiffResult([], TotalChunks: 0, ChangedChunks: 0);
        Assert.False(result.HasChanges);
        Assert.Empty(result.Nodes);
    }
}
