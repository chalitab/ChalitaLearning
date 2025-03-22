using ChalitaLearning.Model;
using ChalitaLearning.Services.AwsService;
using ChalitaLearning.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChalitaLearning.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AwsController : ControllerBase
    {
        private readonly AppSettings _settings;
        private readonly ILogger<AwsController> _logger;
        private readonly IAwsS3Service _awsS3Service;
        public AwsController(IOptions<AppSettings> options,
            IAwsS3Service awsS3Service)
        {
            _settings = options.Value;
            _awsS3Service = awsS3Service;
        }

        [HttpPost]
        [Route("encrypt-by-cert-from-s3")]
        public async Task<IActionResult> EndgryptByCertFromS3([FromBody] TopupFrom123Requset requset)
        {
            try
            {
                if (requset == null) 
                {
                    return BadRequest();
                }

                var str = $"{requset.MerchantId}" +
                    $"{requset.MerchantReference}" +
                    $"{requset.PaymentCode}" +
                    $"{requset.Amount:#,0.00}" +
                    $"{requset.PaidAmount:#,0.00}" +
                    $"{requset.TransactionStatus}" +
                    $"{requset.AgentCode}" +
                    $"{requset.ChannelCode}";
                var dest = CryptographyClient.GetHMACSHA256(str, _settings.SecretKeyFor123);
                requset.Checksum = dest;

                var encryptMsg = _awsS3Service.EncryptByCertFromS3(requset);

                return Ok(new TopupFrom123Response { Message = encryptMsg });
            }
            catch (Exception)
            {

                return NotFound();
            }
        }
    }
}
