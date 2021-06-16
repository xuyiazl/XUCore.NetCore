using Polly.Timeout;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Webs;

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
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> GetAsync<TResult>(this IHttpService httpMessageService, UrlBuilder urlBuilder, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        {
            options ??= HttpOptions<TResult>.Default;

            var client = httpMessageService.CreateClient(urlBuilder.ClientName)
                .SetAccept(options.MediaType);

            if (options.ClientHandler != null)
                options.ClientHandler.Invoke(client);
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                var responseMessage = await client.GetAsync(urlBuilder, cancellationToken);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    watch.Stop();

                    return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(responseMessage);
                }
                else
                {
                    var result = await responseMessage.Content.ReadAsAsync<TResult>(options.MediaType, options.SerializerOptions);

                    watch.Stop();

                    if (options.ElapsedTimeHandler != null)
                        options.ElapsedTimeHandler.Invoke($"{client.BaseAddress}{urlBuilder}", watch.Elapsed);

                    return result;
                }
            }
            catch (TimeoutRejectedException ex)
            {
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.RequestTimeout,
                    Content = new StringContent($"Http请求超时：{client.BaseAddress}{urlBuilder}，{ex.Message}")
                });
            }
            catch (HttpRequestException ex)
            {
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent($"Http请求失败：{client.BaseAddress}{urlBuilder}，{ex.Message}")
                });
            }
        }

        /// <summary>
        /// 异步POST请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PostAsync<TModel, TResult>(this IHttpService httpMessageService, UrlBuilder urlBuilder, TModel model, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        {
            options ??= HttpOptions<TResult>.Default;

            var client = httpMessageService.CreateClient(urlBuilder.ClientName)
                .SetAccept(options.MediaType);

            if (options.ClientHandler != null)
                options.ClientHandler.Invoke(client);
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                var responseMessage = await client.PostAsync(urlBuilder, model, cancellationToken);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    watch.Stop();
                    return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(responseMessage);
                }
                else
                {
                    var result = await responseMessage.Content.ReadAsAsync<TResult>(options.MediaType, options.SerializerOptions);

                    watch.Stop();

                    if (options.ElapsedTimeHandler != null)
                        options.ElapsedTimeHandler.Invoke($"{client.BaseAddress}{urlBuilder}", watch.Elapsed);

                    return result;
                }
            }
            catch (TimeoutRejectedException ex)
            {
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.RequestTimeout,
                    Content = new StringContent($"Http请求超时：{client.BaseAddress}{urlBuilder}，{ex.Message}")
                });
            }
            catch (HttpRequestException ex)
            {
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent($"Http请求失败：{client.BaseAddress}{urlBuilder}，{ex.Message}")
                });
            }
        }

        /// <summary>
        /// 异步POST请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PostAsync<TResult>(this IHttpService httpMessageService, UrlBuilder urlBuilder, HttpContent content, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        {
            options ??= HttpOptions<TResult>.Default;

            var client = httpMessageService.CreateClient(urlBuilder.ClientName)
                .SetAccept(options.MediaType);

            if (options.ClientHandler != null)
                options.ClientHandler.Invoke(client);
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                var responseMessage = await client.PostAsync(urlBuilder, content, cancellationToken);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    watch.Stop();

                    return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(responseMessage);
                }
                else
                {
                    var result = await responseMessage.Content.ReadAsAsync<TResult>(options.MediaType, options.SerializerOptions);

                    watch.Stop();

                    if (options.ElapsedTimeHandler != null)
                        options.ElapsedTimeHandler.Invoke($"{client.BaseAddress}{urlBuilder}", watch.Elapsed);

                    return result;
                }
            }
            catch (TimeoutRejectedException ex)
            {
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.RequestTimeout,
                    Content = new StringContent($"Http请求超时：{client.BaseAddress}{urlBuilder}，{ex.Message}")
                });
            }
            catch (HttpRequestException ex)
            {
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent($"Http请求失败：{client.BaseAddress}{urlBuilder}，{ex.Message}")
                });
            }
        }

        /// <summary>
        /// 异步Put请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PutAsync<TModel, TResult>(this IHttpService httpMessageService, UrlBuilder urlBuilder, TModel model, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        {
            options ??= HttpOptions<TResult>.Default;

            var client = httpMessageService.CreateClient(urlBuilder.ClientName)
                .SetAccept(options.MediaType);

            if (options.ClientHandler != null)
                options.ClientHandler.Invoke(client);
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                var responseMessage = await client.PutAsync(urlBuilder, model, cancellationToken);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    watch.Stop();

                    return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(responseMessage);
                }
                else
                {
                    var result = await responseMessage.Content.ReadAsAsync<TResult>(options.MediaType, options.SerializerOptions);

                    watch.Stop();

                    if (options.ElapsedTimeHandler != null)
                        options.ElapsedTimeHandler.Invoke($"{client.BaseAddress}{urlBuilder}", watch.Elapsed);

                    return result;
                }
            }
            catch (TimeoutRejectedException ex)
            {
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.RequestTimeout,
                    Content = new StringContent($"Http请求超时：{client.BaseAddress}{urlBuilder}，{ex.Message}")
                });
            }
            catch (HttpRequestException ex)
            {
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent($"Http请求失败：{client.BaseAddress}{urlBuilder}，{ex.Message}")
                });
            }
        }

        /// <summary>
        /// 异步Put请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        public static async Task<TResult> PutAsync<TResult>(this IHttpService httpMessageService, UrlBuilder urlBuilder, HttpContent content, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        {
            options ??= HttpOptions<TResult>.Default;

            var client = httpMessageService.CreateClient(urlBuilder.ClientName)
                .SetAccept(options.MediaType);

            if (options.ClientHandler != null)
                options.ClientHandler.Invoke(client);
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                var responseMessage = await client.PutAsync(urlBuilder, content, cancellationToken);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    watch.Stop();

                    return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(responseMessage);
                }
                else
                {
                    var result = await responseMessage.Content.ReadAsAsync<TResult>(options.MediaType, options.SerializerOptions);

                    watch.Stop();

                    if (options.ElapsedTimeHandler != null)
                        options.ElapsedTimeHandler.Invoke($"{client.BaseAddress}{urlBuilder}", watch.Elapsed);

                    return result;
                }
            }
            catch (TimeoutRejectedException ex)
            {
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.RequestTimeout,
                    Content = new StringContent($"Http请求超时：{client.BaseAddress}{urlBuilder}，{ex.Message}")
                });
            }
            catch (HttpRequestException ex)
            {
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent($"Http请求失败：{client.BaseAddress}{urlBuilder}，{ex.Message}")
                });
            }
        }

        /// <summary>
        /// 异步Patch请求
        /// </summary>
        /// <typeparam name="TModel">提交类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="model">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> PatchAsync<TModel, TResult>(this IHttpService httpMessageService, UrlBuilder urlBuilder, TModel model, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        {
            options ??= HttpOptions<TResult>.Default;

            var client = httpMessageService.CreateClient(urlBuilder.ClientName)
                .SetAccept(options.MediaType);

            if (options.ClientHandler != null)
                options.ClientHandler.Invoke(client);
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                var responseMessage = await client.PatchAsync(urlBuilder, model, cancellationToken);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    watch.Stop();

                    return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(responseMessage);
                }
                else
                {
                    var result = await responseMessage.Content.ReadAsAsync<TResult>(options.MediaType, options.SerializerOptions);

                    watch.Stop();

                    if (options.ElapsedTimeHandler != null)
                        options.ElapsedTimeHandler.Invoke($"{client.BaseAddress}{urlBuilder}", watch.Elapsed);

                    return result;
                }
            }
            catch (TimeoutRejectedException ex)
            {
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.RequestTimeout,
                    Content = new StringContent($"Http请求超时：{client.BaseAddress}{urlBuilder}，{ex.Message}")
                });
            }
            catch (HttpRequestException ex)
            {
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent($"Http请求失败：{client.BaseAddress}{urlBuilder}，{ex.Message}")
                });
            }
        }

        /// <summary>
        /// 异步Patch请求
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="content">提交的模型数据</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        public static async Task<TResult> PatchAsync<TResult>(this IHttpService httpMessageService, UrlBuilder urlBuilder, HttpContent content, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        {
            options ??= HttpOptions<TResult>.Default;

            var client = httpMessageService.CreateClient(urlBuilder.ClientName)
                .SetAccept(options.MediaType);

            if (options.ClientHandler != null)
                options.ClientHandler.Invoke(client);
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                var responseMessage = await client.PatchAsync(urlBuilder, content, cancellationToken);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    watch.Stop();

                    return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(responseMessage);
                }
                else
                {
                    var result = await responseMessage.Content.ReadAsAsync<TResult>(options.MediaType, options.SerializerOptions);

                    watch.Stop();

                    if (options.ElapsedTimeHandler != null)
                        options.ElapsedTimeHandler.Invoke($"{client.BaseAddress}{urlBuilder}", watch.Elapsed);

                    return result;
                }
            }
            catch (TimeoutRejectedException ex)
            {
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.RequestTimeout,
                    Content = new StringContent($"Http请求超时：{client.BaseAddress}{urlBuilder}，{ex.Message}")
                });
            }
            catch (HttpRequestException ex)
            {
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent($"Http请求失败：{client.BaseAddress}{urlBuilder}，{ex.Message}")
                });
            }
        }

        /// <summary>
        /// 异步DELETE请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="httpMessageService"><see cref="IHttpService"/>操作服务</param>
        /// <param name="urlBuilder"><see cref="UrlBuilder"/>Url构造器</param>
        /// <param name="options">请求配置</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> DeleteAsync<TResult>(this IHttpService httpMessageService, UrlBuilder urlBuilder, IHttpOptions<TResult> options, CancellationToken cancellationToken = default)
        {
            options ??= HttpOptions<TResult>.Default;

            var client = httpMessageService.CreateClient(urlBuilder.ClientName)
                .SetAccept(options.MediaType);

            if (options.ClientHandler != null)
                options.ClientHandler.Invoke(client);
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                var responseMessage = await client.DeleteAsync(urlBuilder, cancellationToken);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    watch.Stop();

                    return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(responseMessage);
                }
                else
                {
                    var result = await responseMessage.Content.ReadAsAsync<TResult>(options.MediaType, options.SerializerOptions);

                    watch.Stop();

                    if (options.ElapsedTimeHandler != null)
                        options.ElapsedTimeHandler.Invoke($"{client.BaseAddress}{urlBuilder}", watch.Elapsed);

                    return result;
                }
            }
            catch (TimeoutRejectedException ex)
            {
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.RequestTimeout,
                    Content = new StringContent($"Http请求超时：{client.BaseAddress}{urlBuilder}，{ex.Message}")
                });
            }
            catch (HttpRequestException ex)
            {
                return options.ErrorHandler == null ? default : await options.ErrorHandler.Invoke(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent($"Http请求失败：{client.BaseAddress}{urlBuilder}，{ex.Message}")
                });
            }
        }
    }
}
