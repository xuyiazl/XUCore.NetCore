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
    public interface IHttpOptions<TResult>
    {
        /// <summary>
        /// <see cref="HttpClient"/>回调，可以添加需要的Header等
        /// </summary>
        Action<HttpClient> ClientHandler { get; set; }
        /// <summary>
        /// <see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型
        /// </summary>
        Func<HttpResponseMessage, Task<TResult>> ErrorHandler { get; set; }
        /// <summary>
        /// 请求返回的数据类型<see cref="HttpMediaType"/>
        /// </summary>
        HttpMediaType MediaType { get; set; }
        /// <summary>
        /// 序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置
        /// </summary>
        MessagePackSerializerOptions SerializerOptions { get; set; }
    }
}
