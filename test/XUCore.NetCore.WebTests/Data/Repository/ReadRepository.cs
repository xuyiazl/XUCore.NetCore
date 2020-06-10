using XUCore.NetCore.Data.DbService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace XUCore.WebTests.Data.Repository.ReadRepository
{
    public static class ServiceCollectionExtensions
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
        public ReadEntityContext(DbContextOptions<ReadEntityContext> options) : base(options, "sqlserver", $"XUCore.WebTests.Data.Mapping")
        {

        }

        /// <summary>
        /// EF依赖mappingPath，将当前项目文件夹的Entity的映射文件执行注入操作
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //扫描指定文件夹的
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
           .Where(type => !String.IsNullOrEmpty(type.Namespace))
           .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(AbstractEntityTypeConfiguration<>));
            string namespce = mappingPath;
            typesToRegister = typesToRegister.Where(a => a.Namespace.Contains(namespce));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
        }
    }

    public class ReadRepository<TEntity> : MsSqlRepository<TEntity>, IReadRepository<TEntity> where TEntity : class, new()
    {
        public ReadRepository(IReadEntityContext context) : base(context)
        {

        }
    }
}
