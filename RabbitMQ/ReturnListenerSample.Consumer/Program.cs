using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ReturnListenerSample.Consumer
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

            var exchangeName = "return_listener_producer";
            var routingKey = "order.#";
            var queueName = "return_listener_queue";

            channel.ExchangeDeclare(exchangeName,ExchangeType.Topic,true,false,null);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queueName, true, consumer);
            consumer.Received += (model, ea) =>
            {
                Console.WriteLine($"接收到消息：{Encoding.UTF8.GetString(ea.Body.ToArray())}");
            };
            
            Console.WriteLine("输入回车退出...");
            Console.ReadLine();
        }
    }
}
