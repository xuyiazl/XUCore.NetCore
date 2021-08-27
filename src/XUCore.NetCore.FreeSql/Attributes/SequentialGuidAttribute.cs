using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.FreeSql
{
    /// <summary>
    /// 生成有序Guid
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SequentialGuidAttribute : Attribute
    {
        public bool Enable { get; set; } = true;
    }
}
