using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.AspectCore.Cache
{
    public class CacheOptions
    {
        /// <summary>
        /// 只读链接地址
        /// </summary>
        public string RedisRead { get; set; }
        /// <summary>
        /// 只写链接地址
        /// </summary>
        public string RedisWrite { get; set; }
    }
}
