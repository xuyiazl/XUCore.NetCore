using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using XUCore.NetCore.Extensions;
using XUCore.NetCore.Jwt;
using XUCore.NetCore.Logging.Log4Net;
using XUCore.NetCore.MessagePack;
using XUCore.NetCore.Signature;
using XUCore.Configs;
using XUCore.NetCore.ApiTests;
using Microsoft.OpenApi.Models;
using System.IO;
using XUCore.Serializer;
using XUCore.NetCore.Swagger;

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

            var jwtSettings = appSection.Get<JwtOptions>();
            services.AddJwtOptions(options => appSection.Bind(options));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwt(JwtAuthenticationDefaults.AuthenticationScheme, options =>
             {
                 options.Keys = new[] { jwtSettings.Secret };
                 options.VerifySignature = true;
             });

            services.AddControllers()
                .AddMessagePackFormatters(options =>
                {
                    options.JsonSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    options.JsonSerializerSettings.ContractResolver = new LimitPropsCamelCaseContractResolver();

                    //默认设置MessageagePack的日期序列化格式为时间戳，对外输出一致为时间戳的日期，不需要我们自己去序列化，自动操作。
                    //C#实体内仍旧保持DateTime。跨语言MessageagePack没有DateTime类型。
                    options.FormatterResolver = MessagePackSerializerResolver.UnixDateTimeFormatter;
                    options.Options = MessagePackSerializerResolver.UnixDateTimeOptions;

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


            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("test", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = $"test",
                    Description = "test"
                });

                options.SwaggerHttpSignDoc(services);
                //options.SwaggerFiledDoc();

                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var apiXml = Path.Combine(basePath, "XUCore.NetCore.ApiTests.xml");
                //获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                options.IncludeXmlComments(apiXml);

                options.SwaggerControllerDescriptions(apiXml);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //注册log4net日志
            loggerFactory.AddLog4Net();
            //注册真实IP中间件
            app.UseRealIp();
            //启用静态请求上下文
            app.UseStaticHttpContext();

            //app.UseHttpSign<SignMiddlewareDemo>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger(options =>
            {
                //如果使用了 大于 5.6.3 版本，新功能Servers和反向代理的支持问题，
                //issues https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1953

                //由于使用了反向代理需要运维支持转发X-Forwarded-* headers的一些工作，所以太麻烦。故干脆清理掉算了。等官方直接解决了该问题再使用

                options.PreSerializeFilters.Add((swaggerDoc, _) =>
                {
                    swaggerDoc.Servers.Clear();
                });
                //options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                //{
                //    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host}" } };
                //});
            });
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/test/swagger.json", "test API");

                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
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