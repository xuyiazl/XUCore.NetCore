using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using XUCore.NetCore.Data.DbService;

namespace XUCore.NetCore.DataTest.DbRepository
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReadDbContext(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>();

            services.AddDbContext<NigelReadDbContext>(options =>
            {
                options.UseSqlServer(
                    connectionString: config.GetConnectionString("NigelDB_ReadConnection"),
                    sqlServerOptionsAction: options =>
                        {
                            options.EnableRetryOnFailure();
                            //options.ExecutionStrategy(c => new MySqlRetryingExecutionStrategy(c.CurrentContext.Context));
                            //options.ExecutionStrategy(c => new SqlServerRetryingExecutionStrategy(c.CurrentContext.Context));
                        }
                    )
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped(typeof(INigelReadDbRepository<>), typeof(NigelReadDbRepository<>));
            services.AddScoped(typeof(INigelReadDbContext), typeof(NigelReadDbContext));

            return services;
        }
    }

    public interface INigelReadDbContext : IDbContext { }
    public class NigelReadDbContext : DBContextFactory, INigelReadDbContext
    {
        public NigelReadDbContext(DbContextOptions<NigelReadDbContext> options) : base(options) { }
    }

    public interface INigelReadDbRepository<TEntity> : IMsSqlRepository<TEntity> where TEntity : class, new() { }
    public class NigelReadDbRepository<TEntity> : MsSqlRepository<TEntity>, INigelReadDbRepository<TEntity> where TEntity : class, new()
    {
        public NigelReadDbRepository(INigelReadDbContext context) : base(context) { }
    }
}
