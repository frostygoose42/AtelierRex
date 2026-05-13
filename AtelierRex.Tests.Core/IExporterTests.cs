using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class IExporterTests
{
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
        public FileFormat Format => default;
        public IChunk Root => new StubChunk();
        public IReadOnlyList<IChunk> Chunks => [];
        public FileMetadata Metadata => default;
    }

    private sealed class JsonExporter : IExporter
    {
        public string TargetFormat => "json";

        public Result Export(IFile file, ExportOptions options) => Result.Success();
    }

    private sealed class BrokenExporter : IExporter
    {
        public string TargetFormat => "broken";

        public Result Export(IFile file, ExportOptions options) =>
            Result.Failure(new AtelierRexError(ErrorCode.AccessDenied, "cannot write to output path"));
    }

    [Fact]
    public void TargetFormat_identifies_the_export_format_as_a_string()
    {
        IExporter exporter = new JsonExporter();
        Assert.Equal("json", exporter.TargetFormat);
    }

    [Fact]
    public void Export_returns_a_successful_Result_when_export_succeeds()
    {
        IExporter exporter = new JsonExporter();
        var result = exporter.Export(new StubFile(), new ExportOptions { OutputPath = "/out/data.json" });
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Export_returns_a_failure_Result_with_an_error_when_export_fails()
    {
        IExporter exporter = new BrokenExporter();
        var result = exporter.Export(new StubFile(), new ExportOptions());
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.AccessDenied, result.Error.Code);
    }

    [Fact]
    public void Export_receives_the_ExportOptions_passed_by_the_caller()
    {
        var observedOptions = (ExportOptions?)null;
        var spy = new SpyExporter(o => observedOptions = o);
        var expected = new ExportOptions { OutputPath = "/tmp/out", Overwrite = true };

        spy.Export(new StubFile(), expected);

        Assert.Equal(expected, observedOptions);
    }

    private sealed class SpyExporter : IExporter
    {
        private readonly Action<ExportOptions> _onExport;
        public SpyExporter(Action<ExportOptions> onExport) => _onExport = onExport;
        public string TargetFormat => "spy";
        public Result Export(IFile file, ExportOptions options)
        {
            _onExport(options);
            return Result.Success();
        }
    }
}
