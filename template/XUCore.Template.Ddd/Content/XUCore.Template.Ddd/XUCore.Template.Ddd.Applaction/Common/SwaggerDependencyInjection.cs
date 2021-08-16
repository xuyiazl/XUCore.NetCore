using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using XUCore.NetCore.Swagger;

namespace XUCore.Template.Ddd.Applaction
{
    public static class SwaggerDependencyInjection
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, IWebHostEnvironment environment)
        {
            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(ApiGroup.User, new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = $"User Api - {environment.EnvironmentName}",
                    Description = $"User Api - {environment.EnvironmentName}"
                });
                options.SwaggerDoc(ApiGroup.File, new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = $"File Api - {environment.EnvironmentName}",
                    Description = $"File Api - {environment.EnvironmentName}"
                });

                options.AddJwtBearerDoc();
                //options.AddHttpSignDoc(services);
                //options.AddFiledDoc();

                options.AddDescriptions(typeof(DependencyInjection),
                    "XUCore.Template.Ddd.Applaction.xml",
                    "XUCore.Template.Ddd.Domain.xml",
                    "XUCore.Template.Ddd.Domain.Core.xml"
                );

                // TODO:一定要返回true！true 分组无效 注释掉 必须有分组才能出现api
                //options.DocInclusionPredicate((docName, description) => true);
            });

            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
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

                c.SwaggerEndpoint($"/swagger/{ApiGroup.User}/swagger.json", "User API");
                c.SwaggerEndpoint($"/swagger/{ApiGroup.File}/swagger.json", "File API");

                c.DocExpansion(DocExpansion.None);
            });

            return app;
        }
    }
}
