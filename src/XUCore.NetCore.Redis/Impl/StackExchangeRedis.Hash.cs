using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using XUCore.Extensions;
using XUCore.Serializer;
using XUCore.NetCore.Redis.RedisCommand;
using XUCore.Helpers;

namespace XUCore.NetCore.Redis
{
    public abstract partial class StackExchangeRedis : IHashRedisCommand
    {
        public bool HashSet<T>(string hashId, string key, T value, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
             {
                 if (serializer != null)
                     return db.HashSet(hashId, key, serializer.Serializer(value));
                 return db.HashSet(hashId, key, redisSerializer.Serializer(value));
             });
        }

        public bool HashSet<T>(string hashId, string key, T value, OverWrittenTypeDenum isAlways, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
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
                    return db.HashSet(hashId, key, serializer.Serializer(value), when);
                return db.HashSet(hashId, key, redisSerializer.Serializer(value), when);
            });
        }

        public TResult HashGetOrInsert<TResult>(string hashId, string key, Func<TResult> fetcher, int seconds = 0, string connectionRead = null, string connectionWrite = null,
            bool isCache = true, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            if (!isCache)
                return fetcher.Invoke();

            if (!HashExists(hashId, key, connectionRead))
            {
                var source = fetcher.Invoke();
                if (source != null)
                {
                    if (seconds > 0)
                    {
                        bool exists = KeyExists(hashId, connectionRead);

                        HashSet(hashId, key, source, connectionWrite, serializer);
                        if (!exists)
                        {
                            KeyExpire(hashId, seconds, connectionWrite);
                        }
                    }
                    else
                    {
                        HashSet(hashId, key, source, connectionWrite, serializer);
                    }
                }
                return source;
            }
            else
            {
                return HashGet<TResult>(hashId, key, connectionRead, serializer);
            }
        }

        public TResult HashGetOrInsert<T, TResult>(string hashId, string key, Func<T, TResult> fetcher, T t, int seconds = 0, string connectionRead = null, string connectionWrite = null,
            bool isCache = true, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            if (!isCache)
                return fetcher.Invoke(t);

            if (!HashExists(hashId, key, connectionRead))
            {
                var source = fetcher.Invoke(t);
                if (source != null)
                {
                    if (seconds > 0)
                    {
                        bool exists = KeyExists(hashId, connectionRead);
                        HashSet(hashId, key, source, connectionWrite, serializer);
                        if (!exists)
                        {
                            KeyExpire(hashId, seconds, connectionWrite);
                        }
                    }
                    else
                    {
                        HashSet(hashId, key, source, connectionWrite, serializer);
                    }
                }
                return source;
            }
            else
            {
                return HashGet<TResult>(hashId, key, connectionRead, serializer);
            }
        }

        public TResult HashGet<TResult>(string hashId, string key, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var res = db.HashGet(hashId, key);
                if (serializer != null)
                    return serializer.Deserialize<TResult>(res);
                return redisSerializer.Deserialize<TResult>(res);
            });
        }

        public IList<TResult> HashGet<TResult>(string hashId, string[] keys, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                List<RedisValue> listvalues = new List<RedisValue>();
                foreach (var key in keys)
                    listvalues.Add(key);

                var res = db.HashGet(hashId, listvalues.ToArray());

                if (serializer != null)
                    return serializer.Deserialize<TResult>(res);
                return redisSerializer.Deserialize<TResult>(res);
            });
        }

        public Dictionary<string, string> HashGetAll(string hashId, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var res = db.HashGetAll(hashId);

                return res.ToStringDictionary();
            });
        }

        public TResult HashGetAll<TResult>(string hashId, Func<Dictionary<string, string>, TResult> fetcher, string connectionName = null)
        {
            var obj = HashGetAll(hashId, connectionName);

            return fetcher(obj);
        }

        public IList<string> HashKeys(string hashId, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var res = db.HashKeys(hashId);
                return res.ToStringArray().ToList();
            });
        }

        public IList<TResult> HashValues<TResult>(string hashId, string connectionName = null, IRedisSerializer serializer = null)
        {
            RedisThrow.NullSerializer(redisSerializer, serializer);

            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                var value = db.HashValues(hashId);
                if (serializer != null)
                    return serializer.Deserialize<TResult>(value);
                return redisSerializer.Deserialize<TResult>(value);
            });
        }

        public bool HashDelete(string hashId, string Key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                return db.HashDelete(hashId, Key);
            });
        }

        public long HashDelete(string hashId, string[] Key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Write, connectionName, (db) =>
            {
                RedisValue[] redisKeys = new RedisValue[Key.Length];
                for (int i = 0; i < Key.Length; i++)
                {
                    redisKeys[i] = Key[i];
                }
                return db.HashDelete(hashId, redisKeys);
            });
        }

        public bool HashExists(string hashId, string Key, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                return db.HashExists(hashId, Key);
            });
        }

        public long HashLength(string hashId, string connectionName = null)
        {
            return ExecuteCommand(ConnectTypeEnum.Read, connectionName, (db) =>
            {
                return db.HashLength(hashId);
            });
        }
    }
}
