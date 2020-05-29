using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using Console = System.Console;

namespace AckSample.Consumer
{
    public class MyConsumer:DefaultBasicConsumer
    {
        private readonly IModel _channel;

        public MyConsumer(IModel channel):base(channel)
        {
            _channel = channel;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
            IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            Console.WriteLine();
            Console.WriteLine("====================");
            var stringNum = properties.Headers["num"];
            var byteNum = stringNum as byte[];
            Console.WriteLine($"接收到消息  num={Encoding.UTF8.GetString(byteNum)}  {Encoding.UTF8.GetString(body.ToArray())}");
            Console.WriteLine("......模拟业务操作......");
            var num = int.Parse(Encoding.UTF8.GetString(byteNum));
            if (num % 2 == 0)
            {
                Console.WriteLine("......业务处理失败......");
                _channel.BasicNack(deliveryTag, false, true);
            }
            else
            {
                Console.WriteLine("......业务处理成功......");
                _channel.BasicAck(deliveryTag,false);
            }
            Console.WriteLine("====================");
            Console.WriteLine();
            Thread.Sleep(2000);
        }
    }
}
