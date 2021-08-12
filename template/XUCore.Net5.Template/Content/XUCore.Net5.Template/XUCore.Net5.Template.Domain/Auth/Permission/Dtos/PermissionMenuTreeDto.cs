using System.Collections.Generic;

namespace XUCore.Net5.Template.Domain.Auth.Permission
{
    public class PermissionMenuTreeDto : PermissionMenuDto
    {
        public IList<PermissionMenuTreeDto> Child { get; set; }
    }
}
