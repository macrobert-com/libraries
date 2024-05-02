﻿using System.Reflection;
using System.Runtime.CompilerServices;

public static class TypeExtensions
{
    public static ContractContext CollectContractProperties(this Type t)
    {
        var result = new ContractContext();
        t.ForEachContractProperty(p => {
            bool hasGetter = p.CanRead;
            bool hasSetter = p.CanWrite;
            bool isInitOnly = p.IsInitOnly();

            result.ContractProperties.Add(new ContractPropertyInfo(TransformTypeName(p, p.PropertyType), p.Name, hasGetter, hasSetter, isInitOnly));
        });
        return result;
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

    public static void ForEachContractProperty(this Type t, Action<PropertyInfo> action)
    {
        var properties = t.GetProperties().Where(p => p.GetCustomAttribute<ContractAttribute>() != null);
        foreach (var property in properties)
        {
            action(property);
        }
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