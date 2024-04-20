using System.Text;

namespace MacRobert.Util;

public class UriPathBuilder
{
    private record BoolTerm(string Name, bool Value);
    private record IntegerTerm(string Name, int Value);

    private readonly string resourceRoot;
    private Lazy<List<string>> sortTerms = new(() => new List<string>());
    private Lazy<List<string>> filterTerms = new(() => new List<string>());
    private Lazy<List<BoolTerm>> boolTerms = new(() => new List<BoolTerm>());
    private Lazy<List<IntegerTerm>> integerTerms = new(() => new List<IntegerTerm>());
    private int? pageSize;
    private int? pageNumber;

    public UriPathBuilder(string resourceRoot)
    {
        this.resourceRoot = resourceRoot;
    }

    public UriPathBuilder WithSortTerm(string sortTerm)
    {
        sortTerms.Value.Add(sortTerm);
        return this;
    }

    public UriPathBuilder WithFilterTerm(string filterTerm)
    {
        filterTerms.Value.Add(filterTerm);
        return this;
    }

    public UriPathBuilder WithBoolTerm(string name, bool value)
    {
        boolTerms.Value.Add(new BoolTerm(name, value));
        return this;
    }
    public UriPathBuilder WithIntegerTerm(string name, int value)
    {
        integerTerms.Value.Add(new IntegerTerm(name, value));
        return this;
    }

    public UriPathBuilder WithPageSize(int? pageSize)
    {
        if (pageSize is not null) this.pageSize = pageSize;
        return this;
    }

    public UriPathBuilder WithPageNumber(int? pageNumber)
    {
        if (pageNumber is not null) this.pageNumber = pageNumber;
        return this;
    }

    const string termSeparator = "&";

    public string Build()
    {
        var result = new StringBuilder(resourceRoot);
        var queryTerms = new List<string>();

        if (sortTerms.IsValueCreated) queryTerms.Add($"sorts={string.Join(",", sortTerms.Value)}");
        if (filterTerms.IsValueCreated) queryTerms.Add($"filters={string.Join(",", filterTerms.Value)}");
        if (boolTerms.IsValueCreated) queryTerms.Add(string.Join(termSeparator, boolTerms.Value.Select(x => $"{x.Name}={x.Value.ToString().ToLower()}")));
        if (integerTerms.IsValueCreated) queryTerms.Add(string.Join(termSeparator, integerTerms.Value.Select(x => $"{x.Name}={x.Value}")));
        if (pageNumber is not null) queryTerms.Add($"page={pageNumber.Value}");
        if (pageSize is not null) queryTerms.Add($"pageSize={pageSize.Value}");

        if (queryTerms.Count > 0) result.Append('?');
        result.Append(string.Join(termSeparator, queryTerms));

        return result.ToString();
    }
}
