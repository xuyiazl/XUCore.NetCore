using XUCore.Webs;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.HttpFactory
{
    /// <summary>
    /// HttpRequestMessage服务类
    /// </summary>
    public interface IHttpService
    {
        IHttpClientFactory HttpClientFactory { get; set; }
        /// <summary>
        /// GET请求数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
          where T : class, new();
        /// <summary>
        /// GET请求数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="mediaType">mediaType数据格式，请求格式和返回格式一致（JSON、MessagePack）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(UrlArguments urlArguments, HttpMediaType mediaType, CancellationToken cancellationToken = default)
          where T : class, new();
        /// <summary>
        /// POST提交数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="postData">模型数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<T> PostAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
          where T : class, new();
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
        Task<T> PostAsync<T, TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType mediaType, CancellationToken cancellationToken = default)
          where T : class, new();
        /// <summary>
        /// PUT提交数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="postData">模型数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<T> PutAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
          where T : class, new();
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
        Task<T> PutAsync<T, TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType mediaType, CancellationToken cancellationToken = default)
          where T : class, new();
        /// <summary>
        /// PATCH提交数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="postData">模型数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<T> PatchAsync<T, TModel>(UrlArguments urlArguments, TModel postData, CancellationToken cancellationToken = default)
          where T : class, new();
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
        Task<T> PatchAsync<T, TModel>(UrlArguments urlArguments, TModel postData, HttpMediaType mediaType, CancellationToken cancellationToken = default)
          where T : class, new();
        /// <summary>
        /// DELETE请求删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<T> DeleteAsync<T>(UrlArguments urlArguments, CancellationToken cancellationToken = default)
          where T : class, new();
        /// <summary>
        /// DELETE请求删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlArguments">Url构造器</param>
        /// <param name="mediaType">mediaType数据格式，请求格式和返回格式一致（JSON、MessagePack）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        Task<T> DeleteAsync<T>(UrlArguments urlArguments, HttpMediaType mediaType, CancellationToken cancellationToken = default)
          where T : class, new();
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
        Task<T> HttpSendAsync<T, TModel>(UrlArguments urlArguments, HttpMethod method, TModel postData, HttpMediaType mediaType = HttpMediaType.Json, CancellationToken cancellationToken = default)
          where T : class, new();
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
        Task<T> HttpSendAsync<T>(UrlArguments urlArguments, HttpMethod method, Func<HttpContent> contentCall, HttpMediaType mediaType = HttpMediaType.Json, CancellationToken cancellationToken = default)
          where T : class, new();
    }
}