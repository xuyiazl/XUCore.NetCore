using System.Collections.Generic;

namespace XUCore.Template.Ddd.Domain.Auth.Permission
{
    public class PermissionMenuTreeDto : PermissionMenuDto
    {
        public IList<PermissionMenuTreeDto> Child { get; set; }
    }
}
