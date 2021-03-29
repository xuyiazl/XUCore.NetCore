using AspectCore.Extensions.DependencyInjection;
using AspectCore.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.AspectCore.Interceptor
{
    public static partial class CacheServiceCollectionExtensions
    {
        /// <summary>
        /// 注册缓存拦截服务
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddCacheService<TCacheServiceImpl>(this IServiceCollection services)
            where TCacheServiceImpl : class, ICacheService
        {
            services.AddMemoryCache();
            services.TryAddSingleton<ICacheService, TCacheServiceImpl>();
            services.TryAddTransient<QuartzRefreshJob>();
            services.TryAddSingleton<QuartzService>();
            //根据属性注入来配置全局拦截器
            services.ConfigureDynamicProxy();

            return services;
        }
        /// <summary>
        /// 启用缓存拦截服务
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseCacheService(this IApplicationBuilder app)
        {
            var quartz = app.ApplicationServices.GetRequiredService<QuartzService>();
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            lifetime.ApplicationStarted.Register(quartz.Start);
            lifetime.ApplicationStopped.Register(quartz.Stop);

            return app;
        }
        /// <summary>
        /// 启用拦截器，使用 ===》aspect ServiceContextProviderFactory
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static IHostBuilder UseCacheHostBuilder(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseServiceContext();
        }
    }
}
