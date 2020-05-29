using System;
using System.Text;
using RabbitMQ.Client;

namespace TopicExchange.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**** Topic Exchange Producer Sample ****");

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

            var exchangeName = "test_topic_exchange";
            var routingKey1 = "user.save";
            var routingKey2 = "user.update";
            var routingKey3 = "user.delete.abc";

            var message = "Hello World RabbitMQ For Topic Exchange Message";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: exchangeName, routingKey: routingKey1, basicProperties: null, body);
            channel.BasicPublish(exchange: exchangeName, routingKey: routingKey2, basicProperties: null, body);
            channel.BasicPublish(exchange: exchangeName, routingKey: routingKey3, basicProperties: null, body);
            
            Console.WriteLine("消息发送完毕");
        }
    }
}
