using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class IFormatDetectorTests
{
    private static readonly FileFormat DirectorFormat =
        new("Director Movie", "Director", new Version(4, 0), Endianness.BigEndian);

    // Recognises any header starting with RIFX
    private sealed class RifxDetector : IFormatDetector
    {
        private static readonly byte[] Signature = [(byte)'R', (byte)'I', (byte)'F', (byte)'X'];

        public bool CanDetect(ReadOnlySpan<byte> header) =>
            header.Length >= 4 && header[..4].SequenceEqual(Signature);

        public FileFormat Detect(ReadOnlySpan<byte> header) => DirectorFormat;
    }

    private static readonly byte[] RifxHeader = [(byte)'R', (byte)'I', (byte)'F', (byte)'X'];
    private static readonly byte[] UnknownHeader = [0x00, 0x00, 0x00, 0x00];

    [Fact]
    public void CanDetect_returns_true_when_the_detector_recognises_the_header()
    {
        IFormatDetector detector = new RifxDetector();
        Assert.True(detector.CanDetect(RifxHeader));
    }

    [Fact]
    public void CanDetect_returns_false_when_the_header_does_not_match()
    {
        IFormatDetector detector = new RifxDetector();
        Assert.False(detector.CanDetect(UnknownHeader));
    }

    [Fact]
    public void CanDetect_returns_false_for_a_header_shorter_than_4_bytes()
    {
        IFormatDetector detector = new RifxDetector();
        Assert.False(detector.CanDetect(new byte[] { (byte)'R', (byte)'I' }));
    }

    [Fact]
    public void Detect_returns_the_identified_FileFormat()
    {
        IFormatDetector detector = new RifxDetector();
        var format = detector.Detect(RifxHeader);
        Assert.Equal("Director Movie", format.Name);
        Assert.Equal("Director", format.Family);
    }
}
