using Macrobert.ResultPattern;

namespace MacRobert.ApiKeys;

public interface IApiKeyValidator
{
    Result ValidateApiKey(string apiKey, string resource, string hashSecret);
}
