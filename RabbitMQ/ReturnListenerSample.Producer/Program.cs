using System;
using System.Text;
using RabbitMQ.Client;

namespace ReturnListenerSample.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**********  Return Listener Sample Producer  **********");
            
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
            var exchangeNameError = "abc_listener_producer";
            var routingKey = "order.saved";
            var routingKeyError = "abc.saved";

            var message = "Hello RabbitMQ For ReturnListener";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchangeName, routingKey, true, null, body);
            channel.BasicPublish(exchangeName, routingKeyError, true, null, body);
            channel.BasicPublish(exchangeNameError, routingKey, true, null, body);
            channel.BasicPublish(exchangeNameError, routingKeyError, true, null, body);

            channel.BasicReturn += (model, ea) =>
            {
                Console.WriteLine();
                Console.WriteLine("====================");
                Console.WriteLine("触发Return Listener");
                Console.WriteLine($"Message = {Encoding.UTF8.GetString(ea.Body.ToArray())}");
                Console.WriteLine($"Exchange = {ea.Exchange}");
                Console.WriteLine($"ReplyCode = {ea.ReplyCode}");
                Console.WriteLine($"ReplyText = {ea.ReplyText}");
                Console.WriteLine($"RoutingKey = {ea.RoutingKey}");
                Console.WriteLine("====================");
                Console.WriteLine();
            };

            Console.WriteLine("输入回车键退出...");
            Console.ReadLine();
        }
    }
}
