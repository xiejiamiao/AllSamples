using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FanoutExchange.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**** Fanout Exchange Consumer Sample ****");

            var connectionFactory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "admin",
                Password = "admin",
                VirtualHost = "/",
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(3)
            };
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            var exchangeName = "test_fanout_exchange";
            var queueName = "test_fanout_queue";
            // 声明
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue:queueName,exchange:exchangeName,"");

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: queueName, autoAck: true, consumer);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine($"接收到消息：{message}");
            };
            Console.WriteLine("输入回车退出...");
            Console.ReadLine();
        }
    }
}
