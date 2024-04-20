using MacRobert.Util;

namespace Test.MacRobert.Util;


public class UriPathBuilderTest
{
    const string RootResource = "api/v1.0/activities";

    [Fact]
    public void When_no_settings_are_applied_the_result_is_the_same_as_the_root_resource()
    {
        var builder = new UriPathBuilder(RootResource);
        Assert.Equal(RootResource, builder.Build());
    }

    [Fact]
    public void Query_terms_are_separated_from_the_RootResource_by_a_question_mark()
    {
        var builder = new UriPathBuilder(RootResource);
        builder.WithSortTerm("Default");
        Assert.Equal('?', builder.Build()[RootResource.Length]);
    }

    [Fact]
    public void Has_Pagingation_parameters()
    {
        var builder0 = new UriPathBuilder(RootResource);
        builder0.WithPageNumber(10);
        Assert.Equal("api/v1.0/activities?page=10", builder0.Build());

        var builder1 = new UriPathBuilder(RootResource);
        builder1.WithPageSize(50);
        Assert.Equal("api/v1.0/activities?pageSize=50", builder1.Build());
    }

    [Fact]
    public void QueryParameters_are_separated_by_ampersand()
    {
        var builder0 = new UriPathBuilder(RootResource);
        builder0.WithPageNumber(10);
        builder0.WithPageSize(50);
        Assert.Equal("api/v1.0/activities?page=10&pageSize=50", builder0.Build());
    }

    [Fact]
    public void Can_Support_arbitrary_flags()
    {
        var builder0 = new UriPathBuilder(RootResource);
        builder0
            .WithBoolTerm("hello", true)
            .WithBoolTerm("world", false);

        Assert.Equal("api/v1.0/activities?hello=true&world=false", builder0.Build());
    }

    [Fact]
    public void Can_Support_arbitrary_integers()
    {
        var builder0 = new UriPathBuilder(RootResource);
        builder0
            .WithIntegerTerm("hello", 100)
            .WithIntegerTerm("world", 200);

        Assert.Equal("api/v1.0/activities?hello=100&world=200", builder0.Build());
    }
}