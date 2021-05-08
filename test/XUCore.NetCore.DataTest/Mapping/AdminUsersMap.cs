using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.NetCore.Data.DbService;
using XUCore.NetCore.DataTest.Entities;

namespace XUCore.NetCore.DataTest.Mapping
{
    public class AdminUsersMap : BaseMapping<AdminUserEntity>
    {
        public AdminUsersMap() : base("AdminUser", t => t.Id)
        {

        }
    }
}
