using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Scrutor;
using System;
using XUCore.Helpers;

namespace XUCore.NetCore
{
    /// <summary>
    /// 服务扩展
    /// </summary>
    public static partial class DependencyInjection
    {
        /// <summary>
        /// 指定扫描方式，扫描对象。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblyFilter">指定当前项目需要扫描的程序集名，“Demo.dll”中的Demo</param>
        /// <returns></returns>
        public static IServiceCollection AddScanLifetime(this IServiceCollection services, string assemblyFilter)
            => AddScanLifetime(services, scan => scan.FromAssemblies(Reflection.GetCurrentProjectAssemblies(assemblyFilter)));
        /// <summary>
        /// 指定扫描方式，扫描对象。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="fromAssembly">当指定为null，则默认从DependencyContext中扫描</param>
        /// <returns></returns>
        public static IServiceCollection AddScanLifetime(this IServiceCollection services, Func<ITypeSourceSelector, IImplementationTypeSelector> fromAssembly = null)
        {
            services.Scan(scan =>
                    (fromAssembly == null ? scan.FromDependencyContext(DependencyContext.Default) : fromAssembly(scan))
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