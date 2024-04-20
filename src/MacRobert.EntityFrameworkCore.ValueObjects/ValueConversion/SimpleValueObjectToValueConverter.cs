using MacRobert.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MacRobert.EntityFrameworkCore.ValueObjects.ValueConversion;

public class SimpleValueObjectToValueConverter<T, U> : ValueConverter<T, U> where T : ISimpleValueObject<U>
{
    public SimpleValueObjectToValueConverter(ConverterMappingHints? mappingHints = null)
        : base(
            convertToProviderExpression: x => x.Value,
            convertFromProviderExpression: x => (T)Activator.CreateInstance(typeof(T), new object[] { x })!,
            mappingHints)
    { }
}
