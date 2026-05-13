using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class FourCCTests
{
    [Fact]
    public void Parse_from_4_bytes_returns_the_correct_value()
    {
        var bytes = new byte[] { (byte)'R', (byte)'I', (byte)'F', (byte)'X' };
        var fourCC = FourCC.Parse(bytes.AsSpan());
        Assert.Equal("RIFX", fourCC.Value);
    }

    [Fact]
    public void Parse_from_bytes_uses_Latin1_encoding()
    {
        // 0xA9 is © in Latin-1 — verifies we are not using ASCII or UTF-8
        var bytes = new byte[] { 0xA9, (byte)'A', (byte)'B', (byte)'C' };
        var fourCC = FourCC.Parse(bytes.AsSpan());
        Assert.Equal("©ABC", fourCC.Value);
    }

    [Fact]
    public void Parse_from_more_than_4_bytes_uses_only_the_first_4()
    {
        var bytes = new byte[] { (byte)'R', (byte)'I', (byte)'F', (byte)'X', (byte)'Z' };
        var fourCC = FourCC.Parse(bytes.AsSpan());
        Assert.Equal("RIFX", fourCC.Value);
    }

    [Fact]
    public void Parse_from_fewer_than_4_bytes_throws_ArgumentException()
    {
        var bytes = new byte[] { (byte)'A', (byte)'B', (byte)'C' };
        Assert.Throws<ArgumentException>(() => FourCC.Parse(bytes.AsSpan()));
    }

    [Fact]
    public void Parse_from_string_of_exactly_4_characters_returns_the_correct_value()
    {
        var fourCC = FourCC.Parse("Lscr");
        Assert.Equal("Lscr", fourCC.Value);
    }

    [Fact]
    public void Parse_from_null_string_throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => FourCC.Parse((string)null!));
    }

    [Fact]
    public void Parse_from_string_shorter_than_4_characters_throws_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => FourCC.Parse("ABC"));
    }

    [Fact]
    public void Parse_from_string_longer_than_4_characters_throws_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => FourCC.Parse("ABCDE"));
    }

    [Fact]
    public void Two_FourCCs_with_the_same_value_are_equal()
    {
        var a = FourCC.Parse("RIFX");
        var b = FourCC.Parse("RIFX");
        Assert.Equal(a, b);
    }

    [Fact]
    public void Two_FourCCs_with_different_values_are_not_equal()
    {
        var a = FourCC.Parse("RIFX");
        var b = FourCC.Parse("mmap");
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void Equality_operator_returns_true_for_equal_FourCCs()
    {
        var a = FourCC.Parse("RIFX");
        var b = FourCC.Parse("RIFX");
        Assert.True(a == b);
    }

    [Fact]
    public void Inequality_operator_returns_true_for_different_FourCCs()
    {
        var a = FourCC.Parse("RIFX");
        var b = FourCC.Parse("mmap");
        Assert.True(a != b);
    }

    [Fact]
    public void GetHashCode_is_the_same_for_equal_FourCCs()
    {
        var a = FourCC.Parse("RIFX");
        var b = FourCC.Parse("RIFX");
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void ToString_returns_the_value_string()
    {
        var fourCC = FourCC.Parse("BITD");
        Assert.Equal("BITD", fourCC.ToString());
    }

    [Fact]
    public void Case_is_preserved_in_the_value_string()
    {
        var lower = FourCC.Parse("mmap");
        var upper = FourCC.Parse("MMAP");
        Assert.NotEqual(lower, upper);
    }
}
