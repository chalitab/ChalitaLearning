using Amazon.SQS;
using Amazon.SQS.Model;
using ChalitaLearning.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ChalitaLearning.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IAmazonSQS sqsClient,
            IConfiguration configuration,
            ILogger<OrdersController> logger) 
        {
            _sqsClient = sqsClient;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto order) 
        {
            _logger.LogInformation("Receive new order: {OrderId}", order.OrderId);

            var queueUrl = _configuration["SqsQueueUrl"];
            var messageBody = JsonSerializer.Serialize(order);
            var request = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = messageBody,
                MessageGroupId = "order-group-1",
                MessageDeduplicationId = Guid.NewGuid().ToString()
            };

            var response = await _sqsClient.SendMessageAsync(request);

            return Ok(new { MessageId = response.MessageId});
        }


        [HttpPost("callback")]
        public IActionResult Callback([FromBody] OrderDto order) 
        {
            _logger.LogInformation("Callback from consumer - Order : {OrderId}, Customer: {CustomerName}", order.OrderId, order.CustomerName);
            return Ok(new { status = "received", time = DateTime.UtcNow});
        }
    }
}
