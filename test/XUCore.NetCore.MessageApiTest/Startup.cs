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
            //ע��API MessagePack�����ʽ�� ����JSON/MessagePack  ��� JSON/MessagePack/MessagePack-Jackson
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
                //Ĭ������MessageagePack���������л���ʽΪʱ������������һ��Ϊʱ��������ڣ�����Ҫ�����Լ�ȥ���л����Զ�������
                //C#ʵ�����Ծɱ���DateTime��������MessageagePackû��DateTime���͡�
                options.FormatterResolver = MessagePackSerializerResolver.UnixDateTimeFormatter;
                options.Options = MessagePackSerializerResolver.UnixDateTimeOptions;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                //�Զ���ģ����֤���������ṹ
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
                // Ϊ Swagger JSON and UI����xml�ĵ�ע��·��
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                //��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
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
            //���þ�̬����������
            app.UseStaticHttpContext();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //�����м����������Swagger��ΪJSON�ս��
            app.UseSwagger();
            //�����м�������swagger-ui��ָ��Swagger JSON�ս��
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/api/swagger.json", "Test API");
            });
        }
    }
}
