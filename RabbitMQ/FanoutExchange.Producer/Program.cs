using System;
using System.Text;
using RabbitMQ.Client;

namespace FanoutExchange.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**** Fanout Exchange Producer Sample ****");

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
            var message = "Hello World RabbitMQ For Fanout Exchange";
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: null, body: body);

            Console.WriteLine("消息发送完毕");
        }
    }
}
