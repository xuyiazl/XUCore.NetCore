using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace XUCore.Template.EasyLayer.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            //mssql

            //services.AddDbContext<NigelDbContext>(options =>
            //{
            //    options.UseSqlServer(
            //        connectionString: configuration.GetConnectionString("DBConnection-Mssql"),
            //        sqlServerOptionsAction: options =>
            //        {
            //            options.MigrationsAssembly("XUCore.Template.EasyLayer.Persistence");
            //        }
            //        )
            //    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            //    //options.UseLoggerFactory(MyLoggerFactory);
            //});

            // mysql

            services.AddDbContext<DefaultDbContext>(options =>
            {
                options.UseMySql(
                    connectionString: configuration.GetConnectionString("DBConnection-Mysql"),
                    serverVersion: new MySqlServerVersion(new Version(5, 7, 29))
                )
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                //options.UseLoggerFactory(MyLoggerFactory);
            });

            services.AddScoped(typeof(IDefaultDbRepository<>), typeof(DefaultDbRepository<>));

            return services;
        }

        public static IApplicationBuilder UseDbContext(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var dbContext = scope.ServiceProvider.GetService<DefaultDbContext>();

#if !DEBUG
            dbContext.Database.Migrate();
#endif
            return app;
        }
    }
}
