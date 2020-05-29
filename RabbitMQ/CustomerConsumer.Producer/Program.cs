using System;
using System.Text;
using RabbitMQ.Client;

namespace CustomerConsumer.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**********  Return Listener Sample Producer  **********");

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
            channel.ConfirmSelect();
            var exchangeName = "customer_consumer_exchange";
            var routingKey = "order.saved";

            channel.BasicAcks += (model, ea) =>
            {
                Console.WriteLine();
                Console.WriteLine("====================");
                Console.WriteLine("消息发送成功(Broker返回ACK)");
                Console.WriteLine($"DeliveryTag = {ea.DeliveryTag}");
                Console.WriteLine($"Multiple = {ea.Multiple}");
                Console.WriteLine("====================");
                Console.WriteLine();
            };

            for (var i = 0; i < 5; i++)
            {
                var message = $"Hello RabbitMQ For CustomerConsumer By {i}";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchangeName, routingKey, null, body);
            }

            Console.WriteLine("消息发送完毕");
            Console.WriteLine("输入回车退出...");
            Console.ReadLine();
        }
    }
}
