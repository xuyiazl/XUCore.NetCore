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
        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            ContractResolver = new DefaultContractResolver()
        };

        public virtual RedisValue Serializer<T>(T value)
        {
            if (value == null) return RedisValue.Null;

            return value.ToJson(serializerSettings);
        }

        public virtual T Deserialize<T>(RedisValue value)
        {
            if (value == RedisValue.Null) return default;

            return value.SafeString().ToObject<T>(serializerSettings);
        }

        public virtual IList<T> Deserialize<T>(RedisValue[] value)
        {
            if (value == null) return default;

            return value.ToStringArray().ToObject<T>(serializerSettings);
        }
    }
}
