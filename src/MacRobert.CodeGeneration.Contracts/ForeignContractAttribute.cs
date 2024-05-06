namespace MacRobert.CodeGeneration.Contracts;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ForeignContractAttribute : ContractAttribute 
{
    public ForeignContractAttribute(Type ForeignType)
    { 
        this.ForeignType = ForeignType;
    }

    public Type ForeignType { get; }
}