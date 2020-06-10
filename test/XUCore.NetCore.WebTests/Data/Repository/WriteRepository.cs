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
        public WriteEntityContext(DbContextOptions<WriteEntityContext> options) : base(options, "sqlserver", $"XUCore.WebTests.Data.Mapping")
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

    public class WriteRepository<T> : MsSqlRepository<T>, IWriteRepository<T> where T : class, new()
    {
        public WriteRepository(IWriteEntityContext context) : base(context)
        {

        }
    }
}
