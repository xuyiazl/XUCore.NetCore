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
        public static IServiceCollection AddWriteDbContext(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>();

            services.AddDbContext<NigelDbWriteEntityContext>(options =>
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
            services.AddScoped(typeof(INigelDbWriteRepository<>), typeof(NigelDbWriteRepository<>));
            services.AddScoped(typeof(INigelDbWriteEntityContext), typeof(NigelDbWriteEntityContext));

            return services;
        }
    }


    public interface INigelDbWriteRepository<TEntity> : IMsSqlRepository<TEntity> where TEntity : class, new() { }
    public class NigelDbWriteEntityContext : BaseRepositoryFactory, INigelDbWriteEntityContext
    {
        public NigelDbWriteEntityContext(DbContextOptions<NigelDbWriteEntityContext> options)
            : base(typeof(NigelDbWriteEntityContext), options, DbServer.SqlServer, $"XUCore.NetCore.DataTest.Mapping") { }
    }

    public interface INigelDbWriteEntityContext : IDbContext { }
    public class NigelDbWriteRepository<TEntity> : MsSqlRepository<TEntity>, INigelDbWriteRepository<TEntity> where TEntity : class, new()
    {
        public NigelDbWriteRepository(INigelDbWriteEntityContext context) : base(context) { }
    }
}
