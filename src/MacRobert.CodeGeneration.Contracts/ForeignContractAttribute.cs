namespace MacRobert.CodeGeneration.Contracts;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ForeignContractAttribute : ContractAttribute
{
    public ForeignContractAttribute(Type ForeignType, string ForeignInstanceProperty = null)
    {
        this.ForeignType = ForeignType;
        this.ForeignInstanceProperty = ForeignInstanceProperty;
    }

    public Type ForeignType { get; }

    public string ForeignInstanceProperty { get; }

    public ForeignContractInfo ForeignContractInfo => new ForeignContractInfo(ForeignType, ForeignInstanceProperty);
}

public record ForeignContractInfo(Type ForeignType, string ForeignInstanceProperty);