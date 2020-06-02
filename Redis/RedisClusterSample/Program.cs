using System;
using CSRedis;

namespace RedisClusterSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //Cluster不适用prefix，其他操作都与单机一致
            var redis = new CSRedisClient("192.168.3.177:6379,defaultDatabase=0,poolsize=50,prefix=");
            RedisHelper.Initialization(redis);
            var isSuccess= RedisHelper.Set("name", "张三");
            Console.WriteLine($"存储结果：{isSuccess}");
            var value = RedisHelper.Get("name");
            Console.WriteLine($"获取到的值：{value}");
            
            Console.WriteLine("OK");
        }
    }
}
