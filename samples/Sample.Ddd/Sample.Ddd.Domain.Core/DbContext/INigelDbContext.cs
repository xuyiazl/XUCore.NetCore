using Sample.Ddd.Domain.Core.Entities;
using Sample.Ddd.Domain.Core.Entities.Sys.Admin;
using Microsoft.EntityFrameworkCore;
using XUCore.NetCore.Data.DbService;

namespace Sample.Ddd.Domain.Core
{
    public interface INigelDbContext : IDbContext
    {
        #region [ 系统 ]

        DbSet<AdminUserEntity> AdminUser { get; }
        DbSet<AdminUserRoleEntity> AdminAuthUserRole { get; }
        DbSet<LoginRecordEntity> AdminLoginRecord { get; }
        DbSet<AdminRoleMenuEntity> AdminAuthRoleMenus { get; }
        DbSet<AdminRoleEntity> AdminAuthRole { get; }
        DbSet<AdminMenuEntity> AdminAuthMenus { get; }

        #endregion
    }
}
