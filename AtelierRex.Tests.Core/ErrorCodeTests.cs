using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class ErrorCodeTests
{
    [Fact]
    public void Exactly_14_error_codes_are_defined()
    {
        Assert.Equal(14, Enum.GetValues<ErrorCode>().Length);
    }

    [Theory]
    [InlineData(ErrorCode.UnknownFormat)]
    [InlineData(ErrorCode.MalformedHeader)]
    [InlineData(ErrorCode.UnexpectedEndOfFile)]
    [InlineData(ErrorCode.InvalidChunkSize)]
    [InlineData(ErrorCode.ChunkBoundaryViolation)]
    public void All_5_parser_error_codes_are_defined(ErrorCode code)
    {
        Assert.True(Enum.IsDefined(code));
    }

    [Theory]
    [InlineData(ErrorCode.UnknownChunkType)]
    [InlineData(ErrorCode.IncompatibleVersion)]
    [InlineData(ErrorCode.CorruptPayload)]
    public void All_3_interpreter_error_codes_are_defined(ErrorCode code)
    {
        Assert.True(Enum.IsDefined(code));
    }

    [Theory]
    [InlineData(ErrorCode.FileNotFound)]
    [InlineData(ErrorCode.AccessDenied)]
    [InlineData(ErrorCode.OperationCancelled)]
    public void All_3_operation_error_codes_are_defined(ErrorCode code)
    {
        Assert.True(Enum.IsDefined(code));
    }

    [Theory]
    [InlineData(ErrorCode.PluginLoadFailure)]
    [InlineData(ErrorCode.CapabilityViolation)]
    [InlineData(ErrorCode.PluginSecurityViolation)]
    public void All_3_plugin_error_codes_are_defined(ErrorCode code)
    {
        Assert.True(Enum.IsDefined(code));
    }
}
