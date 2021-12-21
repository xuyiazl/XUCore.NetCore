using MessagePack;
using XUCore.Extensions;
using XUCore.Serializer;
using XUCore.Helpers;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Redis
{
    public class MessagePackRedisSerializer : IRedisSerializer
    {
        public virtual RedisValue Serializer<T>(T value)
        {
            if (value == null) return RedisValue.Null;

            return value.ToMessagePackBytes();
        }

        public virtual T Deserialize<T>(RedisValue value)
        {
            if (value == RedisValue.Null) return default;

            return Conv.To<byte[]>(value).ToMessagePackObject<T>();
        }

        public virtual IList<T> Deserialize<T>(RedisValue[] value)
        {
            if (value == null) return default;

            IList<T> list = new List<T>();
            foreach (var v in value)
            {
                if (v == RedisValue.Null)
                    list.Add(default);
                else
                    list.Add(Conv.To<byte[]>(value).ToMessagePackObject<T>());
            }

            return list;
        }
    }
}
