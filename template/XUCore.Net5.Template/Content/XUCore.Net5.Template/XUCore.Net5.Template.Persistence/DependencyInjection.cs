using XUCore.Net5.Template.Domain.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace XUCore.Net5.Template.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            //mssql

            //services.AddDbContext<NigelDbContext>(options =>
            //{
            //    options.UseSqlServer(
            //        connectionString: configuration.GetConnectionString("NigelDBConnection"),
            //        sqlServerOptionsAction: options =>
            //        {
            //            options.EnableRetryOnFailure();
            //            //options.ExecutionStrategy(c => new MySqlRetryingExecutionStrategy(c.CurrentContext.Context));
            //            //options.ExecutionStrategy(c => new SqlServerRetryingExecutionStrategy(c.CurrentContext.Context));
            //            options.MigrationsAssembly("XUCore.Net5.Template.Persistence");
            //        }
            //        )
            //    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            //    //options.UseLoggerFactory(MyLoggerFactory);
            //});

            // mysql

            services.AddDbContext<NigelDbContext>(options =>
            {
                options.UseMySql(
                    connectionString: configuration.GetConnectionString("NigelDBConnection-mysql"),
                    serverVersion: new MySqlServerVersion(new Version(5, 7, 29)),
                    mySqlOptionsAction: options =>
                    {
                        //options.CharSetBehavior(CharSetBehavior.NeverAppend);
                        options.EnableRetryOnFailure();
                        //options.ExecutionStrategy(c => new MySqlRetryingExecutionStrategy(c.CurrentContext.Context));
                        //options.ExecutionStrategy(c => new SqlServerRetryingExecutionStrategy(c.CurrentContext.Context));
                    }
                    )
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                //options.UseLoggerFactory(MyLoggerFactory);
            });

            services.AddScoped(typeof(INigelDbContext), typeof(NigelDbContext));
            services.AddScoped(typeof(INigelDbRepository), typeof(NigelDbRepository));

            return services;
        }

        public static IApplicationBuilder UsePersistence(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var dbContext = scope.ServiceProvider.GetService<NigelDbContext>();

            dbContext.Database.Migrate();

            return app;
        }
    }
}
