using System;
using RabbitMQ.Client;

namespace CustomerConsumer.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**********  Return Listener Sample Consumer  **********");

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

            var exchangeName = "customer_consumer_exchange";
            var routingKey = "order.saved";
            var queueName = "customer_consumer_queue";

            channel.ExchangeDeclare(exchangeName,ExchangeType.Topic,true,false,null);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);
            channel.BasicConsume(queueName, true, new MyConsumer(channel));
            
            Console.WriteLine("输入回车退出...");
            Console.ReadLine();
        }
    }
}
