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
    public abstract partial class StackExchangeRedis : IStringRedisCommandAsync
    {
        public async Task<TResult> StringGetOrInsertAsync<TResult>(string key, Func<Task<TResult>> fetcher, int seconds = 0, string connectionRead = null, string connectionWrite = null,
            bool isCache = true, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            if (!isCache)
                return await fetcher.Invoke();

            if (!await KeyExistsAsync(key, connectionRead))
            {
                var source = await fetcher.Invoke();
                if (source != null)
                    await StringSetAsync(key, source, seconds, connectionWrite, serializer);
                return source;
            }
            else
            {
                return await StringGetAsync<TResult>(key, connectionRead, serializer);
            }
        }

        public async Task<TResult> StringGetOrInsertAsync<T, TResult>(string key, Func<T, Task<TResult>> fetcher, T t, int seconds = 0, string connectionRead = null, string connectionWrite = null,
            bool isCache = true, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            if (!isCache)
                return await fetcher.Invoke(t);

            if (!await KeyExistsAsync(key, connectionRead))
            {
                var source = fetcher.Invoke(t);
                if (source != null)
                    await StringSetAsync(key, source, seconds, connectionWrite, serializer);
                return await source;
            }
            else
            {
                return await StringGetAsync<TResult>(key, connectionRead, serializer);
            }
        }

        public async Task<bool> StringSetAsync<T>(string key, T value, int seconds = 0, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (seconds > 0)
                {
                    if (serializer != null)
                        return await db.StringSetAsync(key, serializer.Serializer(value), TimeSpan.FromSeconds(seconds));
                    return await db.StringSetAsync(key, redisSerializer.Serializer(value), TimeSpan.FromSeconds(seconds));
                }
                else
                {
                    if (serializer != null)
                        return await db.StringSetAsync(key, serializer.Serializer(value));
                    return await db.StringSetAsync(key, redisSerializer.Serializer(value));
                }
            });
        }

        public async Task<long> StringIncrementAsync(string key, long value = 1, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.StringIncrementAsync(key, value);
            });
        }

        public async Task<TResult> StringGetAsync<TResult>(string key, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var value = await db.StringGetAsync(key);
                if (serializer != null)
                    return serializer.Deserialize<TResult>(value);
                return redisSerializer.Deserialize<TResult>(value);
            });
        }

        public async Task<IList<TResult>> StringGetAsync<TResult>(string[] keys, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                RedisKey[] redisKey = new RedisKey[keys.Length];
                for (int i = 0; i < redisKey.Length; i++)
                {
                    redisKey[i] = keys[i];
                }

                var redisValue = await db.StringGetAsync(redisKey);
                if (serializer != null)
                    return serializer.Deserialize<TResult>(redisValue);
                return redisSerializer.Deserialize<TResult>(redisValue);
            });
        }
    }
}
