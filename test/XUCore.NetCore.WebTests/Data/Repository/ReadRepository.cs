using XUCore.NetCore.Data.DbService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace XUCore.WebTests.Data.Repository
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReadDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReadEntityContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("XUCore_ReadConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IReadEntityContext), typeof(ReadEntityContext));

            return services;
        }
    }

    public interface IReadEntityContext : IDbContext
    {

    }

    public interface IReadRepository<T> : IMsSqlRepository<T> where T : class, new()
    {

    }

    public class ReadEntityContext : BaseRepositoryFactory, IReadEntityContext
    {
        public ReadEntityContext(DbContextOptions<ReadEntityContext> options) : base(typeof(ReadEntityContext), options, "sqlserver", $"XUCore.WebTests.Data.Mapping")
        {

        }
    }

    public class ReadRepository<TEntity> : MsSqlRepository<TEntity>, IReadRepository<TEntity> where TEntity : class, new()
    {
        public ReadRepository(IReadEntityContext context) : base(context)
        {

        }
    }
}
