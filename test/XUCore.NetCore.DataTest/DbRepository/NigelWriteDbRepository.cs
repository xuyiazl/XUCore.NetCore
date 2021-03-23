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
        public static IServiceCollection AddWriteDbContext(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>();

            services.AddDbContext<NigelWriteDbContext>(options =>
            {
                options.UseSqlServer(
                    connectionString: config.GetConnectionString("NigelDB_WriteConnection"),
                    sqlServerOptionsAction: options =>
                        {
                            options.EnableRetryOnFailure();
                            //options.ExecutionStrategy(c => new MySqlRetryingExecutionStrategy(c.CurrentContext.Context));
                            //options.ExecutionStrategy(c => new SqlServerRetryingExecutionStrategy(c.CurrentContext.Context));
                        }
                    )
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddScoped(typeof(INigelWriteDbRepository<>), typeof(WriteNigelDbRepository<>));
            services.AddScoped(typeof(INigelWriteDbContext), typeof(NigelWriteDbContext));

            return services;
        }
    }


    public interface INigelWriteDbRepository<TEntity> : IMsSqlRepository<TEntity> where TEntity : class, new() { }
    public class NigelWriteDbContext : DBContextFactory, INigelWriteDbContext
    {
        public NigelWriteDbContext(DbContextOptions<NigelWriteDbContext> options): base(options) { }
    }

    public interface INigelWriteDbContext : IDbContext { }
    public class WriteNigelDbRepository<TEntity> : MsSqlRepository<TEntity>, INigelWriteDbRepository<TEntity> where TEntity : class, new()
    {
        public WriteNigelDbRepository(INigelWriteDbContext context) : base(context) { }
    }
}
