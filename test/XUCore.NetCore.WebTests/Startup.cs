using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using XUCore.NetCore.Extensions;
using XUCore.NetCore.HttpFactory;
using XUCore.NetCore.Logging.Log4Net;
using XUCore.NetCore.Redis;
using XUCore.NetCore.Data.DbService;
using XUCore.WebTests.Data.DbService;
using XUCore.WebTests.Data.Repository.ReadRepository;
using XUCore.WebTests.Data.Repository.WriteRepository;
using System;

namespace XUCore.WebTests
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWriteDbContext(Configuration);
            services.AddReadDbContext(Configuration);

            //DI 注入db持久层业务逻辑
            services.Scan(scan =>
               scan.FromAssemblyOf<IDbDependencyService>()
               .AddClasses(impl => impl.AssignableTo(typeof(IDbDependencyService)))
               .AsImplementedInterfaces()
               .WithScopedLifetime()
           );

            services.AddHttpService();

            services.AddRedisService().AddJsonRedisSerializer();

            //注册razor静态HTML生成器
            services.AddRazorHtml();

            services.AddControllersWithViews(options =>
            {
                //全局异常过滤器
                //options.Filters.Add<ExceptionHandlerAttribute>();
            })
            .AddNewtonsoftJson(options =>
            {
                //需要引入nuget
                //<PackageReference Include="Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="3.1.0" />
                //EF Core中默认为驼峰样式序列化处理key
                //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //使用默认方式，不更改元数据的key的大小写
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();//new DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddUploadService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //注册log4net日志
            loggerFactory.AddLog4Net();
            //注册真实IP中间件
            app.UseRealIp();
            //启用静态请求上下文
            app.UseStaticHttpContext();
            //全局请求日志中间件
            //app.UseRequestLog();
            //全局错误日志中间件
            //app.UseErrorLog();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}