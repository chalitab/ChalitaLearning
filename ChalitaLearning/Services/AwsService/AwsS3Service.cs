using Amazon.S3;
using Amazon.S3.Model;
using ChalitaLearning.Model;
using ChalitaLearning.Utility;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ChalitaLearning.Services.AwsService
{
    public class AwsS3Service : IAwsS3Service
    {
        private readonly AppSettings _appSettings;
        private readonly IAmazonS3 _amazonS3;

        public AwsS3Service(IOptions<AppSettings> appSettings,
            IAmazonS3 amazonS3
            ) 
        {
            _appSettings = appSettings.Value;
            _amazonS3 = amazonS3;
        }

        public string EncryptByCertFromS3(TopupFrom123Requset requset)
        {
            try
            {
                var publicCertPath = _appSettings.PublicCertPath123;
                var bucketName = _appSettings.S2BucketName;

                var text = JsonConvert.SerializeObject(requset);
                string encrypt = "";
                if (string.IsNullOrEmpty(bucketName))
                {
                    if (!File.Exists(publicCertPath))
                        throw new FileNotFoundException();

                    encrypt = CryptographyClient.PKCS7Encrypt(text, publicCertPath);
                }
                else
                {
                    var obj = new GetObjectRequest
                    {
                        BucketName = bucketName,
                        Key = publicCertPath
                    };

                    byte[] certData;
                    using (var response = _amazonS3.GetObjectAsync(obj).Result)
                    using (var memoryStream = new MemoryStream())
                    {
                        response.ResponseStream.CopyTo(memoryStream);
                        certData = memoryStream.ToArray();
                    }
                    encrypt = CryptographyClient.PKCS7Encrypt(text, certData);
                }

                return encrypt;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
