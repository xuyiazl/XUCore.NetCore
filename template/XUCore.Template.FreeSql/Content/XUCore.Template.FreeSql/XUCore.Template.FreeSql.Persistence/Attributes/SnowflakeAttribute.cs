using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Template.FreeSql.Persistence
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SnowflakeAttribute : Attribute
    {
        public bool Enable { get; set; } = true;
    }
}
