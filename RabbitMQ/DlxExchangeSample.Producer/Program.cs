using System;
using System.Text;
using RabbitMQ.Client;

namespace DlxExchangeSample.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("********** Dead Letter Exchange Sample Producer **********");

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

            var exchangeName = "dlx_sample_exchange";
            var routingKey = "order.saved";

            var message = "Hello RabbitMQ For Dead Letter Exchange Sample";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchangeName, routingKey, null, body);

            Console.WriteLine("输入回车退出...");
            Console.ReadLine();
        }
    }
}
