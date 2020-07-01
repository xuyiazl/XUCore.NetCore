using XUCore.NetCore.Data.DbService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace XUCore.WebTests.Data.Repository
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWriteDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<WriteEntityContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("XUCore_WriteConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped(typeof(IWriteEntityContext), typeof(WriteEntityContext));

            return services;
        }
    }

    public interface IWriteEntityContext : IDbContext
    {
    }

    public interface IWriteRepository<T> : IMsSqlRepository<T> where T : class, new()
    {

    }

    public class WriteEntityContext : BaseRepositoryFactory, IWriteEntityContext
    {
        public WriteEntityContext(DbContextOptions<WriteEntityContext> options) : base(typeof(WriteEntityContext), options, "sqlserver", $"XUCore.WebTests.Data.Mapping")
        {

        }
    }

    public class WriteRepository<T> : MsSqlRepository<T>, IWriteRepository<T> where T : class, new()
    {
        public WriteRepository(IWriteEntityContext context) : base(context)
        {

        }
    }
}
