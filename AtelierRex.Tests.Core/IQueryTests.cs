using AtelierRex.Core;

namespace AtelierRex.Tests.Core;

public class IQueryTests
{
    private sealed class TagQuery : IQuery
    {
        public FourCC Tag { get; }
        public TagQuery(FourCC tag) => Tag = tag;
    }

    private sealed class FormatQuery : IQuery
    {
        public string Family { get; }
        public FormatQuery(string family) => Family = family;
    }

    [Fact]
    public void IQuery_can_be_implemented_as_a_marker_interface_with_no_members()
    {
        IQuery query = new TagQuery(FourCC.Parse("Lscr"));
        Assert.NotNull(query);
    }

    [Fact]
    public void A_concrete_IQuery_retains_its_domain_specific_properties()
    {
        var query = new TagQuery(FourCC.Parse("Lscr"));
        Assert.Equal("Lscr", query.Tag.Value);
    }

    [Fact]
    public void Different_IQuery_implementations_can_coexist_as_the_same_interface_type()
    {
        IQuery tagQuery = new TagQuery(FourCC.Parse("Lscr"));
        IQuery formatQuery = new FormatQuery("Director");

        Assert.IsType<TagQuery>(tagQuery);
        Assert.IsType<FormatQuery>(formatQuery);
    }
}
