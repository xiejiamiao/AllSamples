using System;
using RabbitMQ.Client;

namespace AckSample.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("********** ACK Sample Consumer **********");

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
            var queueName = "ack_sample_queue";
            var routingKey = "order.saved";

            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true, false, null);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName,exchangeName,routingKey);

            channel.BasicConsume(queueName, false, new MyConsumer(channel));

            Console.WriteLine("输入回车退出...");
            Console.ReadLine();
        }
    }
}
