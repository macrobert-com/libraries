using MacRobert.CodeGeneration.Contracts;

namespace MacRobert.CodeGeneration;

public record ContractPropertyInfo(string PropertyType, string PropertyName, bool CanRead, bool CanWrite, bool IsInitOnly, ForeignContractInfo ForeignTypeInfo, Type PropertyTypeInfo = null)
{
    public string WriteAccessors() => $"{{ {(CanRead ? "get;" : "")}{(CanWrite ? IsInitOnly ? " init;" : " set;" : "")} }}";
}
