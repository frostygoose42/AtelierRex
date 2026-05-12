namespace AtelierRex.Core;

public interface IFileParser
{
    bool CanParse(FileFormat format);
    Result<IFile> Parse(Stream stream, ParseOptions options);
}
