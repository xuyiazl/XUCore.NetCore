using Microsoft.EntityFrameworkCore;
using XUCore.NetCore.Data.DbService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace XUCore.WebTests.Data.Repository.ReadRepository
{
    public class ReadEntityContext : BaseRepositoryFactory, IReadEntityContext
    {
        public ReadEntityContext(DbContextOptions<ReadEntityContext> options) : base(options, "mysql", $"XUCore.WebTests.Data.Mapping")
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
}
