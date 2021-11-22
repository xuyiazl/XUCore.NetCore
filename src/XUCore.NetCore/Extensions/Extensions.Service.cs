using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using XUCore.Cache;
using XUCore.NetCore.HttpFactory;
using XUCore.NetCore.Razors;
using XUCore.NetCore.Uploads;
using XUCore.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using XUCore.Develops;
using XUCore.NetCore.Signature;
using XUCore.Configs;
using XUCore.NetCore.Oss;

namespace XUCore.NetCore
{
    /// <summary>
    /// 服务扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 数据流量控制
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="interval">默认1秒刷新一次</param>
        /// <param name="size">每秒限制的数据大小，单位kb</param>
        /// <param name="monitoring">监控</param>
        public static void AddFlowMonitoring(this IServiceCollection services, int interval = 1000, int size = 128, Action<decimal> monitoring = null)
        {
            services.AddSingleton<IFlowMonitoring>(o =>
            {
                var flow = new FlowMonitoring(interval);
                if (monitoring != null)
                    flow.Monitoring(monitoring);
                flow.Size = size;
                return flow;
            });
        }

        /// <summary>
        /// 绑定本地缓存管理
        /// </summary>
        /// <remarks>引入 ICacheManager 使用</remarks>
        /// <param name="services">服务集合</param>
        /// <param name="options"></param>
        public static void AddCacheManager(this IServiceCollection services, Action<MemoryCacheOptions> options = null)
        {
            if (options == null)
                services.AddMemoryCache();
            else
                services.AddMemoryCache(options);

            services.TryAddSingleton<ICacheManager, CacheManager>();
        }


        /// <summary>
        /// 注册上传服务
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void AddUploadService(this IServiceCollection services)
        {
            services.AddUploadService<DefaultFileUploadService>();
        }

        /// <summary>
        /// 注册上传服务
        /// </summary>
        /// <typeparam name="TFileUploadService">文件上传服务类型</typeparam>
        /// <param name="services">服务集合</param>
        public static void AddUploadService<TFileUploadService>(this IServiceCollection services)
            where TFileUploadService : class, IFileUploadService
        {
            services.TryAddScoped<IFileUploadService, TFileUploadService>();
        }

        /// <summary>
        /// 注册oss 上传客户端
        /// </summary>
        /// <param name="services"></param>
        /// <param name="clients"></param>
        /// <returns></returns>
        public static IServiceCollection AddOssClient(this IServiceCollection services, params (string name, OssOptions options)[] clients)
        {
            if (clients == null && clients.Length == 0) return services;

            services.TryAddSingleton<IOssFactory>(o =>
            {
                var factory = new OssFactory();

                clients.ForEach(c =>
                {
                    factory.CreateClient(c.name, c.options);
                });

                return factory;
            });

            return services;
        }
        /// <summary>
        /// 注册oss 分片上传客户端
        /// </summary>
        /// <param name="services"></param>
        /// <param name="clients"></param>
        /// <returns></returns>
        public static IServiceCollection AddOssMultiPartClient(this IServiceCollection services, params (string name, OssOptions options)[] clients)
        {
            if (clients == null && clients.Length == 0) return services;

            services.TryAddSingleton<IOssMultiPartFactory>(o =>
            {
                var factory = new OssMultiPartFactory();

                clients.ForEach(c =>
                {
                    factory.CreateClient(c.name, c.options);
                });

                return factory;
            });

            return services;
        }
        /// <summary>
        /// 注册Razor静态Html生成器
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRazorHtml(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IRouteAnalyzer, RouteAnalyzer>();
            services.AddScoped<IRazorHtmlGenerator, DefaultRazorHtmlGenerator>();
            return services;
        }

        #region 注册HttpFactory

        /// <summary>
        /// 注册 HttpFactory Service
        /// </summary>
        /// <param name="services"></param>
        /// <param name="clientName"></param>
        /// <param name="baseAddress"></param>
        /// <param name="messageHandler"></param>
        /// <param name="clientBuilder"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpService(this IServiceCollection services,
            string clientName,
            string baseAddress,
            Func<HttpMessageHandler> messageHandler = null,
            Action<IHttpClientBuilder> clientBuilder = null)
        {
            Action<HttpClient> client = c =>
            {
                c.BaseAddress = new Uri(baseAddress);
                c.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate");
            };

            services.AddHttpService(clientName, client, messageHandler, clientBuilder);

            return services;
        }

        /// <summary>
        /// 注册 HTTPFactory Srevice
        /// </summary>
        /// <param name="services"></param>
        /// <param name="clientName"></param>
        /// <param name="client"></param>
        /// <param name="messageHandler"></param>
        /// <param name="clientBuilder"></param>
        public static IServiceCollection AddHttpService(this IServiceCollection services,
            string clientName = "apiClient",
            Action<HttpClient> client = null,
            Func<HttpMessageHandler> messageHandler = null,
            Action<IHttpClientBuilder> clientBuilder = null)
        {
            services.AddHttpContextAccessor();

            if (client == null)
                client = c =>
                {
                    //c.BaseAddress = new Uri(baseAddress);
                    c.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate");
                };

            var httpClientBuilder = services.AddHttpClient(clientName, client);

            if (messageHandler != null)
                httpClientBuilder.ConfigurePrimaryHttpMessageHandler(messageHandler);
            else
                httpClientBuilder.ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var handler = new HttpClientHandler();
                    handler.AllowAutoRedirect = false;
                    handler.UseDefaultCredentials = false;
                    if (handler.SupportsAutomaticDecompression)
                    {
                        handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    }
                    return handler;
                });

            if (clientBuilder != null)
                clientBuilder.Invoke(httpClientBuilder);

            //services.TryAddScoped(typeof(IHttpOptions<>), typeof(HttpOptions<>));
            services.TryAddSingleton<IHttpService, HttpService>();

            HttpRemote.ServiceProvider = services.BuildServiceProvider();

            return services;
        }

        #endregion 注册HttpFactory
    }
}