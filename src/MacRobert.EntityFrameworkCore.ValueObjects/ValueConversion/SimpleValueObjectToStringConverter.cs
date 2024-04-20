using MacRobert.ValueObjects;

namespace MacRobert.EntityFrameworkCore.ValueObjects.ValueConversion;

public class SimpleValueObjectToStringConverter<T, U> 
    : CompositeValueConverter<T, U, string> where T : ISimpleValueObject<U>
{
    public SimpleValueObjectToStringConverter()
        : base(
            new SimpleValueObjectToValueConverter<T, U>(),
            new ValueToStringConverter<U>())
    { }
}