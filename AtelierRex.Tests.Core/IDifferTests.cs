using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class IDifferTests
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

    private sealed class StubFile : IFile
    {
        public required string Path { get; init; }
        public FileFormat Format => default;
        public IChunk Root { get; init; } = new StubChunk { Tag = FourCC.Parse("ROOT") };
        public IReadOnlyList<IChunk> Chunks { get; init; } = [];
        public FileMetadata Metadata => default;
    }

    // Structural differ: compares tags only, ignores payload
    private sealed class StructuralDiffer : IDiffer
    {
        public DiffResult Diff(IFile left, IFile right, DiffOptions options)
        {
            var leftChunks = left.Chunks;
            var rightChunks = right.Chunks;
            var nodes = new List<DiffNode>();

            int max = Math.Max(leftChunks.Count, rightChunks.Count);
            for (int i = 0; i < max; i++)
            {
                var l = i < leftChunks.Count ? leftChunks[i] : null;
                var r = i < rightChunks.Count ? rightChunks[i] : null;

                DiffNodeType type = (l, r) switch
                {
                    (null, _) => DiffNodeType.Added,
                    (_, null) => DiffNodeType.Removed,
                    _ when l.Tag == r.Tag => DiffNodeType.Identical,
                    _ => DiffNodeType.Modified,
                };

                nodes.Add(new DiffNode(type, l, r, $"/{i}"));
            }

            int changed = nodes.Count(n => n.Type != DiffNodeType.Identical);
            return new DiffResult(nodes, TotalChunks: max, ChangedChunks: changed);
        }

        public DiffResult Diff(IChunk left, IChunk right, DiffOptions options)
        {
            var type = left.Tag == right.Tag ? DiffNodeType.Identical : DiffNodeType.Modified;
            var node = new DiffNode(type, left, right, "/");
            int changed = type == DiffNodeType.Identical ? 0 : 1;
            return new DiffResult([node], TotalChunks: 1, ChangedChunks: changed);
        }
    }

    private static StubChunk Chunk(string tag) => new() { Tag = FourCC.Parse(tag) };

    [Fact]
    public void Diff_two_files_with_identical_chunk_lists_reports_no_changes()
    {
        IDiffer differ = new StructuralDiffer();
        var chunk = Chunk("mmap");
        var left = new StubFile { Path = "a.mmm", Chunks = [chunk] };
        var right = new StubFile { Path = "b.mmm", Chunks = [Chunk("mmap")] };

        var result = differ.Diff(left, right, new DiffOptions());

        Assert.False(result.HasChanges);
    }

    [Fact]
    public void Diff_two_files_where_a_chunk_was_added_reports_one_change()
    {
        IDiffer differ = new StructuralDiffer();
        var left = new StubFile { Path = "a.mmm", Chunks = [] };
        var right = new StubFile { Path = "b.mmm", Chunks = [Chunk("Lscr")] };

        var result = differ.Diff(left, right, new DiffOptions());

        Assert.True(result.HasChanges);
        Assert.Equal(DiffNodeType.Added, result.Nodes[0].Type);
    }

    [Fact]
    public void Diff_two_files_where_a_chunk_was_removed_reports_one_change()
    {
        IDiffer differ = new StructuralDiffer();
        var left = new StubFile { Path = "a.mmm", Chunks = [Chunk("Lscr")] };
        var right = new StubFile { Path = "b.mmm", Chunks = [] };

        var result = differ.Diff(left, right, new DiffOptions());

        Assert.True(result.HasChanges);
        Assert.Equal(DiffNodeType.Removed, result.Nodes[0].Type);
    }

    [Fact]
    public void Diff_two_identical_chunks_returns_an_Identical_node()
    {
        IDiffer differ = new StructuralDiffer();
        var result = differ.Diff(Chunk("mmap"), Chunk("mmap"), new DiffOptions());

        Assert.False(result.HasChanges);
        Assert.Equal(DiffNodeType.Identical, result.Nodes[0].Type);
    }

    [Fact]
    public void Diff_two_chunks_with_different_tags_returns_a_Modified_node()
    {
        IDiffer differ = new StructuralDiffer();
        var result = differ.Diff(Chunk("mmap"), Chunk("Lscr"), new DiffOptions());

        Assert.True(result.HasChanges);
        Assert.Equal(DiffNodeType.Modified, result.Nodes[0].Type);
    }

    [Fact]
    public void Diff_chunk_overload_populates_Left_and_Right_on_the_result_node()
    {
        IDiffer differ = new StructuralDiffer();
        var left = Chunk("mmap");
        var right = Chunk("Lscr");
        var result = differ.Diff(left, right, new DiffOptions());

        Assert.Same(left, result.Nodes[0].Left);
        Assert.Same(right, result.Nodes[0].Right);
    }
}
