using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DlxExchangeSample.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("********** Dead Letter Exchange Sample Consumer **********");

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

            var exchangeName = "dlx_sample_exchange";
            var queueName = "dlx_sample_queue";
            var routingKey = "order.#";

            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true, false, null);
            channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object>() {{"x-dead-letter-exchange", "dlx.exchange"}});
            channel.QueueBind(queueName, exchangeName, routingKey);
            channel.BasicConsume(queueName, false, new MyConsumer(channel));
            
            channel.ExchangeDeclare("dlx.exchange",ExchangeType.Topic,true,false,null);
            channel.QueueDeclare("dlx.queue", true, false, false, null);
            channel.QueueBind("dlx.queue", "dlx.exchange", "#");
            var dlxConsumer = new EventingBasicConsumer(channel);
            channel.BasicConsume("dlx.queue", true, dlxConsumer);
            dlxConsumer.Received += (model, ea) =>
            {
                Console.WriteLine($"死信队列收到消息：{Encoding.UTF8.GetString(ea.Body.ToArray())}");
            };
            
            Console.WriteLine("输入回车退出...");
            Console.ReadLine();
        }
    }
}
