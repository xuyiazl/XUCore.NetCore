using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using XUCore.Extensions;
using XUCore.Serializer;
using XUCore.NetCore.Redis.RedisCommand;

namespace XUCore.NetCore.Redis
{
    public abstract partial class StackExchangeRedis : ISetRedisCommand
    {
        public bool SetAdd<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (serializer != null)
                    return db.SetAdd(key, serializer.Serializer(value));
                return db.SetAdd(key, redisSerializer.Serializer(value));
            });
        }

        public IList<T> SetMembers<T>(string key, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var value = db.SetMembers(key);
                if (serializer != null)
                    return serializer.Deserialize<T>(value);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public bool SetExists<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (serializer != null)
                    return db.SetContains(key, serializer.Serializer(value));
                return db.SetContains(key, redisSerializer.Serializer(value));
            });
        }

        public bool SetRemove<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                if (serializer != null)
                    return db.SetRemove(key, serializer.Serializer(value));
                return db.SetRemove(key, redisSerializer.Serializer(value));
            });
        }

        public T SetPop<T>(string key, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                var value = db.SetPop(key);
                if (serializer != null)
                    return serializer.Deserialize<T>(value);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public long SetLength(string key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                return db.SetLength(key);
            });
        }

        public T SetRandom<T>(string key, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var value = db.SetRandomMember(key);

                if (serializer != null)
                    return serializer.Deserialize<T>(value);
                return redisSerializer.Deserialize<T>(value);
            });
        }
    }
}
