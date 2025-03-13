using System.Security.Cryptography;
using System.Text;

namespace PixelPulse.Application.Service.OtherService;

public static class TrackingNumberService
{
    public static string GenerateTrackingNumber()
    {
        string prefix = "PP";
        string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        string randomPart = GenerateRandomString(5);
        
        return $"{prefix}{timestamp}{randomPart}";
    }

    public static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var randomBytes = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        var result = new StringBuilder(length);
        foreach (byte b in randomBytes)
        {
            result.Append(chars[b % chars.Length]);
        }
        return result.ToString();
    }
}