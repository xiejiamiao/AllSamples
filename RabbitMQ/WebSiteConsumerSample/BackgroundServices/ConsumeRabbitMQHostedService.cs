using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using WebSiteConsumerSample.Consumers;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace WebSiteConsumerSample.BackgroundServices
{
    public class ConsumeRabbitMQHostedService : BackgroundService
    {
        private readonly ILogger<ConsumeRabbitMQHostedService> _logger;
        private readonly ILogger<SolutionMessageConsumer> _consumerLogger;
        private IConnection _connection;
        private IModel _channel;

        private string _exchangeName;
        private string _queueName;
        private string _routingKey;

        public ConsumeRabbitMQHostedService(ILogger<ConsumeRabbitMQHostedService> logger,ILogger<SolutionMessageConsumer> consumerLogger)
        {
            _logger = logger;
            _consumerLogger = consumerLogger;
            InitRabbitMq();
        }

        private void InitRabbitMq()
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "admin",
                Password = "admin",
                VirtualHost = "/"
            };

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            var exchangeName = "dimsum_solution_exchange";
            var queueName = "dimsum_solution_queue";
            var routingKey = "solution.#";

            _exchangeName = exchangeName;
            _queueName = queueName;
            _routingKey = routingKey;

            _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true, false, null);
            _channel.QueueDeclare(queueName, true, false, false, null);
            _channel.QueueBind(queueName, exchangeName, routingKey, null);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
            stoppingToken.ThrowIfCancellationRequested();
            _channel.BasicConsume(_queueName, false, new SolutionMessageConsumer(_channel, _consumerLogger));
        }

        public override void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
            base.Dispose();
        }
    }
}
