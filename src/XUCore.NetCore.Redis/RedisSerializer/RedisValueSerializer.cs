using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.Extensions;
using XUCore.Helpers;

namespace XUCore.NetCore.Redis
{
    public class RedisValueSerializer : IRedisSerializer
    {
        public virtual RedisValue Serializer<T>(T value)
        {
            return RedisValue.Unbox(value);
        }

        public T Deserialize<T>(RedisValue value)
        {
            if (value == RedisValue.Null) return default;

            return Conv.To<T>(value);
        }

        public IList<T> Deserialize<T>(RedisValue[] value)
        {
            if (value == null) return default;

            return value.ForEach(v => Conv.To<T>(v));
        }
    }
}
