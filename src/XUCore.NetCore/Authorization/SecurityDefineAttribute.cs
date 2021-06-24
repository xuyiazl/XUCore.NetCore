using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.Authorization
{
    /// <summary>
    /// 安全定义特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SecurityDefineAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SecurityDefineAttribute()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resourceId"></param>
        public SecurityDefineAttribute(string resourceId)
        {
            ResourceId = resourceId;
        }

        /// <summary>
        /// 资源Id，必须是唯一的
        /// </summary>
        public string ResourceId { get; set; }
    }
}
