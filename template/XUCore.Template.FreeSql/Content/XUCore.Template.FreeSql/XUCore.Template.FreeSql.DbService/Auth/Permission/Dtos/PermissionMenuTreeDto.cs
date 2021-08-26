using System.Collections.Generic;

namespace XUCore.Template.FreeSql.DbService.Auth.Permission
{
    /// <summary>
    /// 权限导航
    /// </summary>
    public class PermissionMenuTreeDto : PermissionMenuDto
    {
        /// <summary>
        /// 子集导航
        /// </summary>

        public IList<PermissionMenuTreeDto> Child { get; set; }
    }
}
