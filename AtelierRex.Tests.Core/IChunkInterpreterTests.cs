using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class IChunkInterpreterTests
{
    private static readonly FileFormat DirectorFormat =
        new("Director Movie", "Director", new Version(4, 0), Endianness.BigEndian);

    private sealed class LscrChunk : IChunk
    {
        public FourCC Tag => FourCC.Parse("Lscr");
        public long Offset => 0;
        public long Size => 0;
        public ReadOnlyMemory<byte> Raw => ReadOnlyMemory<byte>.Empty;
        public IReadOnlyList<IChunk> Children => [];
        public bool IsKnown => true;
    }

    private sealed class StubInterpretedChunk : IInterpretedChunk
    {
        public required IChunk Source { get; init; }
        public required string Summary { get; init; }
    }

    private sealed class LingoInterpreter : IChunkInterpreter
    {
        public bool CanInterpret(FourCC tag, FileFormat format) =>
            tag == FourCC.Parse("Lscr") && format.Family == "Director";

        public Result<IInterpretedChunk> Interpret(IChunk chunk) =>
            Result<IInterpretedChunk>.Success(new StubInterpretedChunk
            {
                Source = chunk,
                Summary = "Lingo script"
            });
    }

    private sealed class UnrecognisedChunkInterpreter : IChunkInterpreter
    {
        public bool CanInterpret(FourCC tag, FileFormat format) => false;

        public Result<IInterpretedChunk> Interpret(IChunk chunk) =>
            Result<IInterpretedChunk>.Failure(
                new AtelierRexError(ErrorCode.UnknownChunkType, "cannot interpret chunk"));
    }

    [Fact]
    public void CanInterpret_returns_true_when_the_interpreter_supports_the_tag_and_format()
    {
        IChunkInterpreter interpreter = new LingoInterpreter();
        Assert.True(interpreter.CanInterpret(FourCC.Parse("Lscr"), DirectorFormat));
    }

    [Fact]
    public void CanInterpret_returns_false_when_the_tag_is_not_supported()
    {
        IChunkInterpreter interpreter = new LingoInterpreter();
        Assert.False(interpreter.CanInterpret(FourCC.Parse("BITD"), DirectorFormat));
    }

    [Fact]
    public void CanInterpret_returns_false_when_the_interpreter_does_not_support_any_chunk()
    {
        IChunkInterpreter interpreter = new UnrecognisedChunkInterpreter();
        Assert.False(interpreter.CanInterpret(FourCC.Parse("Lscr"), DirectorFormat));
    }

    [Fact]
    public void Interpret_returns_a_successful_Result_with_the_interpreted_chunk()
    {
        IChunkInterpreter interpreter = new LingoInterpreter();
        var chunk = new LscrChunk();
        var result = interpreter.Interpret(chunk);
        Assert.True(result.IsSuccess);
        Assert.Same(chunk, result.Value.Source);
        Assert.Equal("Lingo script", result.Value.Summary);
    }

    [Fact]
    public void Interpret_returns_a_failure_Result_with_UnknownChunkType_when_interpretation_fails()
    {
        IChunkInterpreter interpreter = new UnrecognisedChunkInterpreter();
        var result = interpreter.Interpret(new LscrChunk());
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.UnknownChunkType, result.Error.Code);
    }
}
