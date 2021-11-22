using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using XUCore.NetCore;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.RazorTests;

namespace XUCore.RazorTests
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
            //封装使用mssql数据库
            services.AddDbContext<TestDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DbConnection")));

            services.AddMvcAction();

            services.AddCacheInterceptor();

            services.AddScoped<ICacheTest, CacheTest>();

            services.AddRazorPages(options =>
            {
                //忽略XSRF验证
                options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            });
            //services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseStaticHttpContext();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
