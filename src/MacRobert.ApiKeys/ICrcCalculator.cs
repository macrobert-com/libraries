namespace MacRobert.ApiKeys;

public interface ICrcCalculator
{
    string Calculate(string content);
}
