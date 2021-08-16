using System.Collections.Generic;

namespace Sample.Ddd.Domain.Auth.Permission
{
    public class PermissionMenuTreeDto : PermissionMenuDto
    {
        public IList<PermissionMenuTreeDto> Child { get; set; }
    }
}
