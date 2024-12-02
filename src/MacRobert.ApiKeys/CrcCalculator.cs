namespace MacRobert.ApiKeys;

public class CrcCalculator : ICrcCalculator
{
    public string Calculate(string content)
    {
        uint crc = 0xffffffff;
        foreach (char c in content)
        {
            crc ^= c;
            for (int i = 0; i < 8; i++)
            {
                if ((crc & 1) != 0)
                    crc = (crc >> 1) ^ 0xEDB88320; // Polynomial used in PKZIP's CRC32
                else
                    crc = crc >> 1;
            }
        }
        crc ^= 0xffffffff;
        return crc.ToString("X8"); // Convert to 8 character hexadecimal
    }
}
