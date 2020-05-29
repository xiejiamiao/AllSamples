using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConfirmSample.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**********  Confirm Sample Consumer  **********");
            
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

            var exchangeName = "confirm_sample_exchange";
            var routingKey = "order.#";
            var queueName = "confirm_sample_queue";

            channel.ExchangeDeclare(exchangeName,ExchangeType.Topic,true,false,null);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queueName, true, consumer);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine($"接收到消息：{message}");
            };
            
            Console.WriteLine("输入回车退出...");
            Console.ReadLine();
        }
    }
}
