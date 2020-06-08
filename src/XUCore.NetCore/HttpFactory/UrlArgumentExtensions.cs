using MessagePack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Serializer;
using XUCore.Webs;

namespace XUCore.NetCore.HttpFactory
{
    public static class UrlArgumentExtensions
    {
        /// <summary>
        /// 异步GET请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> GetAsync<TResult>(this UrlArguments urlArguments,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
        => await HttpRemote.Service.GetAsync<TResult>(urlArguments, mediaType, options, clientHandler, errorHandler, cancellationToken);
        /// <summary>
        /// 异步POST请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PostAsync<TModel, TResult>(this UrlArguments urlArguments, TModel model,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
        => await HttpRemote.Service.PostAsync<TModel, TResult>(urlArguments, model, mediaType, options, clientHandler, errorHandler, cancellationToken);
        /// <summary>
        /// 异步POST请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PostAsync<TResult>(this UrlArguments urlArguments, HttpContent content,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
        => await HttpRemote.Service.PostAsync<TResult>(urlArguments, content, mediaType, options, clientHandler, errorHandler, cancellationToken);
        /// <summary>
        /// 异步Put请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PutAsync<TModel, TResult>(this UrlArguments urlArguments, TModel model,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
        => await HttpRemote.Service.PutAsync<TModel, TResult>(urlArguments, model, mediaType, options, clientHandler, errorHandler, cancellationToken);
        /// <summary>
        /// 异步Put请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        public static async Task<TResult> PutAsync<TResult>(this UrlArguments urlArguments, HttpContent content,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
        => await HttpRemote.Service.PutAsync<TResult>(urlArguments, content, mediaType, options, clientHandler, errorHandler, cancellationToken);
        /// <summary>
        /// 异步Patch请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PatchAsync<TModel, TResult>(this UrlArguments urlArguments, TModel model,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
        => await HttpRemote.Service.PatchAsync<TModel, TResult>(urlArguments, model, mediaType, options, clientHandler, errorHandler, cancellationToken);
        /// <summary>
        /// 异步Patch请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        public static async Task<TResult> PatchAsync<TResult>(this UrlArguments urlArguments, HttpContent content,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
        => await HttpRemote.Service.PatchAsync<TResult>(urlArguments, content, mediaType, options, clientHandler, errorHandler, cancellationToken);
        /// <summary>
        /// 异步DELETE请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> DeleteAsync<TResult>(this UrlArguments urlArguments,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
        => await HttpRemote.Service.DeleteAsync<TResult>(urlArguments, mediaType, options, clientHandler, errorHandler, cancellationToken);
    }
}
