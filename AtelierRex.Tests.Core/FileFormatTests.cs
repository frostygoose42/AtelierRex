using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class FileFormatTests
{
    private static readonly Version V4 = new(4, 0);

    [Fact]
    public void Constructor_sets_all_properties()
    {
        var format = new FileFormat("Director Movie", "Director", V4, Endianness.BigEndian);
        Assert.Equal("Director Movie", format.Name);
        Assert.Equal("Director", format.Family);
        Assert.Equal(V4, format.Version);
        Assert.Equal(Endianness.BigEndian, format.Endianness);
    }

    [Fact]
    public void Two_formats_with_identical_properties_are_equal()
    {
        var a = new FileFormat("Director Movie", "Director", V4, Endianness.BigEndian);
        var b = new FileFormat("Director Movie", "Director", V4, Endianness.BigEndian);
        Assert.Equal(a, b);
    }

    [Fact]
    public void Two_formats_differing_in_name_are_not_equal()
    {
        var a = new FileFormat("Director Movie", "Director", V4, Endianness.BigEndian);
        var b = new FileFormat("Director Protected", "Director", V4, Endianness.BigEndian);
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void Two_formats_differing_in_family_are_not_equal()
    {
        var a = new FileFormat("Wave Audio", "RIFF", V4, Endianness.LittleEndian);
        var b = new FileFormat("Wave Audio", "Director", V4, Endianness.LittleEndian);
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void Two_formats_differing_in_version_are_not_equal()
    {
        var a = new FileFormat("Director Movie", "Director", new Version(4, 0), Endianness.BigEndian);
        var b = new FileFormat("Director Movie", "Director", new Version(5, 0), Endianness.BigEndian);
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void Two_formats_differing_in_endianness_are_not_equal()
    {
        var a = new FileFormat("Director Movie", "Director", V4, Endianness.BigEndian);
        var b = new FileFormat("Director Movie", "Director", V4, Endianness.LittleEndian);
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void Equality_operator_returns_true_for_equal_formats()
    {
        var a = new FileFormat("Director Movie", "Director", V4, Endianness.BigEndian);
        var b = new FileFormat("Director Movie", "Director", V4, Endianness.BigEndian);
        Assert.True(a == b);
    }

    [Fact]
    public void Inequality_operator_returns_true_for_different_formats()
    {
        var a = new FileFormat("Director Movie", "Director", V4, Endianness.BigEndian);
        var b = new FileFormat("Director Protected", "Director", V4, Endianness.BigEndian);
        Assert.True(a != b);
    }

    [Fact]
    public void GetHashCode_is_the_same_for_equal_formats()
    {
        var a = new FileFormat("Director Movie", "Director", V4, Endianness.BigEndian);
        var b = new FileFormat("Director Movie", "Director", V4, Endianness.BigEndian);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void ToString_includes_name_family_version_and_endianness()
    {
        var format = new FileFormat("Director Movie", "Director", V4, Endianness.BigEndian);
        var str = format.ToString();
        Assert.Contains("Director Movie", str);
        Assert.Contains("Director", str);
        Assert.Contains("4.0", str);
        Assert.Contains("BigEndian", str);
    }

    [Theory]
    [InlineData(Endianness.LittleEndian)]
    [InlineData(Endianness.BigEndian)]
    public void Both_Endianness_values_are_defined(Endianness value)
    {
        Assert.True(Enum.IsDefined(value));
    }
}
