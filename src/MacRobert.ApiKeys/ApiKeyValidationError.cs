using Macrobert.ResultPattern;

namespace MacRobert.ApiKeys;

public static class ApiKeyValidationError
{
    public static readonly Error IsNullOrWhiteSpaceError = new Error(nameof(ApiKeyValidationError) + "." + nameof(IsNullOrWhiteSpaceError), "ApiKey is null or whitespace");
    public static readonly Error PatternMismatchError = new Error(nameof(ApiKeyValidationError) + "." + nameof(PatternMismatchError), "ApiKey does not match expected pattern");
    public static readonly Error CreationDateParseError = new Error(nameof(ApiKeyValidationError) + "." + nameof(CreationDateParseError), "ApiKey CreationDate parse failure");
    public static readonly Error CrcCheckError = new Error(nameof(ApiKeyValidationError) + "." + nameof(CrcCheckError), "ApiKey CRC check failure");
    public static readonly Error HashValidationError = new Error(nameof(ApiKeyValidationError) + "." + nameof(HashValidationError), "ApiKey hash validation error");
}
