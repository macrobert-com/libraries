using MacRobert.Reflection;
using MacRobert.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MacRobert.EntityFrameworkCore.ValueObjects.ValueConversion;

public static class ModelConfigurationBuilderExtensions
{
    public static ModelConfigurationBuilder ConfigureSimpleValueObjects<T>(this ModelConfigurationBuilder builder, IEnumerable<Assembly> assemblies)
    {
        Action<IReadOnlyCollection<Type>> configureSimpleValueObjects = simpleValueObjects =>
        {
            foreach (var type in simpleValueObjects)
            {
                builder
                    .Properties(type)
                    .HaveConversion(typeof(SimpleValueObjectToValueConverter<,>).MakeGenericType(type, typeof(T)));
            }
        };

        foreach (var assembly in assemblies)
        {
            var scanner = new AssemblyScanner(assembly);
            var typedIdTypes = scanner.ScanForImplementations<ISimpleValueObject<T>>();
            configureSimpleValueObjects(typedIdTypes);
        }

        return builder;
    }

    public static ModelConfigurationBuilder ConfigureSimpleValueObjectsAsString<T>(this ModelConfigurationBuilder builder, IEnumerable<Assembly> assemblies, int fieldWidth)
    {
        Action<IReadOnlyCollection<Type>> configureSimpleValueObjects = simpleValueObjects =>
        {
            foreach (var type in simpleValueObjects)
            {
                builder
                    .Properties(type)
                    .HaveConversion(typeof(SimpleValueObjectToValueConverter<,>).MakeGenericType(type, typeof(T)))
                    .HaveMaxLength(fieldWidth);
            }
        };

        foreach (var assembly in assemblies)
        {
            var scanner = new AssemblyScanner(assembly);
            var typedIdTypes = scanner.ScanForImplementations<ISimpleValueObject<T>>();
            configureSimpleValueObjects(typedIdTypes);
        }

        return builder;
    }
}
