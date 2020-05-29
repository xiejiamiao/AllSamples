using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DirectExchange.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**** Direct Exchange Consumer Sample ****");

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

            var exchangeName = "test_direct_exchange";
            var queueName = "test_direct_queue";
            var routingKey = "test.direct";

            // 声明一个交换机
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
            // 声明一个队列
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            // 建立一个绑定关系
            channel.QueueBind(queue:queueName,exchange:exchangeName,routingKey:routingKey);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: queueName, autoAck: true, consumer);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine($"接收到消息：{message}");
            };

            Console.WriteLine("输入回车键退出...");
            Console.ReadLine();
        }
    }
}
