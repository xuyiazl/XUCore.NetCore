using FluentValidation;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using System.Collections.Generic;
using System.Linq;

namespace XUCore.Net5.Template.Domain.Sys.Permission
{
    internal static class View
    {
        /// <summary>
        /// 获取当前账号的权限导航列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="AdminID"></param>
        /// <returns></returns>
        public static IQueryable<AdminMenuEntity> Create(PermissionViewModel model, long AdminID)
        {
            return
                (
                    from userRoles in model.UserRoles

                    join roleMenus in model.RoleMenus on userRoles.RoleId equals roleMenus.RoleId

                    join menus in model.Menus on roleMenus.MenuId equals menus.Id

                    where userRoles.AdminId == AdminID

                    select menus
                )
                .Distinct().AsQueryable();
        }
    }
}
