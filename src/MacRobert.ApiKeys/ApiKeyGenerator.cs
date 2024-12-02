namespace MacRobert.ApiKeys;

public class ApiKeyGenerator : ApiService, IApiKeyGenerator
{
    private const int Schema = 1;

    private readonly string apiPrefix;
    private readonly IHashingService hasher;
    private readonly ICrcCalculator crc;

    public ApiKeyGenerator(string apiPrefix, IHashingService hasher, ICrcCalculator crc)
    {
        this.apiPrefix = apiPrefix;
        this.hasher = hasher;
        this.crc = crc;
    }

    public string CreateApiKey(Environment env, DateTime creationDate, string resource, string hashSecret)
    {
        var hash = hasher.CreateHash(GetProtectedValue(env, creationDate, resource), hashSecret);
        var baseKey = $"{apiPrefix}_{Schema}_{env}_{creationDate.ToString(CreationDateFormat)}_{hash.ToUpperInvariant()}";
        var crcValue = crc.Calculate(baseKey);
        return $"{baseKey}_{crcValue}";
    }
}