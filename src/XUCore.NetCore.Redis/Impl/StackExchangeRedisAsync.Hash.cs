using XUCore.NetCore.Redis.RedisCommand;
using XUCore.Extensions;
using XUCore.Serializer;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XUCore.NetCore.Redis
{
    public abstract partial class StackExchangeRedis : IHashRedisCommandAsync
    {
        public async Task<bool> HashSetAsync<T>(string hashId, string key, T value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                if (serializer != null)
                    return await db.HashSetAsync(hashId, key, serializer.Serializer(value));
                return await db.HashSetAsync(hashId, key, redisSerializer.Serializer(value));
            });
        }

        public async Task<bool> HashSetAsync<T>(string hashId, string key, T value, OverWrittenTypeDenum isAlways, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
             {
                 When when = When.Always;
                 switch (isAlways)
                 {
                     case OverWrittenTypeDenum.Always:
                         when = When.Always;
                         break;
                     case OverWrittenTypeDenum.Exists:
                         when = When.Exists;
                         break;
                     case OverWrittenTypeDenum.NotExists:
                         when = When.NotExists;
                         break;
                 }

                 if (serializer != null)
                     return await db.HashSetAsync(hashId, key, serializer.Serializer(value), when);
                 return await db.HashSetAsync(hashId, key, redisSerializer.Serializer(value), when);
             });
        }

        public async Task<TResult> HashGetOrInsertAsync<TResult>(string hashId, string key, Func<Task<TResult>> fetcher, int seconds = 0, string connectionRead = null, string connectionWrite = null,
            bool isCache = true, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            if (!isCache)
                return await fetcher.Invoke();

            if (!await HashExistsAsync(hashId, key, connectionRead))
            {
                var source = await fetcher.Invoke();
                if (source != null)
                {
                    if (seconds > 0)
                    {
                        bool exists = await KeyExistsAsync(hashId, connectionRead);

                        await HashSetAsync(hashId, key, source, connectionWrite, serializer);
                        if (!exists)
                        {
                            await KeyExpireAsync(hashId, seconds, connectionWrite);
                        }
                    }
                    else
                    {
                        await HashSetAsync(hashId, key, source, connectionWrite, serializer);
                    }
                }
                return source;
            }
            else
            {
                return await HashGetAsync<TResult>(hashId, key, connectionRead, serializer);
            }
        }

        public async Task<TResult> HashGetOrInsertAsync<T, TResult>(string hashId, string key, Func<T, Task<TResult>> fetcher, T t, int seconds = 0, string connectionRead = null, string connectionWrite = null,
            bool isCache = true, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            if (!isCache)
                return await fetcher.Invoke(t);

            if (!await HashExistsAsync(hashId, key, connectionRead))
            {
                var source = fetcher.Invoke(t);
                if (source != null)
                {
                    if (seconds > 0)
                    {
                        bool exists = await KeyExistsAsync(hashId, connectionRead);
                        await HashSetAsync(hashId, key, source, connectionWrite, serializer);
                        if (!exists)
                        {
                            await KeyExpireAsync(hashId, seconds, connectionWrite);
                        }
                    }
                    else
                    {
                        await HashSetAsync(hashId, key, source, connectionWrite, serializer);
                    }
                }
                return await source;
            }
            else
            {
                return await HashGetAsync<TResult>(hashId, key, connectionRead, serializer);
            }
        }

        public async Task<TResult> HashGetAsync<TResult>(string hashId, string key, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var res = await db.HashGetAsync(hashId, key);

                if (serializer != null)
                    return serializer.Deserialize<TResult>(res);
                return redisSerializer.Deserialize<TResult>(res);
            });
        }

        public async Task<IList<TResult>> HashGetAsync<TResult>(string hashId, string[] keys, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                List<RedisValue> listvalues = new List<RedisValue>();

                foreach (var key in keys)
                    listvalues.Add(key);

                var res = await db.HashGetAsync(hashId, listvalues.ToArray());

                if (serializer != null)
                    return serializer.Deserialize<TResult>(res);
                return redisSerializer.Deserialize<TResult>(res);
            });
        }

        public async Task<Dictionary<string, string>> HashGetAllAsync(string hashId, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var res = await db.HashGetAllAsync(hashId);

                return res.ToStringDictionary();
            });
        }

        public async Task<TResult> HashGetAllAsync<TResult>(string hashId, Func<Dictionary<string, string>, TResult> fetcher, string connectionName = null)
        {
            var obj = await HashGetAllAsync(hashId, connectionName);

            return fetcher(obj);
        }

        public async Task<IList<string>> HashKeysAsync(string hashId, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var res = await db.HashKeysAsync(hashId);
                return res.ToStringArray().ToList();
            });
        }

        public async Task<IList<TResult>> HashValuesAsync<TResult>(string hashId, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                var value = await db.HashValuesAsync(hashId);
                if (serializer != null)
                    return serializer.Deserialize<TResult>(value);
                return redisSerializer.Deserialize<TResult>(value);
            });
        }


        public async Task<bool> HashDeleteAsync(string hashId, string Key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                return await db.HashDeleteAsync(hashId, Key);
            });
        }

        public async Task<long> HashDeleteAsync(string hashId, string[] Key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Write, connectionName, async (db) =>
            {
                RedisValue[] redisKeys = new RedisValue[Key.Length];
                for (int i = 0; i < Key.Length; i++)
                {
                    redisKeys[i] = Key[i];
                }
                return await db.HashDeleteAsync(hashId, redisKeys);
            });
        }

        public async Task<bool> HashExistsAsync(string hashId, string Key, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                return await db.HashExistsAsync(hashId, Key);
            });
        }

        public async Task<long> HashLengthAsync(string hashId, string connectionName = null)
        {
            return await ExecuteCommand(ConnectTypeEnum.Read, connectionName, async (db) =>
            {
                return await db.HashLengthAsync(hashId);
            });
        }
    }
}
