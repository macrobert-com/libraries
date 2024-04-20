
using Macrobert.ResultPattern;

namespace Test.MacRobert.ResultPattern;

public class UsageDemo
{
    public Result<string> ValidationFunction(string requestedCode)
    {
        const int ShortCodeMaxLength = 8;

        var trialUrl = WriteShortUrl(requestedCode);
        Uri rewrittenUrl;
        Result<string> result = Result.Ensure(requestedCode,
            (e => !string.IsNullOrWhiteSpace(e), RequestedCode.NullOrWhiteSpace),
            (e => e.Length <= ShortCodeMaxLength, RequestedCode.Length),
            (e => Uri.TryCreate(trialUrl, UriKind.Absolute, out rewrittenUrl) && trialUrl.Equals(rewrittenUrl.AbsoluteUri), RequestedCode.ParseUrl));

        return result;
    }

    private static string WriteShortUrl(string requestedCode)
    {
        return $"https://urlshortener.com/api/{requestedCode}";
    }


    [Fact]
    public void Demo()
    {
        Result<string> result = ValidationFunction(string.Empty);
        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.Contains(RequestedCode.NullOrWhiteSpace, result.Errors);

        result = ValidationFunction("0123456789");
        Assert.Contains(RequestedCode.Length, result.Errors);

        result = ValidationFunction("0123 456");
        Assert.Contains(RequestedCode.ParseUrl, result.Errors);
    }
}

public static class RequestedCode
{
    public static readonly Error NullOrWhiteSpace = new Error($"{nameof(RequestedCode)}.{nameof(NullOrWhiteSpace)}", "Requested code cannot be null or whitespace");
    public static readonly Error Length = new Error($"{nameof(RequestedCode)}.{nameof(Length)}", "Requested code is too long");
    public static readonly Error ParseUrl = new Error($"{nameof(RequestedCode)}.{nameof(ParseUrl)}", "Requested code will result in an invalid URL");
}