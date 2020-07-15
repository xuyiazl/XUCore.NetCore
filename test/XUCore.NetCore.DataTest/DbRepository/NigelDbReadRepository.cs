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
        public static IServiceCollection AddReadDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NigelDbReadEntityContext>(options =>
            {
                options.UseSqlServer(
                    connectionString: configuration.GetConnectionString("NigelDB_ReadConnection"),
                    sqlServerOptionsAction: options =>
                        {
                            options.EnableRetryOnFailure();
                        }
                    )
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped(typeof(INigelDbReadRepository<>), typeof(NigelDbReadRepository<>));
            services.AddScoped(typeof(INigelDbReadEntityContext), typeof(NigelDbReadEntityContext));

            return services;
        }
    }

    public interface INigelDbReadEntityContext : IDbContext
    {

    }

    public interface INigelDbReadRepository<T> : IMsSqlRepository<T> where T : class, new()
    {

    }

    public class NigelDbReadEntityContext : BaseRepositoryFactory, INigelDbReadEntityContext
    {
        public NigelDbReadEntityContext(DbContextOptions<NigelDbReadEntityContext> options) : base(typeof(NigelDbReadEntityContext), options, "sqlserver", $"XUCore.NetCore.DataTest.Mapping")
        {

        }
    }

    public class NigelDbReadRepository<TEntity> : MsSqlRepository<TEntity>, INigelDbReadRepository<TEntity> where TEntity : class, new()
    {
        public NigelDbReadRepository(INigelDbReadEntityContext context) : base(context)
        {

        }
    }
}
