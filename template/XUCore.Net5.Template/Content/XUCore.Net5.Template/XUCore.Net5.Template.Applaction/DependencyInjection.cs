using XUCore.Net5.Template.Applaction.Common.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace XUCore.Net5.Template.Applaction
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IWebHostEnvironment environment, string project = "api")
        {
            services.Scan(scan =>
                scan.FromAssemblyOf<IAppService>()
                .AddClasses(impl => impl.AssignableTo(typeof(IAppService)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            if (project == "api")
            {
                services.AddSwagger(environment);
            }

            return services;
        }

        public static IApplicationBuilder UseApplication(this IApplicationBuilder app, IWebHostEnvironment environment, string project = "api")
        {
            if (project == "api")
            {
                app.UseSwagger(environment);
            }

            return app;
        }
    }
}
