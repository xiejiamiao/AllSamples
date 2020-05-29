using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace MessageSample.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**** MessageSample Producer ****");

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

            var message = "Hello RabbitMQ From MessageSample Producer";
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2; //2=持久化 1=非持久化
            properties.ContentEncoding = "UTF-8"; //设置字符集
            properties.Expiration = "10000"; //过期时间(毫米)
            properties.Headers = new Dictionary<string, object>() {{"my1", "111"}, {"my2", "222"}}; //自定义属性

            channel.BasicPublish("","message.sample", properties, body);

            Console.WriteLine("消息发送完毕");
        }
    }
}
