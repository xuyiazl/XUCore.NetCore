using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using XUCore.Extensions;
using XUCore.NetCore;
using XUCore.NetCore.ApiTests;
using XUCore.NetCore.Authorization;
using XUCore.NetCore.DynamicWebApi;
//using XUCore.NetCore.Logging.Log4Net;
using XUCore.NetCore.Formatter;
using XUCore.NetCore.Signature;
using XUCore.NetCore.Swagger;
using XUCore.Serializer;

namespace XUCore.ApiTests
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
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var appSection = Configuration.GetSection("JwtOptions");

            services.AddHttpSignService();

            //var jwtSettings = appSection.Get<JwtOptions>();
            //services.AddJwtOptions(options => appSection.Bind(options));
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddJwt(JwtAuthenticationDefaults.AuthenticationScheme, options =>
            // {
            //     options.Keys = new[] { jwtSettings.Secret };
            //     options.VerifySignature = true;
            // });

            // 注册jwt
            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);//enableGlobalAuthorize: true

            services.AddControllers()
                .AddFormatters(options =>
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
                    opt.RegisterValidatorsFromAssemblyContaining(typeof(Program));
                });
            //.AddNewtonsoftJson(options =>
            //{
            //    //需要引入nuget
            //    //<PackageReference Include="Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="3.1.0" />
            //    //EF Core中默认为驼峰样式序列化处理key
            //    //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //    //使用默认方式，不更改元数据的key的大小写
            //    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();//new DefaultContractResolver();
            //    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            //    options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            //    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //});


            //统一返回验证的信息
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState
                      .Values
                      .SelectMany(x => x.Errors.Select(p => p.ErrorMessage))
                      .ToList();

                    var message = errors.Join("");

                    return new ObjectResult(new Result<object>()
                    {
                        Code = 0,
                        SubCode = "faild",
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
            services.AddMiniSwagger(swaggerGenAction: (opt) =>
            {
                opt.SwaggerDoc("test", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = $"test",
                    Description = "test"
                });

                opt.AddJwtBearerDoc();
                opt.AddHttpSignDoc(services);
                opt.AddFiledDoc();
                opt.AddHiddenApi();
                opt.AddDescriptions(typeof(Program), "XUCore.NetCore.ApiTests.xml");

                // TODO:一定要返回true！
                opt.DocInclusionPredicate((docName, description) => true);
            });

            services.AddDynamicWebApi(opt =>
            {
                //opt.IsRemoveVerbs = false;
                //opt.SplitActionCamelCase = true;
                //opt.SplitActionCamelCaseSeparator = "-";
            });

            // 自定义配置
            //services.AddDynamicWebApi((options) =>
            //{
            //    // 指定全局默认的 api 前缀
            //    options.DefaultApiPrefix = "apis";

            //    /**
            //     * 清空API结尾，不删除API结尾;
            //     * 若不清空 CreatUserAsync 将变为 CreateUser
            //     */
            //    options.RemoveActionPostfixes.Clear();

            //    /**
            //     * 自定义 ActionName 处理函数;
            //     */
            //    options.GetRestFulActionName = (actionName) => actionName;

            //    /**
            //     * 指定程序集 配置 url 前缀为 apis
            //     * 如: http://localhost:8080/apis/User/CreateUser
            //     */
            //    options.AddAssemblyOptions(this.GetType().Assembly, apiPreFix: "apis");

            //    /**
            //     * 指定程序集 配置所有的api请求方式都为 POST
            //     */
            //    options.AddAssemblyOptions(this.GetType().Assembly, httpVerb: "POST");

            //    /**
            //     * 指定程序集 配置 url 前缀为 apis, 且所有请求方式都为POST
            //     * 如: http://localhost:8080/apis/User/CreateUser
            //     */
            //    options.AddAssemblyOptions(this.GetType().Assembly, apiPreFix: "apis", httpVerb: "POST");
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //注册log4net日志
            //loggerFactory.AddLog4Net();
            //注册真实IP中间件
            app.UseRealIp();
            //启用静态请求上下文
            app.UseStaticHttpContext();

            //app.UseHttpSign<SignMiddlewareDemo>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseMiniSwagger(swaggerUIAction: (opt) =>
            {
                opt.SwaggerEndpoint($"/swagger/test/swagger.json", "test API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "areas", "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });
        }
    }
}