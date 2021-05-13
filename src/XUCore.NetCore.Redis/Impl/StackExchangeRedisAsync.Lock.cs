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
    public abstract partial class StackExchangeRedis : ILockRedisCommandAsync
    {
        public async Task<bool> LockExtendAsync<T>(string key, T value, int seconds, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (serializer != null)
                    return await db.LockExtendAsync(key, serializer.Serializer(value), TimeSpan.FromSeconds(seconds));
                return await db.LockExtendAsync(key, redisSerializer.Serializer(value), TimeSpan.FromSeconds(seconds));
            });
        }

        public async Task<T> LockQueryAsync<T>(string key, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                var value = await db.LockQueryAsync(key);
                if (serializer != null)
                    return serializer.Deserialize<T>(value);
                return redisSerializer.Deserialize<T>(value);
            });
        }

        public async Task<bool> LockReleaseAsync<T>(string key, T value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (serializer != null)
                    return await db.LockReleaseAsync(key, serializer.Serializer(value));
                return await db.LockReleaseAsync(key, redisSerializer.Serializer(value));
            });
        }

        public async Task<bool> LockTakeAsync<T>(string key, T value, int seconds, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, ref serializer);

            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (serializer != null)
                    return await db.LockTakeAsync(key, serializer.Serializer(value), TimeSpan.FromSeconds(seconds));
                return await db.LockTakeAsync(key, redisSerializer.Serializer(value), TimeSpan.FromSeconds(seconds));
            });
        }

    }
}
