using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ChalitaLearning.Utility
{
    public class CryptographyClient
    {
        public static string GetHMACSHA256(string signatureString, string secretKey)
        {
            var encoding = new UTF8Encoding();
            var keyByte = encoding.GetBytes(secretKey);
            var hmac = new HMACSHA256(keyByte);
            var messageBytes = encoding.GetBytes(signatureString);
            var hashmessage = hmac.ComputeHash(messageBytes);
            return ByteArrayToHexString(hashmessage);
        }

        private static string ByteArrayToHexString(byte[] Bytes) 
        {
            var result = new StringBuilder();
            var HexAlphabet = "0123456789ABCDEF";
            foreach (var b in Bytes)
            {
                result.Append(HexAlphabet[(int)(b >> 4)]);
                result.Append(HexAlphabet[(int)(b & 0xF)]);
            };
            return result.ToString().ToUpper();
        }

        public static string PKCS7Encrypt(string inputString, string publicKey) 
        {
            var envelopedCms = new EnvelopedCms(new ContentInfo(Encoding.UTF8.GetBytes(inputString)));
            envelopedCms.Encrypt(new CmsRecipient(SubjectIdentifierType.IssuerAndSerialNumber, new X509Certificate2(publicKey)));

            return Convert.ToBase64String(envelopedCms.Encode());
        }

        public static string PKCS7Encrypt(string inputString, byte[] publicKey)
        {
            var envelopedCms = new EnvelopedCms(new ContentInfo(Encoding.UTF8.GetBytes(inputString)));
            envelopedCms.Encrypt(new CmsRecipient(SubjectIdentifierType.IssuerAndSerialNumber, new X509Certificate2(publicKey)));

            return Convert.ToBase64String(envelopedCms.Encode());
        }
    }
}
