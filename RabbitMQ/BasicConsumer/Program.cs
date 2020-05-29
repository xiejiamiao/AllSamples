using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BasicConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1.创建一个ConnectionFactory
            var connectionFactory = new ConnectionFactory
            {
                HostName = "127.0.0.1", 
                Port = 5672,
                UserName = "admin", 
                Password = "admin",
                VirtualHost = "/"
            };
            // 2.通过连接工厂创建连接
            using (var connection = connectionFactory.CreateConnection())
            {
                // 3.通过connection创建Channel
                var channel = connection.CreateModel();
                // 4.声明一个队列
                var queue = channel.QueueDeclare(queue: "test001", durable: true, exclusive: false, autoDelete: true, arguments: null);
                // 5.创建消费者
                var consumer = new EventingBasicConsumer(channel);
                // 6.设置Channel
                channel.BasicConsume(queue: "test001", autoAck: true, consumer: consumer);
                // 7.获取消息
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine($"接收到消息:{message}");
                };
                Console.WriteLine("输入回车键键退出");
                Console.ReadLine();
            }
        }
    }
}
