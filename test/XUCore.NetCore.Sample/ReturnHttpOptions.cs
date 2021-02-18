using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.NetCore.HttpFactory;
using XUCore.Serializer;

namespace XUCore.NetCore.Sample
{
    /// <summary>
    /// 签名策略
    /// </summary>
    public interface IHttpSignPolicy
    {

    }
    /// <summary>
    /// Http请求签名策略，demo
    /// </summary>
    public class ReturnHttpOptions<TResult> : HttpOptions<TResult>, IReturnHttpOptions<TResult>, IHttpSignPolicy
    {
        public ReturnHttpOptions(ILogger<ReturnHttpOptions<TResult>> logger, IHostEnvironment env)
        {
            MediaType = HttpMediaType.Json;
            SerializerOptions = MessagePackSerializerResolver.DateTimeOptions;

            if (!env.EnvironmentName.ToLower().Equals("release"))
                ElapsedTimeHandler = (url, time) => Console.WriteLine($"{url}，执行时间:{time.TotalMilliseconds}ms");

            ClientHandler = client =>
            {
                //可以在这里处理签名

                //securityRequest.SetSecurity(client, gateWayOptions.AppId, gateWayOptions.AppKey);
            };
            ErrorHandler = async (responseMessage) =>
            {
                var errorMessage = await responseMessage.Content.ReadAsStringAsync();

                logger.LogError($"远程请求，HttpStatus：{responseMessage?.StatusCode}，Error ：{errorMessage}...");

                return default;
            };
        }
    }
    /// <summary>
    /// Http请求签名策略，demo
    /// </summary>
    public interface IReturnHttpOptions<TResult> : IHttpOptions<TResult>, IHttpSignPolicy
    {

    }
}
