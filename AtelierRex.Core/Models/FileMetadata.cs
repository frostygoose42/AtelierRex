namespace AtelierRex.Core;

public readonly struct FileMetadata
{
    public long FileSize { get; }
    public DateTime LastModified { get; }
    public string Hash { get; }
    public FileFormat Format { get; }
    public int ChunkCount { get; }

    public FileMetadata(long fileSize, DateTime lastModified, string hash, FileFormat format, int chunkCount)
    {
        FileSize = fileSize;
        LastModified = lastModified;
        Hash = hash;
        Format = format;
        ChunkCount = chunkCount;
    }
}
