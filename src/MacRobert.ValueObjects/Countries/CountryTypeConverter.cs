using System.ComponentModel;
using System.Globalization;

namespace MacRobert.ValueObjects.Countries;

public class CountryTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        if (sourceType == typeof(string))
        {
            return true;
        }

        return base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string)
        {
            var cty = Country.GetCountryByIso3((string)value);
            return cty.IsSuccess ? cty.Value : null;
        }

        return base.ConvertFrom(context, culture, value);
    }
}
