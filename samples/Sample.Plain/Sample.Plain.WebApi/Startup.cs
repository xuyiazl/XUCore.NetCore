using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Plain.Applaction;
using Sample.Plain.Persistence;
using Sample.Plain.DbService;
using Sample.Plain.WebApi.Controller;

namespace Sample.Plain.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext(Configuration);
            services.AddApplication(Configuration);
            services.AddSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDbContext();
            app.UseApplication(env);
            app.UseSwagger();
        }
    }
}
