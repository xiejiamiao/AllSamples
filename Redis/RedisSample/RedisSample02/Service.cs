using System;
using System.Collections.Generic;
using System.Text;
using Exception = System.Exception;

namespace RedisSample02
{
    public class Service
    {
        public void Handle(string id)
        {
            var key = $"compId:{id}";
            try
            {
                var value = RedisHelper.Get(key);
                if (value == null)
                {
                    //不存在，创建该值
                    RedisHelper.Set(key, long.MaxValue - 10, 20);
                }
                else
                {
                    //存在，自增，同时调用业务
                    var dbValue = RedisHelper.IncrBy(key);
                    Bussiness(id, 10 - (long.MaxValue - dbValue));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("使用已经到达次数上限，请升级会员级别");
                return;
            }
        }

        public void Bussiness(string id,long val)
        {
            Console.WriteLine($"业务操作执行  id={id}   times={val}");
        }
    }
}
