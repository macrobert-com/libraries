using MacRobert.ApiKeys;

namespace Test.MacRobert.ApiKeys;

public class ApiKeyTest
{
    public const string Secret = "SScx5hNb876TbAfu9dC3";
    public const string Resource = "/api/healthchecks-notification";
    public const string ExpectedResult = "bbapi_1_test_20241128_589F2E1754117E9BE2414C81F35EC4049F8736E045EEFA224250C6E076372DBF_B1E507CB";

    public const string BadPattern = "xxapi_1_test_20241128_589F2E1754117E9BE2414C81F35EC4049F8736E045EEFA224250C6E076372DBF_B1E507CB";
    public const string BadDate = "bbapi_1_test_20241131_589F2E1754117E9BE2414C81F35EC4049F8736E045EEFA224250C6E076372DBF_B1E507CB";
    public const string BadCrc = "bbapi_1_test_20241128_589F2E1754117E9BE2414C81F35EC4049F8736E045EEFA224250C6E076372DBF_B1E507CD";
    public const string BadHash = "bbapi_1_test_20241128_589F2E1754117E9BE2414C81F35EC4049F8736E045EEFA224250C6E076372DBE_28EC5671";
    public const string CrcContent = "bbapi_1_test_20241128_589F2E1754117E9BE2414C81F35EC4049F8736E045EEFA224250C6E076372DBE";

    public readonly DateTime Today = new DateTime(2024, 11, 28);

    [Fact]
    public void ApiKey_Generates_Consistent_Result()
    {
        var crcCalc = new CrcCalculator();
        var hashingService = new HMACSHA256HashingService();

        var generator = new ApiKeyGenerator("bbapi", hashingService, crcCalc);
        var apiKey = generator.CreateApiKey(ApiService.Environment.test, Today, Resource, Secret);
        Assert.Equal(ExpectedResult, apiKey);
    }

    [Fact]
    public void ApiKey_Validates()
    {
        var crcCalc = new CrcCalculator();
        var hashingService = new HMACSHA256HashingService();
        var validator = new ApiKeyValidator("bbapi", hashingService, crcCalc);

        Assert.Equal(ApiKeyValidationError.IsNullOrWhiteSpaceError, validator.ValidateApiKey(string.Empty, Resource, Secret).Errors[0]);
        Assert.Equal(ApiKeyValidationError.PatternMismatchError.Code, validator.ValidateApiKey(BadPattern, Resource, Secret).Errors[0]);
        Assert.Equal(ApiKeyValidationError.CreationDateParseError.Code, validator.ValidateApiKey(BadDate, Resource, Secret).Errors[0]);
        Assert.Equal(ApiKeyValidationError.HashValidationError.Code, validator.ValidateApiKey(BadHash, Resource, Secret).Errors[0]);
    }
}