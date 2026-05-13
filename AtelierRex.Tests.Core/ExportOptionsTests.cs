using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class ExportOptionsTests
{
    [Fact]
    public void Default_OutputPath_is_empty_string()
    {
        Assert.Equal(string.Empty, new ExportOptions().OutputPath);
    }

    [Fact]
    public void Default_Overwrite_is_false()
    {
        Assert.False(new ExportOptions().Overwrite);
    }

    [Fact]
    public void Default_CancellationToken_is_the_default_token()
    {
        Assert.Equal(default, new ExportOptions().CancellationToken);
    }

    [Fact]
    public void All_properties_can_be_set_with_init_syntax()
    {
        var cts = new CancellationTokenSource();
        var options = new ExportOptions
        {
            OutputPath = "/out/hellcab",
            Overwrite = true,
            CancellationToken = cts.Token,
        };

        Assert.Equal("/out/hellcab", options.OutputPath);
        Assert.True(options.Overwrite);
        Assert.Equal(cts.Token, options.CancellationToken);
    }

    [Fact]
    public void Two_ExportOptions_with_the_same_values_are_equal()
    {
        var a = new ExportOptions { OutputPath = "/out", Overwrite = true };
        var b = new ExportOptions { OutputPath = "/out", Overwrite = true };
        Assert.Equal(a, b);
    }
}
