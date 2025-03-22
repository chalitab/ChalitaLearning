using ChalitaLearning.Model;

namespace ChalitaLearning.Services.AwsService
{
    public interface IAwsS3Service
    {
        string EncryptByCertFromS3(TopupFrom123Requset requset);
    }
}
