using System;
using System.Text;
using RabbitMQ.Client;

namespace ConsumerLimit.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("********** Consumer Limit Producer **********");

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

            var exchangeName = "consumer_limit_exchange";
            var routingKey = "order.saved";

            var random = new Random();
            for (var i = 0; i < 10; i++)
            {
                var message = $"Hello RabbitMQ For Consumer Limit {random.Next(1,100)}";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchangeName, routingKey, null, body);
            }

            Console.WriteLine("消息发送完成");
            Console.WriteLine("输入回车退出...");
            Console.ReadLine();
        }
    }
}
