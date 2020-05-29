using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsumerLimit.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("********** Consumer Limit Consumer **********");

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
            var routingKey = "order.#";
            var queueName = "consumer_limit_queue";

            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true, false, null);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);

            channel.BasicConsume(queueName, false, new MyConsumer(channel));
            channel.BasicQos(prefetchSize:0,prefetchCount:1,global:false);

            Console.WriteLine("输入回车退出...");
            Console.ReadLine();
        }
    }
}
