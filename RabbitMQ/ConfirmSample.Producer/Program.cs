using System;
using System.Text;
using RabbitMQ.Client;

namespace ConfirmSample.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**********  Confirm Sample Producer  **********");


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

            //开启Confirm模式
            channel.ConfirmSelect();

            var exchangeName = "confirm_sample_exchange";
            var routingKey = "order.saved";

            var message = "Hello RabbitMQ For Confirm";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchangeName, routingKey, false, null, body);

            channel.BasicAcks += (model, ea) =>
            {
                Console.WriteLine("====================");
                Console.WriteLine("Broker确认收到消息");
                Console.WriteLine($"DeliveryTag = {ea.DeliveryTag}");
                Console.WriteLine($"Multiple = {ea.Multiple}");
                Console.WriteLine("====================");
            };
            channel.BasicNacks += (model, ea) =>
            {
                Console.WriteLine("消息发送失败");
            };

            Console.WriteLine("输入回车退出...");
            Console.ReadLine();
        }
    }
}
