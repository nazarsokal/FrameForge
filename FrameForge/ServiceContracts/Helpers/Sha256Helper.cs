using System.Security.Cryptography;
using System.Text;

namespace ServiceContracts.Helpers;

public static class Sha256Helper
{
    public static string ComputeHash(string codeVerifier)
    {
        using var sha256 = SHA256.Create();
        var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
        return Base64UrlEncode(challengeBytes);
    }

    private static string Base64UrlEncode(byte[] bytes)
    {
        string base64 = Convert.ToBase64String(bytes)  // Convert to Base64
            .TrimEnd('=')  // Remove padding
            .Replace('+', '-')  // Replace URL-unsafe characters
            .Replace('/', '_');  
        return base64;
    }
}