using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Generic;
using System.Linq;
using XUCore.Extensions;
using XUCore.NetCore.Authorization.JwtBearer;
using XUCore.NetCore.Extensions;
using XUCore.NetCore.MessagePack;
using XUCore.NetCore.Swagger;
using XUCore.Serializer;

namespace XUCore.NetCore.MessageApiTest
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
            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);//enableGlobalAuthorize: true

            services.AddControllers(options =>
            {
                options.MaxModelValidationErrors = 50;
            })
            //注册API MessagePack输出格式。 输入JSON/MessagePack  输出 JSON/MessagePack/MessagePack-Jackson
            .AddMessagePackFormatters(options =>
            {
                var reProps = new Dictionary<string, string> { { "code", "_code" },
                    { "subCode", "_subCode" },
                    { "bodyMessage", "data" },
                    { "TemperatureC", "c" },
                    { "Summary", "s" } };

                var props = new string[] { "_code", "_subCode", "data", "c", "s" };

                options.JsonSerializerSettings = new JsonSerializerSettings()
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,
                    //ContractResolver = new LimitPropsCamelCaseContractResolver(props, LimitPropsType.Contains, reProps)
                    ContractResolver = new LimitPropsContractResolver()
                };
                //默认设置MessageagePack的日期序列化格式为时间戳，对外输出一致为时间戳的日期，不需要我们自己去序列化，自动操作。
                //C#实体内仍旧保持DateTime。跨语言MessageagePack没有DateTime类型。
                options.FormatterResolver = MessagePackSerializerResolver.UnixDateTimeFormatter;
                options.Options = MessagePackSerializerResolver.UnixDateTimeOptions;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                //自定义模型验证错误的输出结构
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => e.Value.Errors.First().ErrorMessage)
                    .ToList();
                    return new BadRequestObjectResult(new Result<string>() { Code = 0, SubCode = "", Message = errors.Join(), Data = "", ElapsedTime = -1 });
                };
            });

            services.AddHttpService();
            services.AddHttpService("msgpack", "http://localhost:5000");

            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("api", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = $"Test Api",
                    Description = "Test API"
                });
                options.AddJwtBearerDoc();
                //options.AddHttpSignDoc(services);
                //options.AddFiledDoc();

                options.AddDescriptions(typeof(Program),
                    "XUCore.NetCore.MessageApiTest.xml"
                );

                // TODO:一定要返回true！true 分组无效 注释掉 必须有分组才能出现api
                //options.DocInclusionPredicate((docName, description) => true);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //启用静态请求上下文
            app.UseStaticHttpContext();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger(options =>
            {
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
                c.AddMiniProfiler();

                c.SwaggerEndpoint($"/swagger/api/swagger.json", "Test API");

                c.DocExpansion(DocExpansion.None);
            });
        }
    }
}
