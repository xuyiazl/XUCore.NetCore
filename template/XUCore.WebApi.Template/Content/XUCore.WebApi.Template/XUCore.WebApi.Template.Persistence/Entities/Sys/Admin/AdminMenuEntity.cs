
using System;
using System.Collections.Generic;

namespace XUCore.WebApi.Template.Persistence.Entities.Sys.Admin
{
    public partial class AdminMenuEntity : BaseEntity
    {
        public AdminMenuEntity()
        {
            RoleMenus = new HashSet<AdminRoleMenuEntity>();
        }
        public long FatherId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string OnlyCode { get; set; }
        public bool IsMenu { get; set; }
        public int Weight { get; set; }
        public bool IsExpress { get; set; }


        public ICollection<AdminRoleMenuEntity> RoleMenus;
    }
}
