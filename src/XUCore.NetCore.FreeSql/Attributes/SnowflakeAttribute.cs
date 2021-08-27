using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.FreeSql
{
    /// <summary>
    /// 生成雪花id
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SnowflakeAttribute : Attribute
    {
        public bool Enable { get; set; } = true;
    }
}
