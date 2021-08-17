using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using XUCore.NetCore.Swagger;

namespace XUCore.Template.Ddd.Infrastructure
{
    public static class SwaggerDependencyInjection
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddMiniSwagger(swaggerGenAction: opt =>
            {
                opt.SwaggerDoc(ApiGroup.User, new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = $"User Api - {environment.EnvironmentName}",
                    Description = $"User Api - {environment.EnvironmentName}"
                });
                opt.SwaggerDoc(ApiGroup.File, new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = $"File Api - {environment.EnvironmentName}",
                    Description = $"File Api - {environment.EnvironmentName}"
                });

                opt.AddJwtBearerDoc();

                opt.AddDescriptions(typeof(DependencyInjection),
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
            app.UseMiniSwagger(swaggerUIAction: (opt) =>
            {
                opt.SwaggerEndpoint($"/swagger/{ApiGroup.User}/swagger.json", "User API");
                opt.SwaggerEndpoint($"/swagger/{ApiGroup.File}/swagger.json", "File API");
            });

            return app;
        }
    }
}
