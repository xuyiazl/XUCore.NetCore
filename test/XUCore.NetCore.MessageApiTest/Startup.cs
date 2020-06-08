using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using XUCore.Extensions;
using XUCore.Serializer;
using XUCore.NetCore.Extensions;
using XUCore.NetCore.HttpFactory;
using XUCore.NetCore.MessagePack;

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
            services.AddControllers(options =>
            {
                options.MaxModelValidationErrors = 50;
            })
            //注册API MessagePack输出格式。 输入JSON/MessagePack  输出 JSON/MessagePack/MessagePack-Jackson
            .AddMessagePackFormatters(options =>
            {
                options.JsonSerializerSettings = new JsonSerializerSettings()
                {
                    //统一设置JSON格式输出为utc
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    //统一设置JSON为小驼峰格式
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
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
                    return new BadRequestObjectResult(new Result<string>() { code = 0, subCode = "", message = errors.Join(), data = "", elapsedTime = -1 });
                };
            });

            services.AddHttpService();
            services.AddHttpService("msgpack", "http://localhost:5000");


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("api", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = $"Test Api",
                    Description = "Test API"
                });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                //获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                c.IncludeXmlComments(Path.Combine(basePath, "XUCore.NetCore.MessageApiTest.xml"));

                //c.DocumentFilter<HiddenApiFilter>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/api/swagger.json", "Test API");
            });
        }
    }
}
