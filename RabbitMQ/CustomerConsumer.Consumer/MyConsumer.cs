using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace CustomerConsumer.Consumer
{
    public class MyConsumer : DefaultBasicConsumer
    {
        private readonly IModel _channel;

        public MyConsumer(IModel channel) : base(channel)
        {
            _channel = channel;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
            IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            Console.WriteLine();
            Console.WriteLine("====================");
            Console.WriteLine($"接收到消息：{Encoding.UTF8.GetString(body.ToArray())}");
            Console.WriteLine($"consumerTag = {consumerTag}");
            Console.WriteLine($"deliveryTag = {deliveryTag}");
            Console.WriteLine($"redelivered = {redelivered}");
            Console.WriteLine($"exchange = {exchange}");
            Console.WriteLine($"routingKey = {routingKey}");
            Console.WriteLine("====================");
            Console.WriteLine();
        }
    }
}
