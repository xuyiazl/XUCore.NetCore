using MessagePack;
using XUCore.Serializer;
using XUCore.Webs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.HttpFactory
{
    public static partial class IHttpServiceExtensions
    {
        /// <summary>
        /// 异步GET请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> GetAsync<TResult>(this IHttpService httpMessageService, UrlBuilder urlBuilder,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
            => await httpMessageService.GetAsync<TResult>(urlBuilder, new HttpOptions<TResult>
            {
                MediaType = mediaType,
                SerializerOptions = options,
                ClientHandler = clientHandler,
                ErrorHandler = errorHandler
            }, cancellationToken);

        /// <summary>
        /// 异步POST请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PostAsync<TModel, TResult>(this IHttpService httpMessageService, UrlBuilder urlBuilder, TModel model,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
            => await httpMessageService.PostAsync<TModel, TResult>(urlBuilder, model, new HttpOptions<TResult>
            {
                MediaType = mediaType,
                SerializerOptions = options,
                ClientHandler = clientHandler,
                ErrorHandler = errorHandler
            }, cancellationToken);

        /// <summary>
        /// 异步POST请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PostAsync<TResult>(this IHttpService httpMessageService, UrlBuilder urlBuilder, HttpContent content,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
            => await httpMessageService.PostAsync<TResult>(urlBuilder, content, new HttpOptions<TResult>
            {
                MediaType = mediaType,
                SerializerOptions = options,
                ClientHandler = clientHandler,
                ErrorHandler = errorHandler
            }, cancellationToken);

        /// <summary>
        /// 异步Put请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PutAsync<TModel, TResult>(this IHttpService httpMessageService, UrlBuilder urlBuilder, TModel model,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
            => await httpMessageService.PutAsync<TModel, TResult>(urlBuilder, model, new HttpOptions<TResult>
            {
                MediaType = mediaType,
                SerializerOptions = options,
                ClientHandler = clientHandler,
                ErrorHandler = errorHandler
            }, cancellationToken);

        /// <summary>
        /// 异步Put请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        public static async Task<TResult> PutAsync<TResult>(this IHttpService httpMessageService, UrlBuilder urlBuilder, HttpContent content,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
             => await httpMessageService.PutAsync<TResult>(urlBuilder, content, new HttpOptions<TResult>
             {
                 MediaType = mediaType,
                 SerializerOptions = options,
                 ClientHandler = clientHandler,
                 ErrorHandler = errorHandler
             }, cancellationToken);

        /// <summary>
        /// 异步Patch请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PatchAsync<TModel, TResult>(this IHttpService httpMessageService, UrlBuilder urlBuilder, TModel model,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
            => await httpMessageService.PatchAsync<TModel, TResult>(urlBuilder, model, new HttpOptions<TResult>
            {
                MediaType = mediaType,
                SerializerOptions = options,
                ClientHandler = clientHandler,
                ErrorHandler = errorHandler
            }, cancellationToken);

        /// <summary>
        /// 异步Patch请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        public static async Task<TResult> PatchAsync<TResult>(this IHttpService httpMessageService, UrlBuilder urlBuilder, HttpContent content,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
            => await httpMessageService.PatchAsync<TResult>(urlBuilder, content, new HttpOptions<TResult>
            {
                MediaType = mediaType,
                SerializerOptions = options,
                ClientHandler = clientHandler,
                ErrorHandler = errorHandler
            }, cancellationToken);

        /// <summary>
        /// 异步DELETE请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> DeleteAsync<TResult>(this IHttpService httpMessageService, UrlBuilder urlBuilder,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
            => await httpMessageService.DeleteAsync<TResult>(urlBuilder, new HttpOptions<TResult>
            {
                MediaType = mediaType,
                SerializerOptions = options,
                ClientHandler = clientHandler,
                ErrorHandler = errorHandler
            }, cancellationToken);
    }
}
