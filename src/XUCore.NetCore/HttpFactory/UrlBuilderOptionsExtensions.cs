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
    public static partial class UrlBuilderExtensions
    {
        /// <summary>
        /// 异步GET请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> GetAsync<TResult>(this UrlBuilder urlBuilder, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        => await HttpRemote.Service.GetAsync<TResult>(urlBuilder, options, cancellationToken);
        /// <summary>
        /// 异步POST请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PostAsync<TModel, TResult>(this UrlBuilder urlBuilder, TModel model, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        => await HttpRemote.Service.PostAsync<TModel, TResult>(urlBuilder, model, options, cancellationToken);
        /// <summary>
        /// 异步POST请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PostAsync<TResult>(this UrlBuilder urlBuilder, HttpContent content, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        => await HttpRemote.Service.PostAsync<TResult>(urlBuilder, content, options, cancellationToken);
        /// <summary>
        /// 异步Put请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PutAsync<TModel, TResult>(this UrlBuilder urlBuilder, TModel model, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        => await HttpRemote.Service.PutAsync<TModel, TResult>(urlBuilder, model, options, cancellationToken);
        /// <summary>
        /// 异步Put请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        public static async Task<TResult> PutAsync<TResult>(this UrlBuilder urlBuilder, HttpContent content, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        => await HttpRemote.Service.PutAsync<TResult>(urlBuilder, content, options, cancellationToken);
        /// <summary>
        /// 异步Patch请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PatchAsync<TModel, TResult>(this UrlBuilder urlBuilder, TModel model, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        => await HttpRemote.Service.PatchAsync<TModel, TResult>(urlBuilder, model, options, cancellationToken);
        /// <summary>
        /// 异步Patch请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        public static async Task<TResult> PatchAsync<TResult>(this UrlBuilder urlBuilder, HttpContent content, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        => await HttpRemote.Service.PatchAsync<TResult>(urlBuilder, content, options, cancellationToken);
        /// <summary>
        /// 异步DELETE请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> DeleteAsync<TResult>(this UrlBuilder urlBuilder, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        => await HttpRemote.Service.DeleteAsync<TResult>(urlBuilder, options, cancellationToken);
    }
}
