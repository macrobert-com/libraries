namespace MacRobert.CodeGeneration.Contracts;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ForeignCollectionContractAttribute : ContractAttribute
{
    public ForeignCollectionContractAttribute(Type ForeignType, string ForeignCollectionProperty)
    {
        this.ForeignType = ForeignType;
        this.ForeignCollectionProperty = ForeignCollectionProperty;
    }

    public Type ForeignType { get; }

    public string ForeignCollectionProperty { get; }
}