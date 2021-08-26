using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Template.FreeSql.Persistence
{
    /// <summary>
    /// 租户数据库连接配置
    /// </summary>
    public class FreeSqlTenantDto
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public DataType? DbType { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 空闲时间(分)
        /// </summary>
        public int? IdleTime { get; set; }
    }
}
