using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class ParseOptionsTests
{
    [Fact]
    public void Default_GracefulDegradation_is_true()
    {
        Assert.True(new ParseOptions().GracefulDegradation);
    }

    [Fact]
    public void Default_MaxDepth_is_64()
    {
        Assert.Equal(64, new ParseOptions().MaxDepth);
    }

    [Fact]
    public void Default_MaxFileSize_is_512_megabytes()
    {
        Assert.Equal(1024L * 1024 * 512, new ParseOptions().MaxFileSize);
    }

    [Fact]
    public void Default_IncludeRawBytes_is_true()
    {
        Assert.True(new ParseOptions().IncludeRawBytes);
    }

    [Fact]
    public void Default_InterpretOnParse_is_false()
    {
        Assert.False(new ParseOptions().InterpretOnParse);
    }

    [Fact]
    public void Default_CancellationToken_is_the_default_token()
    {
        Assert.Equal(default, new ParseOptions().CancellationToken);
    }

    [Fact]
    public void All_properties_can_be_overridden_with_init_syntax()
    {
        var cts = new CancellationTokenSource();
        var options = new ParseOptions
        {
            GracefulDegradation = false,
            MaxDepth = 10,
            MaxFileSize = 1024L,
            IncludeRawBytes = false,
            InterpretOnParse = true,
            CancellationToken = cts.Token,
        };

        Assert.False(options.GracefulDegradation);
        Assert.Equal(10, options.MaxDepth);
        Assert.Equal(1024L, options.MaxFileSize);
        Assert.False(options.IncludeRawBytes);
        Assert.True(options.InterpretOnParse);
        Assert.Equal(cts.Token, options.CancellationToken);
    }

    [Fact]
    public void Two_ParseOptions_with_the_same_values_are_equal()
    {
        var a = new ParseOptions { MaxDepth = 32 };
        var b = new ParseOptions { MaxDepth = 32 };
        Assert.Equal(a, b);
    }
}
