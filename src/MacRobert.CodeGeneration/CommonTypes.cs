namespace MacRobert.CodeGeneration;

public static class CommonTypes
{
    public static readonly Lazy<Dictionary<Type, string>> PrimitiveNameMap = new Lazy<Dictionary<Type, string>>(() =>
        new Dictionary<Type, string>()
        {
            { typeof(Boolean), "bool" },
            { typeof(Char), "char" },
            { typeof(Byte), "byte" },
            { typeof(SByte), "sbyte" },
            { typeof(Int16), "short" },
            { typeof(UInt16), "ushort" },
            { typeof(Int32), "int" },
            { typeof(UInt32), "uint" },
            { typeof(Int64), "long" },
            { typeof(UInt64), "ulong" },
            { typeof(Single), "float" },
            { typeof(Double), "double" },
            { typeof(Decimal), "decimal" },
            { typeof(DateTime), "DateTime" },
            { typeof(Guid), "Guid" },
            { typeof(DateOnly), "DateOnly" },
            { typeof(TimeOnly), "TimeOnly" },
        });

    public static string TryAsPrimitive(Type t)
    {
        if (!PrimitiveNameMap.Value.TryGetValue(t, out var name))
        {
            if (t == typeof(string))
            {
                name = "string";
            }
            else
            {
                name = t.Name;
            }
        }

        return name;
    }
}
