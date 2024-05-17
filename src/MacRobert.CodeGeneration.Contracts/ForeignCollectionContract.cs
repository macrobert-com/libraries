namespace MacRobert.CodeGeneration.Contracts;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ForeignCollectionContractAttribute : ContractAttribute
{
    public ForeignCollectionContractAttribute(Type ForeignType)
    {
        this.ForeignType = ForeignType;
    }

    public Type ForeignType { get; }
}