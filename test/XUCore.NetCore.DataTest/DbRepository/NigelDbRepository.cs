using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using XUCore.NetCore.Data;
using XUCore.NetCore.DataTest.Entities;

namespace XUCore.NetCore.DataTest.DbRepository
{
    public static partial class ServiceCollectionExtensions
    {
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder =>
            {
#if DEBUG
                builder.AddConsole();
#endif
            });

        public static IServiceCollection AddNigelDbContext(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>();

            services.AddDbContext<NigelDbContext>(options =>
            {
                options.UseSqlServer(
                    connectionString: config.GetConnectionString("NigelDB_Connection"),
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

            // 5.0.0 mysql 暂时支持的只有预览版

            //services.AddDbContext<NigelDbContext>(options =>
            //{
            //    options.UseMySql(
            //        connectionString: config.GetConnectionString("NigelDB_Connection"),
            //        serverVersion: new MySqlServerVersion(new Version(8, 0, 21)),
            //        mySqlOptionsAction: options =>
            //        {
            //            options.CharSetBehavior(CharSetBehavior.NeverAppend);
            //            options.EnableRetryOnFailure();
            //            //options.ExecutionStrategy(c => new MySqlRetryingExecutionStrategy(c.CurrentContext.Context));
            //            //options.ExecutionStrategy(c => new SqlServerRetryingExecutionStrategy(c.CurrentContext.Context));
            //        }
            //        )
            //    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            //    //options.UseLoggerFactory(MyLoggerFactory);
            //});

            services.AddScoped(typeof(INigelDbRepository<>), typeof(NigelDbRepository<>));
            services.AddScoped(typeof(INigelDbContextRepository), typeof(NigelDbContextRepository));

            return services;
        }
    }

    public class NigelDbContext : DBContextFactory
    {
        public NigelDbContext(DbContextOptions<NigelDbContext> options) : base(options) { }

        public DbSet<AdminUserEntity> User => Set<AdminUserEntity>();
        public DbSet<AdminUserAddressEntity> Address => Set<AdminUserAddressEntity>();
        public DbSet<TestKeyEntity> TestKey => Set<TestKeyEntity>();
    }

    public interface INigelDbRepository<TEntity> : IDbRepository<TEntity> where TEntity : class, new() { }
    public class NigelDbRepository<TEntity> : DbRepository<TEntity>, INigelDbRepository<TEntity> where TEntity : class, new()
    {
        public NigelDbRepository(NigelDbContext context) : base(context) { }
    }

    public interface INigelDbContextRepository : IDbContextRepository<NigelDbContext> { }
    public class NigelDbContextRepository : DbContextRepository<NigelDbContext>, INigelDbContextRepository
    {
        public NigelDbContextRepository(NigelDbContext context) : base(context) { }
    }
}
