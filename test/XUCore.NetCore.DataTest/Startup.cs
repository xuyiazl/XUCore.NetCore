﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.NetCore.AspectCore.Interceptor;
using XUCore.NetCore.DataTest.Business;
using XUCore.NetCore.DataTest.DbRepository;
using XUCore.NetCore.DataTest.DbService;

namespace XUCore.NetCore.DataTest
{
    public static class Startup
    {
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddLogging();

            services.AddNigelDbContext();
            services.AddNigelCopyDbContext();

            services.AddReadDbContext();
            services.AddWriteDbContext();

            services.Scan(scan =>
                scan.FromAssemblyOf<IDbServiceProvider>()
                .AddClasses(impl => impl.AssignableTo(typeof(IDbServiceProvider)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            services.Scan(scan =>
                scan.FromAssemblyOf<IServiceDependency>()
                .AddClasses(impl => impl.AssignableTo(typeof(IServiceDependency)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            //注册缓存服务，暂时只提供内存缓存，后面会增加Redis，需要视情况而定
            services.AddCacheService<MemoryCacheService>();

            services.AddHostedService<MainService>();
        }
    }
}
