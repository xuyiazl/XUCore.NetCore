    using MessagePack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XUCore.Serializer;

namespace XUCore.NetCore.HttpFactory
{
    /// <summary>
    /// 请求配置
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class HttpOptions<TResult> : IHttpOptions<TResult>
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        public static HttpOptions<TResult> Default
        {
            get
            {
                return new HttpOptions<TResult>();
            }
        }
        /// <summary>
        /// 请求返回的数据类型<see cref="HttpMediaType"/>
        /// </summary>
        public HttpMediaType MediaType { get; set; } = HttpMediaType.Json;
        /// <summary>
        /// 序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置
        /// </summary>
        public MessagePackSerializerOptions SerializerOptions { get; set; } = MessagePackSerializerResolver.UnixDateTimeOptions;
        /// <summary>
        /// <see cref="HttpClient"/>回调，可以添加需要的Header等
        /// </summary>
        public Action<HttpClient> ClientHandler { get; set; } = null;
        /// <summary>
        /// <see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型
        /// </summary>
        public Func<HttpResponseMessage, Task<TResult>> ErrorHandler { get; set; } = null;
    }
}
