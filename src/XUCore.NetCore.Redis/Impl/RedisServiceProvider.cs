using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Redis
{
    /// <summary>
    /// 缓存操作仓库实现
    /// </summary>
    public class RedisServiceProvider : StackExchangeRedisProvider, IRedisService
    {
        public RedisServiceProvider(IConfiguration configuration, IRedisSerializer redisSerializer)
            : base(configuration, redisSerializer)
        {
        }
    }
}
