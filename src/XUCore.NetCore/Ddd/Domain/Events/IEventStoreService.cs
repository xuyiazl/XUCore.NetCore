﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Ddd.Domain.Events
{
    /// <summary>
    /// 领域存储服务接口
    /// </summary>
    public interface IEventStoreService
    {
        /// <summary>
        /// 将命令模型进行保存
        /// </summary>
        /// <typeparam name="T"> 泛型：Event命令模型</typeparam>
        /// <param name="theEvent"></param>
        void Save<T>(T theEvent) where T : Event;
    }
}
