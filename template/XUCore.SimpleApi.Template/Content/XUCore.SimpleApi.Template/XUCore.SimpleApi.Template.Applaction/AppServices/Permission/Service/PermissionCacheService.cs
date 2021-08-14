using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.DynamicWebApi;
using XUCore.SimpleApi.Template.Core;
using XUCore.SimpleApi.Template.Core.Enums;
using XUCore.SimpleApi.Template.Persistence;
using XUCore.SimpleApi.Template.Persistence.Entities.Sys.Admin;

namespace XUCore.SimpleApi.Template.Applaction.Permission
{
    [NonDynamicWebApi]
    public class PermissionCacheService : IPermissionCacheService
    {
        private readonly IDefaultDbRepository db;
        private readonly IMapper mapper;

        public PermissionCacheService(IDefaultDbRepository db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        [CacheMethod(Key = CacheKey.AuthUser, ParamterKey = "{0}", Seconds = CacheTime.Min5)]
        public async Task<IList<AdminMenuEntity>> GetAllAsync(long adminId, CancellationToken cancellationToken)
        {
            var res =
                     await
                     (
                         from userRoles in db.Context.AdminAuthUserRole

                         join roleMenus in db.Context.AdminAuthRoleMenus on userRoles.RoleId equals roleMenus.RoleId

                         join menus in db.Context.AdminAuthMenus on roleMenus.MenuId equals menus.Id

                         where userRoles.AdminId == adminId && menus.Status == Status.Show

                         select menus
                     )
                     .Distinct()
                     .ToListAsync(cancellationToken);

            return res;
        }
    }
}
