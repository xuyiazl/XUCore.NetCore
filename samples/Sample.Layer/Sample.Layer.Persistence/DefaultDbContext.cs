﻿using Sample.Layer.Persistence.Entities.Sys.Admin;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using XUCore.NetCore.Data;

namespace Sample.Layer.Persistence
{
    public interface IDefaultDbRepository : IDbContextRepository<DefaultDbContext> { }

    public class DefaultDbRepository : DbContextRepository<DefaultDbContext>, IDefaultDbRepository
    {
        public DefaultDbRepository(DefaultDbContext context) : base(context) { }
    }

    public class DefaultDbContext : DBContextFactory
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {

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