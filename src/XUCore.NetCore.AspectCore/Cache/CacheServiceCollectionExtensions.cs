﻿using AspectCore.Extensions.DependencyInjection;
using AspectCore.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.AspectCore.Cache
{
    public static partial class CacheServiceCollectionExtensions
    {
        /// <summary>
        /// 注册缓存拦截服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        public static IServiceCollection AddCacheInterceptor(this IServiceCollection services, Action<CacheOptions> options = null)
        {
            options ??= new Action<CacheOptions>((option) =>
            {
                option.CacheMode = CacheMode.Memory;
                option.RedisRead = "cache-read";
                option.RedisWrite = "cache-write";
            });

            services.Configure<CacheOptions>(options);

            var opt = services.BuildServiceProvider().GetService<IOptions<CacheOptions>>();

            if (opt.Value.CacheMode == CacheMode.Memory)
            {
                services.AddMemoryCache();
                services.TryAddSingleton<ICacheService, MemoryCacheService>();
            }
            else
            {
                services.TryAddSingleton<ICacheService, RedisCacheService>();
            }

            services.TryAddTransient<QuartzRefreshJob>();
            services.TryAddSingleton<QuartzService>();

            services.AddInterceptor();

            return services;
        }
        /// <summary>
        /// 启用缓存拦截服务
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseCacheTiggerService(this IApplicationBuilder app)
        {
            var quartz = app.ApplicationServices.GetRequiredService<QuartzService>();
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            lifetime.ApplicationStarted.Register(quartz.Start);
            lifetime.ApplicationStopped.Register(quartz.Stop);

            return app;
        }
        /// <summary>
        /// 注册拦截器
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInterceptor(this IServiceCollection services)
        {
            //根据属性注入来配置全局拦截器
            services.ConfigureDynamicProxy();

            return services;
        }
        /// <summary>
        /// 启用拦截器，使用 ===》aspect ServiceContextProviderFactory
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static IHostBuilder UseInterceptorHostBuilder(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseServiceContext();
        }
    }
}
