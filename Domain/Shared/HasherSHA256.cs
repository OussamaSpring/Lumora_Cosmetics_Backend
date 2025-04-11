using System.Security.Cryptography;
using System.Text;

namespace Domain.Shared;

public class HasherSHA256
{
    public static string Hash(string input)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);

        using SHA256 sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(inputBytes);

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            builder.Append(hashBytes[i].ToString("x2"));
        }

        return builder.ToString();
    }
}
