using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Redis
{
    public static class RedisSerializerOptions
    {
        public static IRedisSerializer Json
        {
            get { return new JsonRedisSerializer(); }
        }
        public static IRedisSerializer MessagePack
        {
            get { return new MessagePackRedisSerializer(); }
        }
        public static IRedisSerializer RedisValue
        {
            get { return new RedisValueSerializer(); }
        }
    }
}
