namespace AtelierRex.Core;

public interface IFormatDetector
{
    bool CanDetect(ReadOnlySpan<byte> header);
    FileFormat Detect(ReadOnlySpan<byte> header);
}
