using Macrobert.ResultPattern;
using System.Collections.Immutable;

namespace MacRobert.ValueObjects.Currencies;

public record Currency(string Iso, string Name, int Precision)
{
    public const int Length = 3;

    private static ImmutableDictionary<string, Currency> currenciesByIso;

    static Currency()
    {
        currenciesByIso = CurrencyData.AllCurrencies.ToImmutableDictionary(c => c.Iso);
    }

    public static readonly Currency None = new("", "", 0);

    public static Result<Currency?> GetCurrencyByIso(string? iso)
    {
        Currency? currency = null;
        return Result.Ensure(iso,
            CurrencyError.IsNullOrWhitespace,
            (v => !string.IsNullOrWhiteSpace(v), CurrencyError.IsNullOrWhitespace),
            (v => v.Length == 3, CurrencyError.ThreeCharsOnly),
            (v => currenciesByIso.TryGetValue(v, out currency), CurrencyError.MissingCurrencyIso(iso!)))
            .Map(_ => currency ?? default);
    }
}
