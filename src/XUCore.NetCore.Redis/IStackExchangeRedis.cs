using System;
using System.Collections.Generic;
using System.Text;
using XUCore.NetCore.Redis.RedisCommand;
using StackExchange.Redis;

namespace XUCore.NetCore.Redis
{
    /// <summary>
    /// 定义功能操作部分
    /// 详细命令参考:http://redisdoc.com/
    /// </summary>
    public interface IStackExchangeRedis :
        IKeyRedisCommand, IStringRedisCommand, IHashRedisCommand, ISetRedisCommand, ISortSetRedisCommand, IListRedisCommand,
        IKeyRedisCommandAsync, IStringRedisCommandAsync, IHashRedisCommandAsync, ISetRedisCommandAsync, ISortSetRedisCommandAsync, IListRedisCommandAsync,
        ILockRedisCommand, ILockRedisCommandAsync
    {
        /// <summary>
        /// 查询返回IDataBase
        /// </summary>
        /// <param name="connectTypeEnum"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        IDatabase QueryDataBase(ConnectTypeEnum connectTypeEnum, string connectionName = null);
        /// <summary>
        /// 查询返回ISubscriber
        /// 消息中间件使用
        /// </summary>
        /// <param name="connectTypeEnum"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        ISubscriber QuerySubscriber(ConnectTypeEnum connectTypeEnum, string connectionName = null);
        /// <summary>
        /// 查询返回ServerCounters
        /// </summary>
        /// <param name="connectTypeEnum"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        ServerCounters QueryServerCounters(ConnectTypeEnum connectTypeEnum, string connectionName = null);
        /// <summary>
        /// 获得连接器
        /// </summary>
        /// <param name="connectTypeEnum"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        ConnectionMultiplexer QueryMultiplexer(ConnectTypeEnum connectTypeEnum, string connectionName = null);
    }
}
