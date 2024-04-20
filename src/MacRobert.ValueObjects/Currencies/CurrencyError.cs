using Macrobert.ResultPattern;

namespace MacRobert.ValueObjects.Currencies;

public static class CurrencyError
{
    private static readonly string Prefix = nameof(CurrencyError) + ".";

    public static readonly Error IsNullOrWhitespace = new Error(Prefix + nameof(IsNullOrWhitespace), "Currency Code must not be null or whitespace");
    public static readonly Error ThreeCharsOnly = new Error(Prefix + nameof(ThreeCharsOnly), "Iso Currency Code must be 3 chars only");
    public static readonly Func<string, Error> MissingCurrencyIso = (iso) => new Error(Prefix + nameof(MissingCurrencyIso), $"Could not find currency for Iso3:\"{iso}\"");
}
