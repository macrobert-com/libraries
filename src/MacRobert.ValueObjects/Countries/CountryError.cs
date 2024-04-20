using Macrobert.ResultPattern;

namespace MacRobert.ValueObjects.Countries;

public static class CountryError
{
    private static readonly string Prefix = nameof(CountryError) + ".";

    public static readonly Error IsNullOrWhitespace = new Error(Prefix + nameof(IsNullOrWhitespace), "Country Code must not be null or whitespace");
    public static readonly Error TwoCharsOnly = new Error(Prefix + nameof(TwoCharsOnly), "Iso2 Country Code must be 2 chars only");
    public static readonly Error ThreeCharsOnly = new Error(Prefix + nameof(ThreeCharsOnly), "Iso3 Country Code must be 3 chars only");
    public static readonly Func<string, Error> MissingCountryIso2 = (iso2) => new Error(Prefix + nameof(MissingCountryIso2), $"Could not find country for Iso2:\"{iso2}\"");
    public static readonly Func<string, Error> MissingCountryIso3 = (iso3) => new Error(Prefix + nameof(MissingCountryIso3), $"Could not find country for Iso3:\"{iso3}\"");
}
