using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using XUCore.NetCore.Data;
using XUCore.Template.Easy.Core.Enums;
using XUCore.Template.Easy.Persistence.Entities;
using XUCore.Template.Easy.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.Easy.Persistence
{
    public interface IDefaultDbRepository : IDbContextRepository<DefaultDbContext> { }

    public class DefaultDbRepository : DbContextRepository<DefaultDbContext>, IDefaultDbRepository
    {
        public DefaultDbRepository(DefaultDbContext context, IMapper mapper) : base(context, mapper) { }
    }

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

        #region [ 系统 ]

        public DbSet<AdminUserEntity> AdminUser => Set<AdminUserEntity>();
        public DbSet<AdminUserRoleEntity> AdminAuthUserRole => Set<AdminUserRoleEntity>();
        public DbSet<LoginRecordEntity> AdminLoginRecord => Set<LoginRecordEntity>();
        public DbSet<AdminRoleMenuEntity> AdminAuthRoleMenus => Set<AdminRoleMenuEntity>();
        public DbSet<AdminRoleEntity> AdminAuthRole => Set<AdminRoleEntity>();
        public DbSet<AdminMenuEntity> AdminAuthMenus => Set<AdminMenuEntity>();

        #endregion
    }
}