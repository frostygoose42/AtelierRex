using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class AtelierRexErrorTests
{
    [Fact]
    public void Constructor_sets_code_message_context_and_inner_exception()
    {
        var inner = new Exception("inner");
        var error = new AtelierRexError(ErrorCode.MalformedHeader, "bad header", "offset 0", inner);

        Assert.Equal(ErrorCode.MalformedHeader, error.Code);
        Assert.Equal("bad header", error.Message);
        Assert.Equal("offset 0", error.Context);
        Assert.Same(inner, error.InnerException);
    }

    [Fact]
    public void Context_defaults_to_empty_string_when_omitted()
    {
        var error = new AtelierRexError(ErrorCode.UnknownFormat, "unknown");
        Assert.Equal(string.Empty, error.Context);
    }

    [Fact]
    public void InnerException_defaults_to_null_when_omitted()
    {
        var error = new AtelierRexError(ErrorCode.UnknownFormat, "unknown");
        Assert.Null(error.InnerException);
    }

    [Fact]
    public void ToString_without_context_includes_code_and_message_but_no_separator()
    {
        var error = new AtelierRexError(ErrorCode.FileNotFound, "file not found");
        var str = error.ToString();
        Assert.Contains("FileNotFound", str);
        Assert.Contains("file not found", str);
        Assert.DoesNotContain("—", str);
    }

    [Fact]
    public void ToString_with_context_includes_code_message_and_context()
    {
        var error = new AtelierRexError(ErrorCode.FileNotFound, "file not found", "JHELL.MMM");
        var str = error.ToString();
        Assert.Contains("FileNotFound", str);
        Assert.Contains("file not found", str);
        Assert.Contains("JHELL.MMM", str);
    }
}
