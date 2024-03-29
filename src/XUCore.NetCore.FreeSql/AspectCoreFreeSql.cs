﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.FreeSql
{
    /// <summary>
    /// 解决AspectCore冲突问题，原因：作者表示不支持不带namespace的接口和类的注入
    /// </summary>
    public class AspectCoreFreeSql
    {
        public IFreeSql Orm { get; set; }
    }
    /// <summary>
    /// 解决AspectCore冲突问题，原因：作者表示不支持不带namespace的接口和类的注入
    /// </summary>
    /// <typeparam name="TMark"></typeparam>
    public class AspectCoreFreeSql<TMark>
    {
        public IFreeSql<TMark> Orm { get; set; }
    }
    /// <summary>
    /// 解决AspectCore冲突问题，原因：作者表示不支持不带namespace的接口和类的注入
    /// </summary>
    public class AspectCoreIdleBus
    {
        /// <summary>
        /// 容器管理器
        /// </summary>
        public IdleBus<IFreeSql> IBus { get; set; }

        /// <summary>
        /// 从容器管理中，获取FreeSql实例，此处需要重写容器创建和获取逻辑
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public virtual IFreeSql GetOrCreateFreeSql(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IFreeSql>();
        }
    }
}
