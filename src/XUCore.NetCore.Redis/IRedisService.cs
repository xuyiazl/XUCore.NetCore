using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Redis
{
    /// <summary>
    /// 定义redis连接服务
    /// XUCore.NetCore.Redis.IStackExchangeRedis 已经定义了一些操作
    /// </summary>
    public interface IRedisService : IStackExchangeRedis
    {
        //如果有需要扩充需求可以在此处添加,XUCore.NetCore.Redis.IStackExchangeRedis中已经包含一些必要的操作了
    }
}
