﻿namespace MacRobert.CodeGeneration;

public class ContractContext
{
    public IList<ContractPropertyInfo> ContractProperties { get; } = new List<ContractPropertyInfo>();
    public IList<Type> ForeignTypes { get; } = new List<Type>();
    public IList<Type> EnumTypes { get; } = new List<Type>();
}
