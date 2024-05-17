using MacRobert.CodeGeneration.Contracts;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MacRobert.CodeGeneration;

public static class TypeExtensions
{
    public static ContractContext CollectContractProperties(this Type t)
    {
        var result = new ContractContext();

        t.ForEachContractProperty( (propInfo, foreignContractInfo) => {
            bool hasGetter = propInfo.CanRead;
            bool hasSetter = propInfo.CanWrite;
            bool isInitOnly = propInfo.IsInitOnly();

            result.ContractProperties.Add(new ContractPropertyInfo(TransformTypeName(propInfo, propInfo.PropertyType), propInfo.Name, hasGetter, hasSetter, isInitOnly, foreignContractInfo, propInfo.PropertyType));
            if (foreignContractInfo != null)
            {
                result.ForeignTypes.Add(foreignContractInfo);
            }
            if (propInfo.IsEnumType())
            {
                result.EnumTypes.Add(propInfo.GetEnumType());
            }
        });

        return result;
    }

    public static bool IsEnumType(this PropertyInfo property) => IsEnumType(property.PropertyType);

    public static bool IsEnumType(this Type t)
    {
        var x = (Nullable.GetUnderlyingType(t)?.IsEnum ?? false);
        var y = t.IsEnum;
        return t.IsEnum || (Nullable.GetUnderlyingType(t)?.IsEnum ?? false);
    }

    public static Type GetEnumType(this PropertyInfo property)
    {
        if (property.IsNullable())
        {
            return Nullable.GetUnderlyingType(property.PropertyType)!;
        }

        return property.PropertyType;
    }

    public static Type GetEnumType(this Type t)
    {
        return Nullable.GetUnderlyingType(t) ?? t;
    }

    private static string? TransformNullableName(Type propertyType)
    {
        return $"{CommonTypes.TryAsPrimitive(propertyType)}?";
    }

    public static bool IsInitOnly(this PropertyInfo property)
    {
        if (!property.CanWrite)
        {
            return false;
        }

        var setMethod = property.SetMethod;

        // Get the modifiers applied to the return parameter.
        var setMethodReturnParameterModifiers = setMethod.ReturnParameter.GetRequiredCustomModifiers();

        // Init-only properties are marked with the IsExternalInit type.
        return setMethodReturnParameterModifiers.Contains(typeof(System.Runtime.CompilerServices.IsExternalInit));
    }

    private static string TransformTypeName(PropertyInfo propertyInfo, Type propertyReturnType)
    {
        bool isReturnTypeNullable = propertyReturnType.IsNullable();
        bool isPropertyTypeNullable = propertyInfo.IsNullable();

        if (isPropertyTypeNullable && !isReturnTypeNullable)
        {
            return TransformNullableName(propertyInfo.PropertyType);
        }

        if (isPropertyTypeNullable && isReturnTypeNullable)
        {
            var t = propertyReturnType;
            t = Nullable.GetUnderlyingType(t);
            return TransformNullableName(t);
        }

        return CommonTypes.TryAsPrimitive(propertyReturnType);
    }

    public static void ForEachContractProperty(this Type t, Action<PropertyInfo, ForeignContractInfo> action)
    {
        var properties = t.GetProperties().Where(HasContractAttribute);
        foreach (var property in properties)
        {
            action(property, property.GetCustomAttribute<ForeignContractAttribute>()?.ForeignContractInfo!);
        }
    }

    private static bool HasContractAttribute(PropertyInfo p)
    {
        return 
            p.GetCustomAttribute<ContractAttribute>() != null ||
            p.GetCustomAttribute<ForeignContractAttribute>() != null;
    }

    public static bool IsNullable(this Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        return (Nullable.GetUnderlyingType(type) != null);
    }

    public static bool IsNullable(this PropertyInfo memberInfo)
    {
        if (memberInfo == null)
            throw new ArgumentNullException(nameof(memberInfo));

        Type type = memberInfo.PropertyType;

        if (Nullable.GetUnderlyingType(type) != null)
            return true;

        // Handle nullable reference types
        if (!type.IsValueType)
        {
            var nullableAttribute = memberInfo
                .GetCustomAttributes<NullableAttribute>()
                .FirstOrDefault();

            if (nullableAttribute != null && nullableAttribute.NullableFlags.Length > 0)
            {
                return nullableAttribute.NullableFlags[0] == 2;
            }

            var contextAttribute = memberInfo.DeclaringType?
                .GetCustomAttributes<NullableContextAttribute>()
                .FirstOrDefault();

            if (contextAttribute != null)
            {
                return contextAttribute.Flag == 2;
            }
        }

        return false;
    }
}
