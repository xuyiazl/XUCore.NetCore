﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.DynamicWebApi;
using XUCore.Template.Easy.Core;
using XUCore.Template.Easy.Persistence;
using XUCore.Template.Easy.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.Easy.Applaction.Permission
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
                        from userRoles in db.Context.AdminUserRole

                        join roleMenus in db.Context.AdminRoleMenu on userRoles.RoleId equals roleMenus.RoleId

                        join menus in db.Context.AdminMenu on roleMenus.MenuId equals menus.Id

                        where userRoles.AdminId == adminId

                        select menus
                    )
                    .Distinct()
                    .ToListAsync(cancellationToken);

            return res;
        }
    }
}
