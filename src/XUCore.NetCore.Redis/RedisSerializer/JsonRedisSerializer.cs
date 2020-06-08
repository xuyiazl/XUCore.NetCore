using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using XUCore.Extensions;
using XUCore.Serializer;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Redis
{
    public class JsonRedisSerializer : IRedisSerializer
    {
        public virtual RedisValue Serializer<T>(T value)
        {
            if (value == null) return RedisValue.Null;

            return value.ToJson(new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new DefaultContractResolver()
            });
        }

        public virtual T Deserialize<T>(RedisValue value)
        {
            if (value == RedisValue.Null) return default;
            return value.SafeString().ToObject<T>();
        }

        public virtual IList<T> Deserialize<T>(RedisValue[] value)
        {
            if (value == null) return default;

            var json = value.ToStringArray().ToJson();

            return json.ToObject<IList<T>>();
        }
    }
}
