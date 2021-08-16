using XUCore.Template.Ddd.Applaction;
using XUCore.Template.Ddd.Infrastructure;
using XUCore.Template.Ddd.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XUCore.Template.Ddd.Applaction;

namespace XUCore.Template.Ddd.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public const string Project = "api";

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
