using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Redis
{
    /// <summary>
    /// Redis的组件服务提供
    /// </summary>
    public class StackExchangeRedisProvider : StackExchangeRedis, IStackExchangeRedis
    {
        public StackExchangeRedisProvider(IConfiguration configuration, IRedisSerializer redisSerializer) : base(configuration, redisSerializer)
        {
            // 读取配置文件中的Redis字符串信息
            if (connMultiplexer == null)
            {
                var config = new List<StackExchangeConnectionSettings>();
                configuration.GetSection("StackExchangeConnectionSettings").Bind(config);

                connMultiplexer = config;
            }
        }
    }
}
