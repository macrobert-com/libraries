namespace MacRobert.CodeGeneration;

public record ContractPropertyInfo(string PropertyType, string PropertyName, bool CanRead, bool CanWrite, bool IsInitOnly, Type ForeignType)
{
    public string WriteAccessors() => $"{{ {(CanRead ? "get;" : "")}{(CanWrite ? IsInitOnly ? " init;" : " set;" : "")} }}";
}
