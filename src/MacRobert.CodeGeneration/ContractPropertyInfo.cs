public record ContractPropertyInfo(string PropertyType, string PropertyName, bool CanRead, bool CanWrite, bool IsInitOnly)
{
    public string WriteAccessors() => $"{{ {(CanRead ? "get;" : "")}{(CanWrite ? IsInitOnly ? " init;" : " set;" : "")} }}";
}
