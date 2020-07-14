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
        public static IServiceCollection AddNigelDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NigelDbEntityContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("XUCore_ReadConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
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
        public NigelDbEntityContext(DbContextOptions<NigelDbEntityContext> options) : base(typeof(NigelDbEntityContext), options, "sqlserver", $"XUCore.WebTests.Data.Mapping")
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
