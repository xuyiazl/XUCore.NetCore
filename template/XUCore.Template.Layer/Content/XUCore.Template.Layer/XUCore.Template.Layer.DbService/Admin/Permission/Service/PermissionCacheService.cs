using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.Template.Layer.Core;
using XUCore.Template.Layer.Persistence;
using XUCore.Template.Layer.Persistence.Entities.Admin;

namespace XUCore.Template.Layer.DbService.Admin.Permission
{
    public class PermissionCacheService : IPermissionCacheService
    {
        private readonly IDefaultDbRepository<AdminMenuEntity> db;

        public PermissionCacheService(IDefaultDbRepository<AdminMenuEntity> db)
        {
            this.db = db;
        }

        [CacheMethod(Key = CacheKey.AuthUser, ParamterKey = "{0}", Seconds = CacheTime.Min5)]
        public async Task<IList<AdminMenuEntity>> GetAllAsync(long adminId, CancellationToken cancellationToken)
        {
            var res =
                    await
                    (
                        from userRoles in db.Context.Set<AdminUserRoleEntity>()

                        join roleMenus in db.Context.Set<AdminRoleMenuEntity>() on userRoles.RoleId equals roleMenus.RoleId

                        join menus in db.Context.Set<AdminMenuEntity>() on roleMenus.MenuId equals menus.Id

                        where userRoles.AdminId == adminId

                        select menus
                    )
                    .Distinct()
                    .ToListAsync(cancellationToken);

            return res;
        }
    }
}
