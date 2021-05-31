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
using XUCore.NetCore.DynamicWebApi;

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
                    options.JsonSerializerSettings.ContractResolver = new LimitPropsContractResolver();

                    //Ĭ������MessageagePack���������л���ʽΪʱ������������һ��Ϊʱ��������ڣ�����Ҫ�����Լ�ȥ���л����Զ�������
                    //C#ʵ�����Ծɱ���DateTime��������MessageagePackû��DateTime���͡�
                    options.FormatterResolver = MessagePackSerializerResolver.UnixDateTimeFormatter;
                    options.Options = MessagePackSerializerResolver.UnixDateTimeOptions;

                });
            //.AddNewtonsoftJson(options =>
            //{
            //    //��Ҫ����nuget
            //    //<PackageReference Include="Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="3.1.0" />
            //    //EF Core��Ĭ��Ϊ�շ���ʽ���л�����key
            //    //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //    //ʹ��Ĭ�Ϸ�ʽ��������Ԫ���ݵ�key�Ĵ�Сд
            //    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();//new DefaultContractResolver();
            //    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            //    options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            //    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //});


            //ע��Swagger������������һ���Ͷ��Swagger �ĵ�
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

                // Ϊ Swagger JSON and UI����xml�ĵ�ע��·��
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var apiXml = Path.Combine(basePath, "XUCore.NetCore.ApiTests.xml");
                //��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
                options.IncludeXmlComments(apiXml);

                options.SwaggerControllerDescriptions(apiXml);

                // TODO:һ��Ҫ����true��
                options.DocInclusionPredicate((docName, description) => true);

            });

            services.AddDynamicWebApi();

            // �Զ�������
            //services.AddDynamicWebApi((options) =>
            //{
            //    // ָ��ȫ��Ĭ�ϵ� api ǰ׺
            //    options.DefaultApiPrefix = "apis";

            //    /**
            //     * ���API��β����ɾ��API��β;
            //     * ������� CreatUserAsync ����Ϊ CreateUser
            //     */
            //    options.RemoveActionPostfixes.Clear();

            //    /**
            //     * �Զ��� ActionName ������;
            //     */
            //    options.GetRestFulActionName = (actionName) => actionName;

            //    /**
            //     * ָ������ ���� url ǰ׺Ϊ apis
            //     * ��: http://localhost:8080/apis/User/CreateUser
            //     */
            //    options.AddAssemblyOptions(this.GetType().Assembly, apiPreFix: "apis");

            //    /**
            //     * ָ������ �������е�api����ʽ��Ϊ POST
            //     */
            //    options.AddAssemblyOptions(this.GetType().Assembly, httpVerb: "POST");

            //    /**
            //     * ָ������ ���� url ǰ׺Ϊ apis, ����������ʽ��ΪPOST
            //     * ��: http://localhost:8080/apis/User/CreateUser
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

            //ע��log4net��־
            loggerFactory.AddLog4Net();
            //ע����ʵIP�м��
            app.UseRealIp();
            //���þ�̬����������
            app.UseStaticHttpContext();

            //app.UseHttpSign<SignMiddlewareDemo>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            //�����м����������Swagger��ΪJSON�ս��
            app.UseSwagger(options =>
            {
                //���ʹ���� ���� 5.6.3 �汾���¹���Servers�ͷ�������֧�����⣬
                //issues https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1953

                //����ʹ���˷��������Ҫ��ά֧��ת��X-Forwarded-* headers��һЩ����������̫�鷳���ʸɴ���������ˡ��ȹٷ�ֱ�ӽ���˸�������ʹ��

                options.PreSerializeFilters.Add((swaggerDoc, _) =>
                {
                    swaggerDoc.Servers.Clear();
                });
                //options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                //{
                //    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host}" } };
                //});
            });
            //�����м�������swagger-ui��ָ��Swagger JSON�ս��
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