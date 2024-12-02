namespace MacRobert.ApiKeys;

public interface IApiKeyGenerator
    {
        string CreateApiKey(ApiService.Environment env, DateTime creationDate, string resource, string hashSecret);
    }
