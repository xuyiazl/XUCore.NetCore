using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using XUCore.NetCore.Swagger;
using WebApi.Applaction;

namespace WebApi.WebApi.Controller
{
    public static class SwaggerDependencyInjection
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            var env = services.BuildServiceProvider().GetService<IWebHostEnvironment>();

            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(ApiGroup.Admin, new OpenApiInfo
                {
                    Version = ApiGroup.Admin,
                    Title = $"管理员后台API - {env.EnvironmentName}",
                    Description = "管理员后台API"
                });
                options.SwaggerDoc(ApiGroup.Login, new OpenApiInfo
                {
                    Version = ApiGroup.Login,
                    Title = $"登录相关API - {env.EnvironmentName}",
                    Description = "登录相关API"
                });
                options.SwaggerDoc(ApiGroup.File, new OpenApiInfo
                {
                    Version = ApiGroup.File,
                    Title = $"文件操作相关API - {env.EnvironmentName}",
                    Description = "文件操作相关API"
                });

                options.AddJwtBearerDoc();
                //options.AddHttpSignDoc(services);
                //options.AddFiledDoc();

                options.AddDescriptions(typeof(Startup),
                        "WebApi.WebApi.xml",
                        "WebApi.Applaction.xml",
                        "WebApi.Persistence.xml",
                        "WebApi.Core.xml");

                // TODO:一定要返回true！true 分组无效 注释掉 必须有分组才能出现api
                //options.DocInclusionPredicate((docName, description) => true);
            });

            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(options =>
            {
                options.PreSerializeFilters.Add((swaggerDoc, _) =>
                {
                    swaggerDoc.Servers.Clear();
                });
            });

            app.UseSwaggerUI(c =>
            {
                c.AddMiniProfiler();

                c.SwaggerEndpoint($"/swagger/{ApiGroup.Admin}/swagger.json", $"管理员后台 API");
                c.SwaggerEndpoint($"/swagger/{ApiGroup.Login}/swagger.json", $"登录相关 API");
                c.SwaggerEndpoint($"/swagger/{ApiGroup.File}/swagger.json", $"文件操作相关 API");

                c.DocExpansion(DocExpansion.None);
            });

            return app;
        }
    }
}
