using System;
using System.Text;
using RabbitMQ.Client;

namespace DirectExchange.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**** Direct Exchange Producer Sample ****");

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
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var exchangeName = "test_direct_exchange";
                    var routingKey = "test.direct";

                    var message = "Hello World RabbitMQ For Direct Exchange";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchangeName, routingKey, null, body);
                }
            }

            Console.WriteLine("消息发送完毕");
        }
    }
}
