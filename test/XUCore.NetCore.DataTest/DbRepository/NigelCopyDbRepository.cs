using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.NetCore.Data.BulkExtensions;
using XUCore.NetCore.Data.DbService;

namespace XUCore.NetCore.DataTest.DbRepository
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNigelCopyDbContext(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>();

            services.AddDbContext<NigelCopyDbEntityContext>(options =>
            {
                options.UseSqlServer(
                    connectionString: config.GetConnectionString("NigelCopyDB_Connection"),
                    sqlServerOptionsAction: options =>
                        {
                            options.EnableRetryOnFailure();
                            //options.ExecutionStrategy(c => new MySqlRetryingExecutionStrategy(c.CurrentContext.Context));
                            //options.ExecutionStrategy(c => new SqlServerRetryingExecutionStrategy(c.CurrentContext.Context));
                        }
                    )
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                //options.UseLoggerFactory(MyLoggerFactory);
            });

            services.AddScoped(typeof(INigelCopyDbRepository<>), typeof(NigelCopyDbRepository<>));
            services.AddScoped(typeof(INigelCopyDbEntityContext), typeof(NigelCopyDbEntityContext));

            return services;
        }
    }

    public interface INigelCopyDbEntityContext : IDbContext { }
    public class NigelCopyDbEntityContext : BaseRepositoryFactory, INigelCopyDbEntityContext
    {
        public NigelCopyDbEntityContext(DbContextOptions<NigelCopyDbEntityContext> options)
            : base(typeof(NigelCopyDbEntityContext), options, DbServer.SqlServer, $"XUCore.NetCore.DataTest.Mapping") { }
    }

    public interface INigelCopyDbRepository<TEntity> : IMsSqlRepository<TEntity> where TEntity : class, new() { }
    public class NigelCopyDbRepository<TEntity> : MsSqlRepository<TEntity>, INigelCopyDbRepository<TEntity> where TEntity : class, new()
    {
        public NigelCopyDbRepository(INigelCopyDbEntityContext context) : base(context) { }
    }
}
