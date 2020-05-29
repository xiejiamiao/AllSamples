using System;
using System.Text;
using RabbitMQ.Client;

namespace WebSiteSample.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("********** Producer **********");

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

            var random = new Random();

            for (var i = 0; i < 10; i++)
            {
                var message = $"Solution was added by Jiamiao -> {random.Next(10, 100)}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchangeName,routingKey,null,body);
            }

            Console.WriteLine("消息发送完毕");
        }
    }
}
