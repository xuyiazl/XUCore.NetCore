using MessagePack;
using XUCore.Json;
using XUCore.Webs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.HttpFactory
{
    public static class IHttpServiceExtensions
    {
        /// <summary>
        /// 异步GET请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> GetAsync<TResult>(this IHttpService httpMessageService, UrlArguments urlArguments,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
        {
            options = options ?? MessagePackSerializerResolver.DateTimeOptions;

            var client = httpMessageService.CreateClient(urlArguments.ClientName)
                .SetHeaderAccept(mediaType);

            if (clientHandler != null)
                clientHandler.Invoke(client);

            var responseMessage = await client.GetAsync(urlArguments, cancellationToken);

            //if (!responseMessage.IsSuccessStatusCode)
            //{
            //    if (errorHandler == null)
            //    {
            //        var errorMessage = await responseMessage.Content.ReadAsAsync<ReturnModel<string>>(mediaType, options);

            //        return new TResult
            //        {
            //            code = errorMessage.code,
            //            message = errorMessage.message,
            //            subCode = errorMessage.subCode,
            //            requestTime = errorMessage.requestTime,
            //            bodyMessage = default
            //        };
            //    }
            //    else
            //    {
            //        return await errorHandler.Invoke(responseMessage);
            //    }
            //}

            if (!responseMessage.IsSuccessStatusCode)
                return errorHandler == null ? default : await errorHandler.Invoke(responseMessage);
            else
                return await responseMessage.Content.ReadAsAsync<TResult>(mediaType, options);
        }

        /// <summary>
        /// 异步POST请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PostAsync<TModel, TResult>(this IHttpService httpMessageService, UrlArguments urlArguments, TModel model,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
        {
            options = options ?? MessagePackSerializerResolver.DateTimeOptions;

            var client = httpMessageService.CreateClient(urlArguments.ClientName)
                .SetHeaderAccept(mediaType);

            if (clientHandler != null)
                clientHandler.Invoke(client);

            var responseMessage = await client.PostAsync(urlArguments, model, cancellationToken);

            //if (!responseMessage.IsSuccessStatusCode)
            //{
            //    if (errorHandler == null)
            //    {
            //        var errorMessage = await responseMessage.Content.ReadAsAsync<ReturnModel<string>>(mediaType, options);

            //        return new TResult
            //        {
            //            code = errorMessage.code,
            //            message = errorMessage.message,
            //            subCode = errorMessage.subCode,
            //            requestTime = errorMessage.requestTime,
            //            bodyMessage = default
            //        };
            //    }
            //    else
            //    {
            //        return await errorHandler.Invoke(responseMessage);
            //    }
            //}

            if (!responseMessage.IsSuccessStatusCode)
                return errorHandler == null ? default : await errorHandler.Invoke(responseMessage);
            else
                return await responseMessage.Content.ReadAsAsync<TResult>(mediaType, options);
        }

        /// <summary>
        /// 异步POST请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PostAsync<TResult>(this IHttpService httpMessageService, UrlArguments urlArguments, HttpContent content,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
        {
            options = options ?? MessagePackSerializerResolver.DateTimeOptions;

            var client = httpMessageService.CreateClient(urlArguments.ClientName)
                .SetHeaderAccept(mediaType);

            if (clientHandler != null)
                clientHandler.Invoke(client);

            var responseMessage = await client.PostAsync(urlArguments, content, cancellationToken);

            //if (!responseMessage.IsSuccessStatusCode)
            //{
            //    if (errorHandler == null)
            //    {
            //        var errorMessage = await responseMessage.Content.ReadAsAsync<ReturnModel<string>>(mediaType, options);

            //        return new TResult
            //        {
            //            code = errorMessage.code,
            //            message = errorMessage.message,
            //            subCode = errorMessage.subCode,
            //            requestTime = errorMessage.requestTime,
            //            bodyMessage = default
            //        };
            //    }
            //    else
            //    {
            //        return await errorHandler.Invoke(responseMessage);
            //    }
            //}

            if (!responseMessage.IsSuccessStatusCode)
                return errorHandler == null ? default : await errorHandler.Invoke(responseMessage);
            else
                return await responseMessage.Content.ReadAsAsync<TResult>(mediaType, options);
        }

        /// <summary>
        /// 异步Put请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PutAsync<TModel, TResult>(this IHttpService httpMessageService, UrlArguments urlArguments, TModel model,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
        {
            options = options ?? MessagePackSerializerResolver.DateTimeOptions;

            var client = httpMessageService.CreateClient(urlArguments.ClientName)
                .SetHeaderAccept(mediaType);

            if (clientHandler != null)
                clientHandler.Invoke(client);

            var responseMessage = await client.PutAsync(urlArguments, model, cancellationToken);

            //if (!responseMessage.IsSuccessStatusCode)
            //{
            //    if (errorHandler == null)
            //    {
            //        var errorMessage = await responseMessage.Content.ReadAsAsync<ReturnModel<string>>(mediaType, options);

            //        return new TResult
            //        {
            //            code = errorMessage.code,
            //            message = errorMessage.message,
            //            subCode = errorMessage.subCode,
            //            requestTime = errorMessage.requestTime,
            //            bodyMessage = default
            //        };
            //    }
            //    else
            //    {
            //        return await errorHandler.Invoke(responseMessage);
            //    }
            //}

            if (!responseMessage.IsSuccessStatusCode)
                return errorHandler == null ? default : await errorHandler.Invoke(responseMessage);
            else
                return await responseMessage.Content.ReadAsAsync<TResult>(mediaType, options);
        }

        /// <summary>
        /// 异步Put请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        public static async Task<TResult> PutAsync<TResult>(this IHttpService httpMessageService, UrlArguments urlArguments, HttpContent content,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
        {
            options = options ?? MessagePackSerializerResolver.DateTimeOptions;

            var client = httpMessageService.CreateClient(urlArguments.ClientName)
                .SetHeaderAccept(mediaType);

            if (clientHandler != null)
                clientHandler.Invoke(client);

            var responseMessage = await client.PutAsync(urlArguments, content, cancellationToken);

            //if (!responseMessage.IsSuccessStatusCode)
            //{
            //    if (errorHandler == null)
            //    {
            //        var errorMessage = await responseMessage.Content.ReadAsAsync<ReturnModel<string>>(mediaType, options);

            //        return new TResult
            //        {
            //            code = errorMessage.code,
            //            message = errorMessage.message,
            //            subCode = errorMessage.subCode,
            //            requestTime = errorMessage.requestTime,
            //            bodyMessage = default
            //        };
            //    }
            //    else
            //    {
            //        return await errorHandler.Invoke(responseMessage);
            //    }
            //}

            if (!responseMessage.IsSuccessStatusCode)
                return errorHandler == null ? default : await errorHandler.Invoke(responseMessage);
            else
                return await responseMessage.Content.ReadAsAsync<TResult>(mediaType, options);
        }

        /// <summary>
        /// 异步Patch请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PatchAsync<TModel, TResult>(this IHttpService httpMessageService, UrlArguments urlArguments, TModel model,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
        {
            options = options ?? MessagePackSerializerResolver.DateTimeOptions;

            var client = httpMessageService.CreateClient(urlArguments.ClientName)
                .SetHeaderAccept(mediaType);

            if (clientHandler != null)
                clientHandler.Invoke(client);

            var responseMessage = await client.PatchAsync(urlArguments, model, cancellationToken);

            //if (!responseMessage.IsSuccessStatusCode)
            //{
            //    if (errorHandler == null)
            //    {
            //        var errorMessage = await responseMessage.Content.ReadAsAsync<ReturnModel<string>>(mediaType, options);

            //        return new TResult
            //        {
            //            code = errorMessage.code,
            //            message = errorMessage.message,
            //            subCode = errorMessage.subCode,
            //            requestTime = errorMessage.requestTime,
            //            bodyMessage = default
            //        };
            //    }
            //    else
            //    {
            //        return await errorHandler.Invoke(responseMessage);
            //    }
            //}

            if (!responseMessage.IsSuccessStatusCode)
                return errorHandler == null ? default : await errorHandler.Invoke(responseMessage);
            else
                return await responseMessage.Content.ReadAsAsync<TResult>(mediaType, options);
        }

        /// <summary>
        /// 异步Patch请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        public static async Task<TResult> PatchAsync<TResult>(this IHttpService httpMessageService, UrlArguments urlArguments, HttpContent content,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
        {
            options = options ?? MessagePackSerializerResolver.DateTimeOptions;

            var client = httpMessageService.CreateClient(urlArguments.ClientName)
                .SetHeaderAccept(mediaType);

            if (clientHandler != null)
                clientHandler.Invoke(client);

            var responseMessage = await client.PatchAsync(urlArguments, content, cancellationToken);

            //if (!responseMessage.IsSuccessStatusCode)
            //{
            //    if (errorHandler == null)
            //    {
            //        var errorMessage = await responseMessage.Content.ReadAsAsync<ReturnModel<string>>(mediaType, options);

            //        return new TResult
            //        {
            //            code = errorMessage.code,
            //            message = errorMessage.message,
            //            subCode = errorMessage.subCode,
            //            requestTime = errorMessage.requestTime,
            //            bodyMessage = default
            //        };
            //    }
            //    else
            //    {
            //        return await errorHandler.Invoke(responseMessage);
            //    }
            //}

            if (!responseMessage.IsSuccessStatusCode)
                return errorHandler == null ? default : await errorHandler.Invoke(responseMessage);
            else
                return await responseMessage.Content.ReadAsAsync<TResult>(mediaType, options);
        }

        /// <summary>
        /// 异步DELETE请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlArguments"><see cref="UrlArguments"/>Url构造器</param>
        /// <param name="mediaType">请求返回的数据类型<see cref="HttpMediaType"/></param>
        /// <param name="options">序列化方式<see cref="MessagePackSerializerResolver"/>，如果是JSON不需要设置</param>
        /// <param name="clientHandler"><see cref="HttpClient"/>回调，可以添加需要的Header等</param>
        /// <param name="errorHandler"><see cref="HttpResponseMessage"/>请求异常处理，默认情况返回错误的Return模型</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> DeleteAsync<TResult>(this IHttpService httpMessageService, UrlArguments urlArguments,
            HttpMediaType mediaType = HttpMediaType.Json, MessagePackSerializerOptions options = null, Action<HttpClient> clientHandler = null, Func<HttpResponseMessage, Task<TResult>> errorHandler = null, CancellationToken cancellationToken = default)
        {
            options = options ?? MessagePackSerializerResolver.DateTimeOptions;

            var client = httpMessageService.CreateClient(urlArguments.ClientName)
                .SetHeaderAccept(mediaType);

            if (clientHandler != null)
                clientHandler.Invoke(client);

            var responseMessage = await client.DeleteAsync(urlArguments, cancellationToken);

            //if (!responseMessage.IsSuccessStatusCode)
            //{
            //    if (errorHandler == null)
            //    {
            //        var errorMessage = await responseMessage.Content.ReadAsAsync<ReturnModel<string>>(mediaType, options);

            //        return new TResult
            //        {
            //            code = errorMessage.code,
            //            message = errorMessage.message,
            //            subCode = errorMessage.subCode,
            //            requestTime = errorMessage.requestTime,
            //            bodyMessage = default
            //        };
            //    }
            //    else
            //    {
            //        return await errorHandler.Invoke(responseMessage);
            //    }
            //}

            if (!responseMessage.IsSuccessStatusCode)
                return errorHandler == null ? default : await errorHandler.Invoke(responseMessage);
            else
                return await responseMessage.Content.ReadAsAsync<TResult>(mediaType, options);
        }
    }
}
