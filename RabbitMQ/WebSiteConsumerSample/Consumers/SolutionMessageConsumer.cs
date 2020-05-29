using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace WebSiteConsumerSample.Consumers
{
    public class SolutionMessageConsumer:DefaultBasicConsumer
    {
        private readonly IModel _channel;
        private readonly ILogger<SolutionMessageConsumer> _logger;

        public SolutionMessageConsumer(IModel channel,ILogger<SolutionMessageConsumer> logger):base(channel)
        {
            _channel = channel;
            _logger = logger;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
            IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var message = Encoding.UTF8.GetString(body.ToArray());
            _logger.LogInformation($"接收到消息  {message}");
        }
    }
}
