using XUCore.SimpleApi.Template.Persistence.Entities.Sys.Admin;
using System.Collections.Generic;

namespace XUCore.SimpleApi.Template.Applaction.Permission
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
