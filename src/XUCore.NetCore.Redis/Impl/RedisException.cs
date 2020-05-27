using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Redis
{
    /// <summary>
    /// redis组件异常
    /// </summary>
    public class RedisException : Exception
    {
        /// <summary>
        /// 异常的redis主机地址
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 异常的redis主机端口
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// 缓存服务器原始错误
        /// </summary>
        public string SourceErrorMessage { get; set; }
        /// <summary>
        /// Redis缓存自定义错误
        /// </summary>
        public string CustomErrorMessage { get; set; }

        public RedisException(string CustomeMsg)
            : base(string.Format("{0} 有可能是缓存服务器关闭了,尝试开启缓存服务以后刷新页面", CustomeMsg))
        {

        }
        public RedisException(string format, object[] args) : base(string.Format(format, args))
        {

        }

        public RedisException(string host, string port, string sourceErrorMessage, string customErrorMessage)
            : base(string.Format("地址{0}:{1}错误源信息:{2},自定义错误信息:{3}", host, port, sourceErrorMessage, customErrorMessage))
        {
            this.Host = host;
            this.Port = port;
            this.SourceErrorMessage = sourceErrorMessage;
            this.CustomErrorMessage = customErrorMessage;

        }
    }

    public static class RedisThrow
    {
        public static void NullSerializer(IRedisSerializer redisSerializer, IRedisSerializer serializer)
        {
            if (redisSerializer == null && serializer == null)
                throw new ArgumentNullException(nameof(IRedisSerializer), "请注入或者传入Redis序列化组件，并实现IRedisSerializer接口");
        }
    }
}
