using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

namespace XUCore.NetCore
{
    /// <summary>
    /// 服务扩展
    /// </summary>
    public static partial class DependencyInjection
    {
        /// <summary>
        /// 扫描注册生命周期对象
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddScanLifetime(this IServiceCollection services)
        {
            services.Scan(scan =>
                scan
                    .FromDependencyContext(DependencyContext.Default)
                    //扫描单例
                    .AddClasses(impl => impl.AssignableTo<ISingleton>())
                    //.AsImplementedInterfaces()
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime()
                    //扫描作用域
                    .AddClasses(impl => impl.AssignableTo<IScoped>())
                    //.AsImplementedInterfaces()
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime()
                    //扫描新实例
                    .AddClasses(impl => impl.AssignableTo<ITransient>())
                    //.AsImplementedInterfaces()
                    .AsSelfWithInterfaces()
                    .WithTransientLifetime()
            );

            return services;
        }
    }
}