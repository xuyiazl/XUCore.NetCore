using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XUCore.Net5.Template.Applaction;
using XUCore.Net5.Template.Infrastructure;
using XUCore.Net5.Template.Persistence;

namespace XUCore.Net5.Template.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public const string Project = "mvc";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration, Environment, Project);
            services.AddPersistence(Configuration);
            services.AddApplication(Environment, Project);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseInfrastructure(env, Project);
            app.UsePersistence();
            app.UseApplication(env, Project);
        }
    }
}
