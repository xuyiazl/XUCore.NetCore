using System.Collections.Generic;

namespace WebApi.DbService.Sys.Admin.AdminMenu
{
    /// <summary>
    /// 导航列表
    /// </summary>
    public class AdminMenuTreeDto : AdminMenuDto
    {
        public IList<AdminMenuTreeDto> Child { get; set; }
    }
}
