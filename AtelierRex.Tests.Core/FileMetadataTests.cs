using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class FileMetadataTests
{
    private static readonly FileFormat DirectorFormat =
        new("Director Movie", "Director", new Version(4, 0), Endianness.BigEndian);

    [Fact]
    public void Constructor_sets_all_properties()
    {
        var now = DateTime.UtcNow;
        var metadata = new FileMetadata(1024L, now, "abc123", DirectorFormat, 42);

        Assert.Equal(1024L, metadata.FileSize);
        Assert.Equal(now, metadata.LastModified);
        Assert.Equal("abc123", metadata.Hash);
        Assert.Equal(DirectorFormat, metadata.Format);
        Assert.Equal(42, metadata.ChunkCount);
    }

    [Fact]
    public void FileSize_can_represent_large_files()
    {
        var metadata = new FileMetadata(1024L * 1024 * 512, DateTime.UtcNow, "", default, 0);
        Assert.Equal(536870912L, metadata.FileSize);
    }

    [Fact]
    public void ChunkCount_can_be_zero_for_an_empty_file()
    {
        var metadata = new FileMetadata(0L, DateTime.UtcNow, "", default, 0);
        Assert.Equal(0, metadata.ChunkCount);
    }
}
