namespace MacRobert.ApiKeys;

public interface IHashingService
{
    string CreateHash(string input, string secretKey);
    bool ValidateHash(string hash, string input, string secretKey);
}
