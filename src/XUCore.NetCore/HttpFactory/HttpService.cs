using MessagePack.Resolvers;
using XUCore.Helpers;
using XUCore.Json;
using XUCore.Webs;
using XUCore.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace XUCore.NetCore.HttpFactory
{
    /// <summary>
    /// HttpRequestMessage服务类
    /// </summary>
    [Obsolete("已弃用该服务，请使用 AddHttpMessageService")]
    public class HttpService : IHttpService
    {
        public IHttpClientFactory HttpClientFactory { get; set; }
        public ILogger<HttpService> _logger { get; set; }

        public HttpService(ILogger<HttpService> logger, IHttpClientFactory HttpClientFactory)
        {
            this.HttpClientFactory = HttpClientFactory;
            this._logger = logger;
        }

        /// <summary>
        /// GET请求数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Get, null, HttpMediaType.Json, cancellationToken);

        /// <summary>
        /// GET请求数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="mediaType">mediaType数据格式，请求格式和返回格式一致（JSON、MessagePack）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(UrlArguments urlArguments, HttpMediaType mediaType, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Get, null, mediaType, cancellationToken);
        /// <summary>
        /// POST提交数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="postData">模型数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Post, postData, HttpMediaType.Json, cancellationToken);

        /// <summary>
        /// POST提交数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="postData">模型数据</param>
        /// <param name="mediaType">mediaType数据格式，请求格式和返回格式一致（JSON、MessagePack）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> PostAsync<T, TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType mediaType, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Post, postData, mediaType, cancellationToken);

        /// <summary>
        /// PUT提交数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="postData">模型数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> PutAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Put, postData, HttpMediaType.Json, cancellationToken);

        /// <summary>
        /// PUT提交数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="postData">模型数据</param>
        /// <param name="mediaType">mediaType数据格式，请求格式和返回格式一致（JSON、MessagePack）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> PutAsync<T, TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType mediaType, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Put, postData, mediaType, cancellationToken);

        /// <summary>
        /// PATCH提交数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="postData">模型数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> PatchAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Patch, postData, HttpMediaType.Json, cancellationToken);

        /// <summary>
        /// PATCH提交数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="postData">模型数据</param>
        /// <param name="mediaType">mediaType数据格式，请求格式和返回格式一致（JSON、MessagePack）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> PatchAsync<T, TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType mediaType, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T, TModel>(urlArguments, HttpMethod.Patch, postData, mediaType, cancellationToken);

        /// <summary>
        /// DELETE请求删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> DeleteAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Delete, null, HttpMediaType.Json, cancellationToken);

        /// <summary>
        /// DELETE请求删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="mediaType">mediaType数据格式，请求格式和返回格式一致（JSON、MessagePack）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> DeleteAsync<T>(UrlArguments urlArguments, HttpMediaType mediaType, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, HttpMethod.Delete, null, mediaType, cancellationToken);

        /// <summary>
        /// 发送请求数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="method">请求类型</param>
        /// <param name="postData">模型数据</param>
        /// <param name="mediaType">mediaType数据格式，请求格式和返回格式一致（JSON、MessagePack）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<T> HttpSendAsync<T, TModel>(UrlArguments urlArguments, HttpMethod method, TModel postData, HttpMediaType mediaType = HttpMediaType.Json, CancellationToken cancellationToken = default)
            where T : class, new()
            => await HttpSendAsync<T>(urlArguments, method, () => postData == null ? null : new StringContent(postData.ToJson(), Encoding.UTF8, "application/json"), mediaType, cancellationToken);

        /// <summary>
        /// 发送请求数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="method">请求类型</param>
        /// <param name="contentCall">HttpContent请求内容</param>
        /// <param name="mediaType">mediaType数据格式，请求格式和返回格式一致（JSON、MessagePack）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public virtual async Task<T> HttpSendAsync<T>(UrlArguments urlArguments, HttpMethod method, Func<HttpContent> contentCall, HttpMediaType mediaType = HttpMediaType.Json, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            HttpClient client = HttpClientFactory.CreateClient(string.IsNullOrEmpty(urlArguments.ClientName) ? "apiClient" : urlArguments.ClientName);

            string requestUrl = urlArguments.Complete().Url;

            string _mediaType = mediaType.Description();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            HttpResponseMessage responseMessage = null;

            if (client.BaseAddress == null)
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = new Uri(requestUrl)
                };

                foreach (var accept in client.DefaultRequestHeaders.Accept)
                    requestMessage.Headers.Accept.Add(accept);

                requestMessage.Headers.Authorization = client.DefaultRequestHeaders.Authorization;

                RequestHeaders(requestMessage.Headers);

                requestMessage.Content = contentCall?.Invoke();
                //if (requestMessage.Content != null)
                //    requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);

                responseMessage = await client.SendAsync(requestMessage, cancellationToken);
            }
            else
            {
                RequestHeaders(client.DefaultRequestHeaders);

                HttpContent content = contentCall?.Invoke();
                //if (content != null)
                //    content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);

                responseMessage = await SendAsync(client, requestUrl, method, content, cancellationToken);
            }

            switch (mediaType)
            {
                case HttpMediaType.MessagePack:
                    {
                        var res = await responseMessage.Content.ReadAsByteArrayAsync();

                        if (_logger.IsEnabled(LogLevel.Information))
                            _logger.LogInformation($"{client.BaseAddress}{requestUrl} MediaType：{_mediaType}，Method：{method.Method}，HttpMessage Read Byte Data Length：{res.Length}");

                        return res.ToMsgPackObject<T>();
                    }
                case HttpMediaType.MessagePackJackson:
                    {
                        var res = await responseMessage.Content.ReadAsStringAsync();

                        if (_logger.IsEnabled(LogLevel.Debug))
                            _logger.LogDebug($"{client.BaseAddress}{requestUrl} MediaType：{_mediaType}，Method：{method.Method}，HttpMessage Read Jackson Data：{res}");

                        return res.ToMsgPackBytesFromJson().ToMsgPackObject<T>();
                    }
                default:
                    {
                        var res = await responseMessage.Content.ReadAsStringAsync();

                        if (_logger.IsEnabled(LogLevel.Information))
                            _logger.LogInformation($"{client.BaseAddress}{requestUrl} MediaType：{_mediaType}，Method：{method.Method}，HttpMessage Read Json Data：{res}");

                        return res.ToObject<T>();
                    }
            }
        }

        private async Task<HttpResponseMessage> SendAsync(HttpClient client, string requestUrl, HttpMethod method, HttpContent content, CancellationToken cancellationToken)
        {
            switch (method.Method)
            {
                case "GET":
                    return await client.GetAsync(requestUrl, cancellationToken);
                case "POST":
                    return await client.PostAsync(requestUrl, content, cancellationToken);
                case "PUT":
                    return await client.PutAsync(requestUrl, content, cancellationToken);
                case "DELETE":
                    return await client.DeleteAsync(requestUrl, cancellationToken);
                case "PATCH":
                    return await client.PatchAsync(requestUrl, content, cancellationToken);
                default:
                    return await client.GetAsync(requestUrl, cancellationToken);
            }
        }

        /// <summary>
        /// 添加Headers消息头
        /// </summary>
        /// <param name="headers">header</param>
        public virtual void RequestHeaders(HttpRequestHeaders headers)
        {
            headers.Add("Client-IP", Web.IP);
        }
    }
}