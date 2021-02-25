using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.NetCore.Data.DbService;

namespace XUCore.NetCore.DataTest.DbRepository
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWriteDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NigelDbWriteEntityContext>(options =>
            {
                options.UseSqlServer(
                    connectionString: configuration.GetConnectionString("NigelDB_WriteConnection"),
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

    public interface INigelDbWriteEntityContext : IDbContext
    {
    }

    public interface INigelDbWriteRepository<T> : IMsSqlRepository<T> where T : class, new()
    {

    }

    public class NigelDbWriteEntityContext : BaseRepositoryFactory, INigelDbWriteEntityContext
    {
        public NigelDbWriteEntityContext(DbContextOptions<NigelDbWriteEntityContext> options) : base(typeof(NigelDbWriteEntityContext), options, "sqlserver", $"XUCore.NetCore.DataTest.Mapping")
        {

        }
    }

    public class NigelDbWriteRepository<T> : MsSqlRepository<T>, INigelDbWriteRepository<T> where T : class, new()
    {
        public NigelDbWriteRepository(INigelDbWriteEntityContext context) : base(context)
        {

        }
    }
}
