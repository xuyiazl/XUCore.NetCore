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
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> GetAsync<TResult>(this IHttpService httpMessageService, UrlArguments urlArguments, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        {
            options ??= HttpOptions<TResult>.Default;

            var client = httpMessageService.CreateClient(urlArguments.ClientName)
                .SetHeaderAccept(options.MediaType);

            if (options.ClientHandler != null)
                options.ClientHandler.Invoke(client);

            var responseMessage = await client.GetAsync(urlArguments, cancellationToken);

            if (!responseMessage.IsSuccessStatusCode)
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(responseMessage);
            else
                return await responseMessage.Content.ReadAsAsync<TResult>(options.MediaType, options.SerializerOptions);
        }

        /// <summary>
        /// 异步POST请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PostAsync<TModel, TResult>(this IHttpService httpMessageService, UrlArguments urlArguments, TModel model, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        {
            options ??= HttpOptions<TResult>.Default;

            var client = httpMessageService.CreateClient(urlArguments.ClientName)
                .SetHeaderAccept(options.MediaType);

            if (options.ClientHandler != null)
                options.ClientHandler.Invoke(client);

            var responseMessage = await client.PostAsync(urlArguments, model, cancellationToken);

            if (!responseMessage.IsSuccessStatusCode)
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(responseMessage);
            else
                return await responseMessage.Content.ReadAsAsync<TResult>(options.MediaType, options.SerializerOptions);
        }

        /// <summary>
        /// 异步POST请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PostAsync<TResult>(this IHttpService httpMessageService, UrlArguments urlArguments, HttpContent content, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        {
            options ??= HttpOptions<TResult>.Default;

            var client = httpMessageService.CreateClient(urlArguments.ClientName)
                .SetHeaderAccept(options.MediaType);

            if (options.ClientHandler != null)
                options.ClientHandler.Invoke(client);

            var responseMessage = await client.PostAsync(urlArguments, content, cancellationToken);

            if (!responseMessage.IsSuccessStatusCode)
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(responseMessage);
            else
                return await responseMessage.Content.ReadAsAsync<TResult>(options.MediaType, options.SerializerOptions);
        }

        /// <summary>
        /// 异步Put请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PutAsync<TModel, TResult>(this IHttpService httpMessageService, UrlArguments urlArguments, TModel model, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        {
            options ??= HttpOptions<TResult>.Default;

            var client = httpMessageService.CreateClient(urlArguments.ClientName)
                .SetHeaderAccept(options.MediaType);

            if (options.ClientHandler != null)
                options.ClientHandler.Invoke(client);

            var responseMessage = await client.PutAsync(urlArguments, model, cancellationToken);

            if (!responseMessage.IsSuccessStatusCode)
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(responseMessage);
            else
                return await responseMessage.Content.ReadAsAsync<TResult>(options.MediaType, options.SerializerOptions);
        }

        /// <summary>
        /// 异步Put请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        public static async Task<TResult> PutAsync<TResult>(this IHttpService httpMessageService, UrlArguments urlArguments, HttpContent content, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        {
            options ??= HttpOptions<TResult>.Default;

            var client = httpMessageService.CreateClient(urlArguments.ClientName)
                .SetHeaderAccept(options.MediaType);

            if (options.ClientHandler != null)
                options.ClientHandler.Invoke(client);

            var responseMessage = await client.PutAsync(urlArguments, content, cancellationToken);

            if (!responseMessage.IsSuccessStatusCode)
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(responseMessage);
            else
                return await responseMessage.Content.ReadAsAsync<TResult>(options.MediaType, options.SerializerOptions);
        }

        /// <summary>
        /// 异步Patch请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PatchAsync<TModel, TResult>(this IHttpService httpMessageService, UrlArguments urlArguments, TModel model, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        {
            options ??= HttpOptions<TResult>.Default;

            var client = httpMessageService.CreateClient(urlArguments.ClientName)
                .SetHeaderAccept(options.MediaType);

            if (options.ClientHandler != null)
                options.ClientHandler.Invoke(client);

            var responseMessage = await client.PatchAsync(urlArguments, model, cancellationToken);

            if (!responseMessage.IsSuccessStatusCode)
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(responseMessage);
            else
                return await responseMessage.Content.ReadAsAsync<TResult>(options.MediaType, options.SerializerOptions);
        }

        /// <summary>
        /// 异步Patch请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        public static async Task<TResult> PatchAsync<TResult>(this IHttpService httpMessageService, UrlArguments urlArguments, HttpContent content, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        {
            options ??= HttpOptions<TResult>.Default;

            var client = httpMessageService.CreateClient(urlArguments.ClientName)
                .SetHeaderAccept(options.MediaType);

            if (options.ClientHandler != null)
                options.ClientHandler.Invoke(client);

            var responseMessage = await client.PatchAsync(urlArguments, content, cancellationToken);

            if (!responseMessage.IsSuccessStatusCode)
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(responseMessage);
            else
                return await responseMessage.Content.ReadAsAsync<TResult>(options.MediaType, options.SerializerOptions);
        }

        /// <summary>
        /// 异步DELETE请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> DeleteAsync<TResult>(this IHttpService httpMessageService, UrlArguments urlArguments, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        {
            options ??= HttpOptions<TResult>.Default;

            var client = httpMessageService.CreateClient(urlArguments.ClientName)
                .SetHeaderAccept(options.MediaType);

            if (options.ClientHandler != null)
                options.ClientHandler.Invoke(client);

            var responseMessage = await client.DeleteAsync(urlArguments, cancellationToken);

            if (!responseMessage.IsSuccessStatusCode)
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(responseMessage);
            else
                return await responseMessage.Content.ReadAsAsync<TResult>(options.MediaType, options.SerializerOptions);
        }
    }
}
