using Macrobert.ResultPattern;
using System.Collections.Immutable;
using System.ComponentModel;

namespace MacRobert.ValueObjects.Countries;

[TypeConverter(typeof(CountryTypeConverter))]
public record Country(string Iso2, string Iso3, string Name)
{
    private static readonly ImmutableDictionary<string, Country> countriesByIso2;
    private static readonly ImmutableDictionary<string, Country> countriesByIso3;

    public const int Iso3Length = 3;
    public const int Iso2Length = 2;

    static Country()
    {
        // No point in making round-trip to DB for this.
        countriesByIso2 = CountryData.AllCountries.ToImmutableDictionary(c => c.Iso2);
        countriesByIso3 = CountryData.AllCountries.ToImmutableDictionary(c => c.Iso3);
    }

    public static Result<Country?> GetCountryByIso2(string? iso2)
    {
        Country? country = null;
        return Result.Ensure(
            iso2,
            CountryError.IsNullOrWhitespace,
            (v => !string.IsNullOrWhiteSpace(v), CountryError.IsNullOrWhitespace),
            (v => v.Length == 2, CountryError.TwoCharsOnly),
            (v => countriesByIso2.TryGetValue(v, out country), CountryError.MissingCountryIso2(iso2!)))
            .Map(_ => country ?? default);
    }

    public static Result<Country?> GetCountryByIso3(string? iso3)
    {
        Country? country = null;
        return Result.Ensure(
            iso3,
            CountryError.IsNullOrWhitespace,
            (v => !string.IsNullOrWhiteSpace(v), CountryError.IsNullOrWhitespace),
            (v => v.Length == 3, CountryError.ThreeCharsOnly),
            (v => countriesByIso3.TryGetValue(v, out country), CountryError.MissingCountryIso3(iso3!)))
            .Map(_ => country ?? default);
    }

    public static explicit operator string(Country country) => country.Iso3;
}
