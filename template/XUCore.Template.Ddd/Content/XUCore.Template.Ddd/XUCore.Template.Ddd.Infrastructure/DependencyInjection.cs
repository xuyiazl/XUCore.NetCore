using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using XUCore.Ddd.Domain;
using XUCore.Ddd.Domain.Bus;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.Authorization.JwtBearer;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Extensions;
using XUCore.NetCore.MessagePack;
using XUCore.NetCore.Oss;
using XUCore.NetCore.Redis;
using XUCore.Serializer;
using XUCore.Template.Ddd.Domain.Core.Mappings;
using XUCore.Template.Ddd.Domain.Notifications;
using XUCore.Template.Ddd.Infrastructure.Authorization;
using XUCore.Template.Ddd.Infrastructure.Events;

namespace XUCore.Template.Ddd.Infrastructure
{
    public static class DependencyInjection
    {
        const string policyName = "CorsPolicy";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment, string project = "api")
        {
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddMediatR(typeof(DomainNotificationHandler));

            services.AddRequestBehaviour(options =>
            {
                options.Logger = true;
                options.Performance = true;
                options.Validation = true;
            });

            // 命令总线Domain Bus (Mediator)
            services.AddMediatorBus<MediatorMemoryBus>();

            // 注入 基础设施层 - 事件溯源
            services.AddEventStore<SqlEventStoreService>();

            services.AddScoped<IAuthService, AuthService>();

            // 注入redis插件
            services.AddRedisService().AddJsonRedisSerializer();

            //// 注入缓存拦截器（Redis分布式缓存）
            //services.AddCacheService<RedisCacheService>((option) =>
            //{
            //    option.RedisRead = "cache-read";
            //    option.RedisWrite = "cache-write";
            //});

            // 注入缓存拦截器（内存缓存）
            services.AddCacheService<MemoryCacheService>();
            // 注入内存缓存
            services.AddCacheManager();

            // 注册jwt
            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);//enableGlobalAuthorize: true

            IMvcBuilder mvcBuilder;

            if (project.Equals("api"))
            {
                mvcBuilder = services.AddControllers();

                services.AddDynamicWebApi(options =>
                {
                    // 指定全局默认的 api 前缀
                    options.DefaultApiPrefix = "api";
                });

                //添加跨域配置，加载进来，启用的话需要使用Configure
                services.AddCors(options =>
                    options.AddPolicy(policyName, builder =>
                    {
                        builder
                            .AllowAnyHeader()
                            .AllowAnyOrigin()
                            .AllowAnyMethod();

                        //builder.AllowCredentials(); 
                        /*
                            该方法不能和AllowAnyOrigin 同时使用，否则会触发异常：
                            The CORS protocol does not allow specifying a wildcard (any) origin and credentials at the same time. 
                            Configure the CORS policy by listing individual origins if credentials needs to be supported
                        */
                    })
                );

                services.AddSwagger(environment);
            }
            else
            {
                mvcBuilder = services.AddControllersWithViews();
            }

            mvcBuilder
                .AddMessagePackFormatters(options =>
                {
                    options.JsonSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    options.JsonSerializerSettings.ContractResolver = new LimitPropsContractResolver();

                    //默认设置MessageagePack的日期序列化格式为时间戳，对外输出一致为时间戳的日期，不需要我们自己去序列化，自动操作。
                    //C#实体内仍旧保持DateTime。跨语言MessageagePack没有DateTime类型。
                    options.FormatterResolver = MessagePackSerializerResolver.UnixDateTimeFormatter;
                    options.Options = MessagePackSerializerResolver.UnixDateTimeOptions;

                })
                .AddFluentValidation(opt =>
                {
                    opt.ValidatorOptions.CascadeMode = FluentValidation.CascadeMode.Stop;
                    opt.DisableDataAnnotationsValidation = false;
                    opt.RegisterValidatorsFromAssemblyContaining(typeof(DomainNotificationHandler));
                });

            // 注册上传服务
            services.AddUploadService();

            // 注册OSS上传服务（阿里Oss）
            services.AddOssClient(
                    (
                        "images", new OssOptions
                        {
                            AccessKey = "xxx",
                            AccessKeySecret = "xxx",
                            BluckName = "xxx",
                            EndPoint = "oss-cn-hangzhou.aliyuncs.com",
                            Domain = "https://img.xxx.com"
                        }
                    )
                );

            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IWebHostEnvironment env, string project = "api")
        {
            if (project == "api")
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                //支持跨域
                app.UseCors(policyName);
            }
            else
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                }
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticHttpContext();
            app.UseStaticFiles();

            if (project == "api")
            {
                app.UseSwagger(env);

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
            else
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
            }

            return app;
        }
    }
}
