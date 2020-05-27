using XUCore.NetCore.Data.DbService;
using XUCore.WebTests.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XUCore.WebTests.Data.Mapping
{
    public class AdminUsersMap : AbstractEntityTypeConfiguration<AdminUsers>
    {
        public AdminUsersMap() : base("AdminUsers", t => t.Id)
        {
            SetIndentity(t => t.Id);
        }
    }
}
