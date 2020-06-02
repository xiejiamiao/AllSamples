using System;
using System.Threading;
using System.Threading.Tasks;
using CSRedis;

namespace RedisSample02
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = new CSRedisClient("127.0.0.1:6379,defaultDatabase=0,prefix=ds_");
            
            RedisHelper.Initialization(redis);

            //限制用户调用业务次数
            //A用户限制10次/分钟，B用户限制30次/分钟，C用户不限制

            /*
             * 分析
             * 1. 设定一个服务方法，用于模拟实际业务调用的服务，内部采用打印模拟调用
             * 2. 在业务调用前服务调用控制单元，内部使用redis进行控制，参照之前的方案
             * 2. 对调用超限使用异常进行控制，异常处理设定为打印提示信息
             */

            
            var taskA = new Task(() =>
            {
                var userA = new Service();
                while (true)
                {
                    userA.Handle("userA");
                    Thread.Sleep(1000);
                }
            });
            taskA.Start();
            Console.ReadLine();
        }
    }
}
