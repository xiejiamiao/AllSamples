using System;
using System.Text;
using RabbitMQ.Client;

namespace BasicProducer
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
                VirtualHost = "/",
                AutomaticRecoveryEnabled = true, //是否重连
                NetworkRecoveryInterval = TimeSpan.FromSeconds(3) //重连间隔时间
            };
            // 2.通过连接工厂创建连接
            using (var connection = connectionFactory.CreateConnection())
            {
                // 3.通过connection创建Channel
                var channel = connection.CreateModel();
                // 4.通过channel发送数据
                var message = "Hello RabbitMQ";
                var body = Encoding.UTF8.GetBytes(message);
                for (int i = 0; i < 5; i++)
                {
                    channel.BasicPublish(exchange: "", routingKey: "test001", basicProperties: null, body: body);
                }
            }
            Console.WriteLine("发送完毕");
        }
    }
}
