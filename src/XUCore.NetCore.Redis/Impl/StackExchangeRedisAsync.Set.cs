using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using XUCore.Extensions;
using XUCore.Serializer;
using System.Threading.Tasks;
using XUCore.NetCore.Redis.RedisCommand;

namespace XUCore.NetCore.Redis
{
    public abstract partial class StackExchangeRedis : ISetRedisCommandAsync
    {
        public async Task<bool> SetAddAsync<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (serializer != null)
                    return await db.SetAddAsync(key, serializer.Serializer(value));
                return await db.SetAddAsync(key, redisSerializer.Serializer(value));
            });
        }

        public async Task<IList<T>> SetMembersAsync<T>(string key, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var value = await db.SetMembersAsync(key);
                if (serializer != null)
                    return serializer.Deserialize<T>(value);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public async Task<bool> SetExistsAsync<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (serializer != null)
                    return await db.SetContainsAsync(key, serializer.Serializer(value));
                return await db.SetContainsAsync(key, redisSerializer.Serializer(value));
            });
        }

        public async Task<bool> SetRemoveAsync<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (serializer != null)
                    return await db.SetRemoveAsync(key, serializer.Serializer(value));
                return await db.SetRemoveAsync(key, redisSerializer.Serializer(value));
            });
        }

        public async Task<T> SetPopAsync<T>(string key, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var value = await db.SetPopAsync(key);
                if (serializer != null)
                    return serializer.Deserialize<T>(value);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public async Task<long> SetLengthAsync(string key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                return await db.SetLengthAsync(key);
            });
        }

        public async Task<T> SetRandomAsync<T>(string key, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var value = await db.SetRandomMemberAsync(key);
                if (serializer != null)
                    return serializer.Deserialize<T>(value);
                return redisSerializer.Deserialize<T>(value);
            });
        }
    }
}
