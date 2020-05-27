using Newtonsoft.Json.Serialization;
using XUCore.Extensions;
using XUCore.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Redis
{
    public interface IRedisSerializer
    {
        RedisValue Serializer<T>(T value);

        T Deserialize<T>(RedisValue value);

        IList<T> Deserialize<T>(RedisValue[] value);
    }

   

}
