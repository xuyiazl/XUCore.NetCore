using XUCore.Template.Easy.Persistence.Entities.Admin;
using System.Collections.Generic;

namespace XUCore.Template.Easy.Applaction.Permission
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
