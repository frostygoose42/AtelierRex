using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class IFileParserTests
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
        public string Path => "test.mmm";
        public FileFormat Format => DirectorFormat;
        public IChunk Root => new StubChunk();
        public IReadOnlyList<IChunk> Chunks => [];
        public FileMetadata Metadata => default;
    }

    private sealed class DirectorParser : IFileParser
    {
        public bool CanParse(FileFormat format) => format.Family == "Director";

        public Result<IFile> Parse(Stream stream, ParseOptions options) =>
            Result<IFile>.Success(new StubFile());
    }

    private sealed class RejectionParser : IFileParser
    {
        public bool CanParse(FileFormat format) => false;

        public Result<IFile> Parse(Stream stream, ParseOptions options) =>
            Result<IFile>.Failure(new AtelierRexError(ErrorCode.UnknownFormat, "unsupported format"));
    }

    [Fact]
    public void CanParse_returns_true_when_the_parser_supports_the_format()
    {
        IFileParser parser = new DirectorParser();
        Assert.True(parser.CanParse(DirectorFormat));
    }

    [Fact]
    public void CanParse_returns_false_when_the_parser_does_not_support_the_format()
    {
        IFileParser parser = new RejectionParser();
        Assert.False(parser.CanParse(DirectorFormat));
    }

    [Fact]
    public void Parse_returns_a_successful_Result_containing_an_IFile()
    {
        IFileParser parser = new DirectorParser();
        using var stream = new MemoryStream();
        var result = parser.Parse(stream, new ParseOptions());
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void Parse_returns_a_failure_Result_with_UnknownFormat_when_the_format_is_not_supported()
    {
        IFileParser parser = new RejectionParser();
        using var stream = new MemoryStream();
        var result = parser.Parse(stream, new ParseOptions());
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.UnknownFormat, result.Error.Code);
    }

    [Fact]
    public void Parse_receives_the_ParseOptions_passed_by_the_caller()
    {
        var observedOptions = (ParseOptions?)null;
        var spyParser = new SpyParser(o => observedOptions = o);
        var expected = new ParseOptions { MaxDepth = 16, IncludeRawBytes = false };

        using var stream = new MemoryStream();
        spyParser.Parse(stream, expected);

        Assert.Equal(expected, observedOptions);
    }

    private sealed class SpyParser : IFileParser
    {
        private readonly Action<ParseOptions> _onParse;
        public SpyParser(Action<ParseOptions> onParse) => _onParse = onParse;
        public bool CanParse(FileFormat format) => true;
        public Result<IFile> Parse(Stream stream, ParseOptions options)
        {
            _onParse(options);
            return Result<IFile>.Success(new StubFile());
        }
    }
}
