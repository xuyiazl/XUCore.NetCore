using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using XUCore.Extensions;
using XUCore.NetCore;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.Authorization.JwtBearer;
using XUCore.NetCore.EasyQuartz;
using XUCore.NetCore.Extensions;
using XUCore.NetCore.MessagePack;
using XUCore.NetCore.Oss;
using XUCore.NetCore.Redis;
using XUCore.Serializer;
using Sample.Plain.Core;
using Sample.Plain.DbService;

namespace Sample.Plain.Applaction
{
    public static class DependencyInjection
    {
        const string policyName = "CorsPolicy";

        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(IMapFrom<>));

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

            //添加跨域配置，加载进来，启用的话需要使用Configure
            services.AddCors(options =>
                options.AddPolicy(policyName, builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    //builder.AllowCredentials(); 该方法不能和AllowAnyOrigin 同时使用，否则会触发异常：The CORS protocol does not allow specifying a wildcard (any) origin and credentials at the same time. Configure the CORS policy by listing individual origins if credentials needs to be supported
                })
            );

            // 注册jwt
            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);//enableGlobalAuthorize: true

            services.AddControllers(options =>
            {
                options.MaxModelValidationErrors = 50;
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            })
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
                opt.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                opt.RegisterValidatorsFromAssemblyContaining(typeof(IDbService));
            });

            // 统一返回验证的信息
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState
                      .Values
                      .SelectMany(x => x.Errors.Select(p => p.ErrorMessage))
                      .ToList();

                    var message = errors.Join("");

                    (var code, _) = SubCodeMessage.Message(SubCode.ValidError);

                    return new ObjectResult(new Result<object>()
                    {
                        Code = 0,
                        SubCode = code,
                        Message = message,
                        Data = null,
                        ElapsedTime = -1,
                        OperationTime = DateTime.Now
                    })
                    {
                        StatusCode = (int)HttpStatusCode.OK
                    };
                };
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
