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

namespace XUCore.NetCore.Extensions
{
    /// <summary>
    /// 服务扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 绑定本地缓存管理
        /// </summary>
        /// <remarks>引入 ICacheManager 使用</remarks>
        /// <param name="services">服务集合</param>
        /// <param name="memoryCacheOptions"></param>
        public static void AddCacheManager(this IServiceCollection services, MemoryCacheOptions memoryCacheOptions = null)
        {
            //services.AddSingleton<IMemoryCache, MemoryCache>();

            if (memoryCacheOptions == null)
                memoryCacheOptions = new MemoryCacheOptions() { };

            services.AddSingleton<ICacheManager>(o =>
            {
                return new CacheManager(memoryCacheOptions);
            });
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
        /// 注册Razor静态Html生成器
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRazorHtml(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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
        /// <param name="httpClientLeftTime"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpMessageService(this IServiceCollection services,
            string clientName,
            string baseAddress,
            Func<HttpMessageHandler> messageHandler = null,
            TimeSpan? httpClientLeftTime = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            Action<HttpClient> client = c =>
            {
                c.BaseAddress = new Uri(baseAddress);
                c.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate");
            };

            services.AddHttpMessageService(clientName, client, messageHandler, httpClientLeftTime, serviceLifetime);

            return services;
        }

        /// <summary>
        /// 注册 HTTPFactory Srevice
        /// </summary>
        /// <param name="services"></param>
        /// <param name="clientName"></param>
        /// <param name="client"></param>
        /// <param name="messageHandler"></param>
        /// <param name="httpClientLeftTime"></param>
        /// <param name="serviceLifetime"></param>
        public static IServiceCollection AddHttpMessageService(this IServiceCollection services,
            string clientName = "apiClient",
            Action<HttpClient> client = null,
            Func<HttpMessageHandler> messageHandler = null,
            TimeSpan? httpClientLeftTime = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddPolicyRegistry();

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

            if (httpClientLeftTime != null)
                httpClientBuilder.SetHandlerLifetime(httpClientLeftTime.Value);

            services.AddHttpMessageServiceLeftTime(serviceLifetime);

            return services;
        }

        /// <summary>
        /// 注册 HTTPFactory Srevice
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        private static IServiceCollection AddHttpMessageServiceLeftTime(this IServiceCollection services,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                    services.TryAddScoped<IHttpService, HttpMessageService>();
                    break;

                case ServiceLifetime.Transient:
                    services.TryAddTransient<IHttpService, HttpMessageService>();
                    break;

                case ServiceLifetime.Singleton:
                    services.TryAddSingleton<IHttpService, HttpMessageService>();
                    break;
            }

            return services;
        }

        #endregion 注册HttpFactory
    }
}