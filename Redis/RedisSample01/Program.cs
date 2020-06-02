using System;
using System.Linq;
using System.Threading.Tasks;
using CSRedis;

namespace RedisSample01
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var redis = new CSRedisClient("192.168.3.177:6379,defaultDatabase=0,prefix=ds_");
            RedisHelper.Initialization(redis);

            Console.WriteLine("↓↓↓↓↓ String Sample ↓↓↓↓↓");
            await RedisHelper.SetAsync("name", "dimsum");
            var name = await RedisHelper.GetAsync<string>("name");
            Console.WriteLine($"name = {name}");
            Console.WriteLine("↑↑↑↑↑ Sample End ↑↑↑↑↑↑");
            Console.WriteLine();


            Console.WriteLine("↓↓↓↓↓ List Sample ↓↓↓↓↓");
            await RedisHelper.DelAsync("list1");
            await RedisHelper.LPushAsync("list1", "a", "b", "c");
            await RedisHelper.RPushAsync("list1", "x");

            var list1 =await RedisHelper.LRangeAsync("list1", 0, -1);
            Console.WriteLine($"list1 = {String.Join(',',list1)}");
            var list1Length = await redis.LLenAsync("list1");
            Console.WriteLine($"list1.length = {list1Length}");
            Console.WriteLine("↑↑↑↑↑ Sample End ↑↑↑↑↑↑");


            Console.WriteLine("↓↓↓↓↓ Hash Sample ↓↓↓↓↓");

            await RedisHelper.HSetAsync("hash1", "name", "张三");
            await RedisHelper.HSetAsync("hash1", "age", 19);
            await RedisHelper.HSetAsync("hash1", "job", "C#");

            var hash1 = await RedisHelper.HGetAllAsync("hash1");
            Console.WriteLine($"hash1 = {string.Join(',', hash1.Select(x => $"{x.Key}:{x.Value}").ToArray())}");
            Console.WriteLine("↑↑↑↑↑ Sample End ↑↑↑↑↑↑");
            
            Console.WriteLine();
            Console.WriteLine("====================");
            Console.WriteLine("Sample done");
        }
    }
}
