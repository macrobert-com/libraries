using Macrobert.ResultPattern;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MacRobert.ApiKeys;

public class ApiKeyValidator : ApiService, IApiKeyValidator
{
    public ApiKeyValidator(string apiPrefix, IHashingService hashingService, ICrcCalculator crcCalc)
    {
        this.apiPrefix = apiPrefix;
        this.apiKeyRegex = new Regex($@"^{apiPrefix}_(?<SchemaNumber>\d{{1}})_(?<Environment>(?:prod|stag|dev|test))_(?<CreationDate>\d{{8}})_(?<Token>[A-F0-9]{{64}})_(?<Crc>[A-F0-9]{{8}})$", RegexOptions.Compiled);
        this.hashingService = hashingService;
        this.crcCalc = crcCalc;
    }

    private readonly string apiPrefix;
    private readonly Regex apiKeyRegex;
    private readonly IHashingService hashingService;
    private readonly ICrcCalculator crcCalc;

    public Result ValidateApiKey(string apiKey, string resource, string hashSecret)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return Result.Failure(ApiKeyValidationError.IsNullOrWhiteSpaceError);
        }

        Match match = apiKeyRegex.Match(apiKey);
        if (!match.Success)
        {
            return Result.Failure(ApiKeyValidationError.PatternMismatchError);
        }

        var schemaNumber = match.Groups["SchemaNumber"].Value;
        var environment = match.Groups["Environment"].Value;
        var creationDate = match.Groups["CreationDate"].Value;
        var token = match.Groups["Token"].Value;
        var crc = match.Groups["Crc"].Value;

        // Validate each part
        if (!DateTime.TryParseExact(creationDate, CreationDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var creationDateTime))
        {
            return Result.Failure(ApiKeyValidationError.CreationDateParseError);
        }

        // Check the CRC
        var baseKey = string.Join("_", apiPrefix, schemaNumber, environment, creationDate, token);
        if (!crc.Equals(crcCalc.Calculate(baseKey)))
        {
            return Result.Failure(ApiKeyValidationError.CrcCheckError);
        }

        var env = Enum.Parse<Environment>(environment);

        if (!hashingService.ValidateHash(token, GetProtectedValue(env, creationDateTime, resource), hashSecret))
        {
            return Result.Failure(ApiKeyValidationError.HashValidationError);
        }

        return Result.Success();
    }
}
