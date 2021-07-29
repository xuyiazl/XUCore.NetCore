﻿using Sample1.Persistence.Entities.Sys.Admin;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using XUCore.NetCore.Data.DbService;

namespace Sample1.Persistence
{
    public interface INigelDbRepository : IDbContextRepository<NigelDbContext> { }

    public class NigelDbRepository : DbContextRepository<NigelDbContext>, INigelDbRepository
    {
        public NigelDbRepository(NigelDbContext context) : base(context) { }
    }

    public class NigelDbContext : DBContextFactory
    {
        public NigelDbContext(DbContextOptions<NigelDbContext> options) : base(options)
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