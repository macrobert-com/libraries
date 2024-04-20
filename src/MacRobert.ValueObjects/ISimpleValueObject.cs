namespace MacRobert.ValueObjects;

public interface ISimpleValueObject<T>
{
    public T Value { get; }
}
