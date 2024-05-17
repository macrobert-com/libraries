using MacRobert.CodeGeneration.Contracts;

namespace MacRobert.CodeGeneration;

public class ContractContext
{
    public IList<ContractPropertyInfo> ContractProperties { get; } = new List<ContractPropertyInfo>();
    public HashSet<ForeignContractInfo> ForeignTypes { get; } = new HashSet<ForeignContractInfo>();
    public HashSet<Type> EnumTypes { get; } = new HashSet<Type>();
}
