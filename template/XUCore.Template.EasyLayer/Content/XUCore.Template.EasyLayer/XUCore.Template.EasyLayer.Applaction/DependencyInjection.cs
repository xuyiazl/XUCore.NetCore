using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.Authorization.JwtBearer;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Extensions;
using XUCore.NetCore.MessagePack;
using XUCore.NetCore.Oss;
using XUCore.NetCore.Swagger;
using XUCore.Serializer;
using XUCore.Template.EasyLayer.Core;
using XUCore.Template.EasyLayer.DbService;

namespace XUCore.Template.EasyLayer.Applaction
{
    public static class DependencyInjection
    {
        const string policyName = "CorsPolicy";

        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddMediatR(typeof(IAppService), typeof(IDbService));

            services.Scan(scan =>
                scan.FromAssemblyOf<IAppService>()
                .AddClasses(impl => impl.AssignableTo(typeof(IAppService)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            services.Scan(scan =>
                scan.FromAssemblyOf<IDbService>()
                .AddClasses(impl => impl.AssignableTo(typeof(IDbService)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            // 注册用户信息
            services.AddSingleton<IUserInfo, UserInfo>();

            // 注入redis插件
            //services.AddRedisService().AddJsonRedisSerializer();

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

            //添加跨域配置，加载进来，启用的话需要使用Configure
            services.AddCors(options =>
                options.AddPolicy(policyName, builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                })
            );

            // 注册jwt
            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);//enableGlobalAuthorize: true

            services
                .AddControllers()
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
                });

            // 注入动态API
            services.AddDynamicWebApi(opt =>
            {
                opt.IsAutoSortAction = false;
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

            var env = services.BuildServiceProvider().GetService<IWebHostEnvironment>();

            services.AddMiniSwagger(swaggerGenAction: opt =>
            {
                opt.SwaggerDoc(ApiGroup.Admin, new OpenApiInfo
                {
                    Version = ApiGroup.Admin,
                    Title = $"管理员后台API - {env.EnvironmentName}",
                    Description = "管理员后台API"
                });

                opt.AddJwtBearerDoc();

                opt.AddDescriptions(typeof(DependencyInjection),
                    "XUCore.Template.EasyLayer.Applaction.xml",
                    "XUCore.Template.EasyLayer.Persistence.xml",
                    "XUCore.Template.EasyLayer.DbService.xml",
                    "XUCore.Template.EasyLayer.Core.xml");

                // TODO:一定要返回true！true 分组无效 注释掉 必须有分组才能出现api
                //options.DocInclusionPredicate((docName, description) => true);
            });

            return services;
        }

        public static IApplicationBuilder UseApplication(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            //支持跨域
            app.UseCors(policyName);

            app.UseRouting();
            app.UseRealIp();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticHttpContext();
            app.UseStaticFiles();

            app.UseMiniSwagger(swaggerUIAction: (opt) =>
            {
                opt.SwaggerEndpoint($"/swagger/{ApiGroup.Admin}/swagger.json", $"管理员后台 API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
