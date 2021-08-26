﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Template.FreeSql.Persistence
{
    /// <summary>
    /// 数据隔离类型
    /// </summary>
    public enum DataIsolationType
    {
        /// <summary>
        /// 独立数据库
        /// </summary>
        [Description("独立数据库")]
        OwnDb = 1,

        /// <summary>
        /// 独立数据表
        /// </summary>
        [Description("独立数据表")]
        OwnDt = 2,

        /// <summary>
        /// 共享数据库，独立架构
        /// </summary>
        [Description("独立架构")]
        Schema = 3,

        /// <summary>
        /// 共享数据库
        /// </summary>
        [Description("共享数据库")]
        Share = 4
    }
}
