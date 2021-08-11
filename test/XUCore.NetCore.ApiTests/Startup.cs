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
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Linq;
using System.Net;
using XUCore.Extensions;
using XUCore.NetCore;
using XUCore.NetCore.ApiTests;
using XUCore.NetCore.Authorization.JwtBearer;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Extensions;
using XUCore.NetCore.Logging.Log4Net;
using XUCore.NetCore.MessagePack;
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

            // ע��jwt
            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);//enableGlobalAuthorize: true

            services.AddControllers()
                .AddMessagePackFormatters(options =>
                {
                    options.JsonSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    options.JsonSerializerSettings.ContractResolver = new LimitPropsContractResolver();

                    //Ĭ������MessageagePack���������л���ʽΪʱ������������һ��Ϊʱ��������ڣ�����Ҫ�����Լ�ȥ���л����Զ�������
                    //C#ʵ�����Ծɱ���DateTime��������MessageagePackû��DateTime���͡�
                    options.FormatterResolver = MessagePackSerializerResolver.UnixDateTimeFormatter;
                    options.Options = MessagePackSerializerResolver.UnixDateTimeOptions;

                })
                .AddFluentValidation(opt =>
                {
                    opt.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    opt.RegisterValidatorsFromAssemblyContaining(typeof(Program));
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


            //ͳһ������֤����Ϣ
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
            //ע��Swagger������������һ���Ͷ��Swagger �ĵ�
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("test", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = $"test",
                    Description = "test"
                });

                options.AddJwtBearerDoc();
                options.AddHttpSignDoc(services);
                options.AddFiledDoc();
                options.AddHiddenApi();
                options.AddDescriptions(typeof(Program), "XUCore.NetCore.ApiTests.xml");

                // TODO:һ��Ҫ����true��
                options.DocInclusionPredicate((docName, description) => true);

            });

            services.AddDynamicWebApi(opt =>
            {
                opt.IsRemoveVerbs = false;
                opt.SplitActionCamelCase = true;
                opt.SplitActionCamelCaseSeparator = "-";
            });

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

                c.AddMiniProfiler();

                c.DocExpansion(DocExpansion.None);
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