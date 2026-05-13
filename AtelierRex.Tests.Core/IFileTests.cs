using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class IFileTests
{
    private static readonly FileFormat DirectorFormat =
        new("Director Movie", "Director", new Version(4, 0), Endianness.BigEndian);

    private sealed class StubChunk : IChunk
    {
        public FourCC Tag => FourCC.Parse("ROOT");
        public long Offset => 0;
        public long Size => 0;
        public ReadOnlyMemory<byte> Raw => ReadOnlyMemory<byte>.Empty;
        public IReadOnlyList<IChunk> Children => [];
        public bool IsKnown => false;
    }

    private sealed class StubFile : IFile
    {
        public string Path { get; init; } = string.Empty;
        public FileFormat Format { get; init; }
        public IChunk Root { get; init; } = new StubChunk();
        public IReadOnlyList<IChunk> Chunks { get; init; } = [];
        public FileMetadata Metadata { get; init; }
    }

    [Fact]
    public void Path_is_readable_as_string()
    {
        IFile file = new StubFile { Path = @"C:\HELLCAB\HCDATA\JHELL.MMM" };
        Assert.Equal(@"C:\HELLCAB\HCDATA\JHELL.MMM", file.Path);
    }

    [Fact]
    public void Format_is_readable_as_FileFormat()
    {
        IFile file = new StubFile { Format = DirectorFormat };
        Assert.Equal("Director Movie", file.Format.Name);
        Assert.Equal("Director", file.Format.Family);
    }

    [Fact]
    public void Root_is_readable_as_IChunk()
    {
        var root = new StubChunk();
        IFile file = new StubFile { Root = root };
        Assert.Same(root, file.Root);
    }

    [Fact]
    public void Chunks_is_a_flat_read_only_list_of_all_chunks()
    {
        var a = new StubChunk();
        var b = new StubChunk();
        IFile file = new StubFile { Chunks = [a, b] };
        Assert.Equal(2, file.Chunks.Count);
    }

    [Fact]
    public void Metadata_exposes_file_size_and_chunk_count()
    {
        var metadata = new FileMetadata(4096L, DateTime.UtcNow, "sha256abc", DirectorFormat, 7);
        IFile file = new StubFile { Metadata = metadata };
        Assert.Equal(4096L, file.Metadata.FileSize);
        Assert.Equal(7, file.Metadata.ChunkCount);
    }
}
