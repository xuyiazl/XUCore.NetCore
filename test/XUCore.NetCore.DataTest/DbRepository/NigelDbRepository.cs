using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.NetCore.Data.BulkExtensions;
using XUCore.NetCore.Data.DbService;

namespace XUCore.NetCore.DataTest.DbRepository
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNigelDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NigelDbEntityContext>(options =>
            {
                options.UseSqlServer(
                    connectionString: configuration.GetConnectionString("NigelDB_Connection"),
                    sqlServerOptionsAction: options =>
                        {
                            options.EnableRetryOnFailure();
                            //options.ExecutionStrategy(c => new MySqlRetryingExecutionStrategy(c.CurrentContext.Context));
                            //options.ExecutionStrategy(c => new SqlServerRetryingExecutionStrategy(c.CurrentContext.Context));
                        }
                    )
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped(typeof(INigelDbRepository<>), typeof(NigelDbRepository<>));
            services.AddScoped(typeof(INigelDbEntityContext), typeof(NigelDbEntityContext));

            return services;
        }
    }

    public interface INigelDbEntityContext : IDbContext
    {

    }

    public interface INigelDbRepository<T> : IMsSqlRepository<T> where T : class, new()
    {

    }

    public class NigelDbEntityContext : BaseRepositoryFactory, INigelDbEntityContext
    {
        public NigelDbEntityContext(DbContextOptions<NigelDbEntityContext> options)
            : base(typeof(NigelDbEntityContext), options, DbServer.SqlServer, $"XUCore.NetCore.DataTest.Mapping")
        {

        }
    }

    public class NigelDbRepository<TEntity> : MsSqlRepository<TEntity>, INigelDbRepository<TEntity> where TEntity : class, new()
    {
        public NigelDbRepository(INigelDbEntityContext context) : base(context)
        {

        }
    }
}
