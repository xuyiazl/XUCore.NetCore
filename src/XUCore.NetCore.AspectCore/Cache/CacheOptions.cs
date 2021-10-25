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
        /// 启用缓存类型
        /// </summary>
        public CacheMode CacheMode { get; set; }
        /// <summary>
        /// 只读链接地址
        /// </summary>
        public string RedisRead { get; set; }
        /// <summary>
        /// 只写链接地址
        /// </summary>
        public string RedisWrite { get; set; }
    }

    public enum CacheMode
    {
        /// <summary>
        /// 内存缓存
        /// </summary>
        Memory,
        /// <summary>
        /// redis缓存
        /// </summary>
        Redis
    }
}
