using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class DiffOptionsTests
{
    [Fact]
    public void Default_strategy_is_Structural()
    {
        Assert.Equal(DiffStrategy.Structural, new DiffOptions().Strategy);
    }

    [Fact]
    public void Strategy_can_be_set_to_Content_with_init_syntax()
    {
        var options = new DiffOptions { Strategy = DiffStrategy.Content };
        Assert.Equal(DiffStrategy.Content, options.Strategy);
    }

    [Fact]
    public void Strategy_can_be_set_to_Semantic_with_init_syntax()
    {
        var options = new DiffOptions { Strategy = DiffStrategy.Semantic };
        Assert.Equal(DiffStrategy.Semantic, options.Strategy);
    }

    [Theory]
    [InlineData(DiffStrategy.Structural)]
    [InlineData(DiffStrategy.Content)]
    [InlineData(DiffStrategy.Semantic)]
    public void All_3_DiffStrategy_values_are_defined(DiffStrategy strategy)
    {
        Assert.True(Enum.IsDefined(strategy));
    }

    [Fact]
    public void Two_DiffOptions_with_the_same_strategy_are_equal()
    {
        var a = new DiffOptions { Strategy = DiffStrategy.Content };
        var b = new DiffOptions { Strategy = DiffStrategy.Content };
        Assert.Equal(a, b);
    }
}
