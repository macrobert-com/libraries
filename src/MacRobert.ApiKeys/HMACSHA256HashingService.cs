using System.Security.Cryptography;
using System.Text;

namespace MacRobert.ApiKeys;

public class HMACSHA256HashingService : IHashingService
{
    public string CreateHash(string input, string secretKey)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);

        using (var hmac = new HMACSHA256(keyBytes))
        {
            byte[] hashValue = hmac.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashValue.Length; i++)
            {
                sb.Append(hashValue[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }

    public bool ValidateHash(string hash, string input, string secretKey)
    {
        string generatedHash = CreateHash(input, secretKey);
        return HashEquals(generatedHash.ToUpperInvariant(), hash);
    }

    private bool HashEquals(string a, string b)
    {
        uint diff = (uint)a.Length ^ (uint)b.Length;
        for (int i = 0; i < a.Length && i < b.Length; i++)
        {
            diff |= (uint)(a[i] ^ b[i]);
        }
        return diff == 0;
    }
}
