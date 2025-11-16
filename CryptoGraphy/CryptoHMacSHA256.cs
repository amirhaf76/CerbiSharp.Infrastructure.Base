using System.Security.Cryptography;

namespace CerbiSharp.Infrastructure.Base.CryptoGraphy
{
    public class CryptoHMacSHA256
    {
        public static byte[] GenerateRandomSecretKey(int count)
        {
            byte[] secretKey = RandomNumberGenerator.GetBytes(count);

            return secretKey;
        }

        public static byte[] SignDataBytes(byte[] secretKey, byte[] data)
        {
            using var hmac = new HMACSHA256(secretKey);

            return hmac.ComputeHash(data);
        }

        public static bool ValidateSignedData(byte[] secretKey, byte[] signedData, byte[] data)
        {
            byte[] resignedData = SignDataBytes(secretKey, data);

            int hashLength = resignedData.Length;

            for (int i = 0; i < hashLength; i++)
            {
                if (resignedData[i] != signedData[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
