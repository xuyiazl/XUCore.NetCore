using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using XUCore.NetCore.Data;
using XUCore.Template.Easy.Core.Enums;
using XUCore.Template.Easy.Persistence.Entities;

namespace XUCore.Template.Easy.Persistence
{
    /// <summary>
    /// 数据仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IDefaultDbRepository<TEntity> : IDbRepository<TEntity> where TEntity : class, new() { }
    /// <summary>
    /// 数据仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DefaultDbRepository<TEntity> : DbRepository<TEntity>, IDefaultDbRepository<TEntity> where TEntity : class, new()
    {
        public DefaultDbRepository(DefaultDbContext context, IMapper mapper) : base(context, mapper) { }
    }
    /// <summary>
    /// 上下文
    /// </summary>
    public class DefaultDbContext : DBContextFactory
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
            SavingChanges += DefaultDbContext_SavingChanges;
        }

        private void DefaultDbContext_SavingChanges(object sender, SavingChangesEventArgs e)
        {
            ChangeTracker.Entries().Where(e => e.Entity is BaseEntity<long>).ToList().ForEach(e =>
            {
                //添加操作
                if (e.State == EntityState.Added)
                {
                    if (e.Entity is BaseEntity<long>)
                    {
                        var entity = e.Entity as BaseEntity<long>;
                        entity.CreatedAt = DateTime.Now;
                    }
                }
                //修改操作
                if (e.State == EntityState.Modified)
                {
                    if (e.Entity is BaseEntity<long>)
                    {
                        var entity = e.Entity as BaseEntity<long>;
                        switch (entity.Status)
                        {
                            case Status.Default:
                            case Status.Show:
                            case Status.SoldOut:
                                entity.UpdatedAt = DateTime.Now;
                                break;
                            case Status.Trash:
                                entity.DeletedAt = DateTime.Now;
                                break;
                        }
                    }
                }
            });
        }

        public override Assembly[] Assemblies => new Assembly[] { Assembly.GetExecutingAssembly() };
    }
}