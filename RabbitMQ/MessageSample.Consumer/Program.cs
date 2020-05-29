using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageSample.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**** MessageSample Consumer ****");

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

            
            var queueName = "message.sample";
            channel.QueueDeclare(queueName,true,false,false,null);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queueName, true, consumer);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                var properties = ea.BasicProperties;
                Console.WriteLine($"接收到消息：{message}");
                Console.WriteLine($"Properties.Header = {string.Join(',', properties.Headers.ToArray())}");
            };

            Console.WriteLine("输入回车退出");
            Console.ReadLine();
        }
    }
}
