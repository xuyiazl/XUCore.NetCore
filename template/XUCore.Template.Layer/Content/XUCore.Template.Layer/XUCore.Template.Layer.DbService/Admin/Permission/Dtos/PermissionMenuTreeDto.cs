using System.Collections.Generic;

namespace XUCore.Template.Layer.DbService.Admin.Permission
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
