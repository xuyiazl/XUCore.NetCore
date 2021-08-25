using XUCore.Template.Ddd.Domain.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace XUCore.Template.Ddd.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            //mssql

            //services.AddDbContext<NigelDbContext>(options =>
            //{
            //    options.UseSqlServer(
            //        connectionString: configuration.GetConnectionString("DefaultDBConnection"),
            //        sqlServerOptionsAction: options =>
            //        {
            //            options.MigrationsAssembly("XUCore.Template.Ddd.Persistence");
            //        }
            //        )
            //    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            //    //options.UseLoggerFactory(MyLoggerFactory);
            //});

            // mysql

            services.AddDbContext<DefaultDbContext>(options =>
            {
                options.UseMySql(
                    connectionString: configuration.GetConnectionString("DefaultDBConnection-mysql"),
                    serverVersion: new MySqlServerVersion(new Version(5, 7, 29))
                    )
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                //options.UseLoggerFactory(MyLoggerFactory);
            });

            services.AddScoped(typeof(IDefaultDbContext), typeof(DefaultDbContext));
            services.AddScoped(typeof(IDefaultDbRepository), typeof(DefaultDbRepository));

            return services;
        }

        public static IApplicationBuilder UsePersistence(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var dbContext = scope.ServiceProvider.GetService<DefaultDbContext>();

            dbContext.Database.Migrate();

            return app;
        }
    }
}
