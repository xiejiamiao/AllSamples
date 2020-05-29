using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace AckSample.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("********** ACK Sample Producer **********");

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

            var exchangeName = "ack_sample_exchange";
            var routingKey = "order.saved";

            var random =new Random();

            for (var i = 0; i < 10; i++)
            {
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2;
                properties.ContentEncoding = "UTF-8";
                properties.Headers = new Dictionary<string, object>() {{"num", random.Next(0, 10).ToString()}};
                
                var message = $"Hello RabbitMQ For ACK  ->  {i}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchangeName, routingKey, properties, body);
            }

            Console.WriteLine("输入回车退出...");
            Console.ReadLine();
        }
    }
}
