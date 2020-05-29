using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace WebSiteProducerSample.Controllers
{
    [Route("{controller}/{action}")]
    public class TempController : Controller
    {
        private readonly ILogger<TempController> _logger;

        public TempController(ILogger<TempController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Publish()
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "admin",
                Password = "admin",
                VirtualHost = "/"
            };

            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            var exchangeName = "dimsum_solution_exchange";
            var routingKey = "solution.added";

            var message = $"当前时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            var body = Encoding.UTF8.GetBytes(message);

            channel.ConfirmSelect();
            channel.BasicAcks += (model, ea) =>
            {
                _logger.LogInformation($"接收到Broker返回的ACK  ->   DeliveryTag = {ea.DeliveryTag}   Multiple = {ea.Multiple}");
            };
            channel.BasicPublish(exchangeName, routingKey, false, null, body);
            return Json(new { });
        }
    }
}