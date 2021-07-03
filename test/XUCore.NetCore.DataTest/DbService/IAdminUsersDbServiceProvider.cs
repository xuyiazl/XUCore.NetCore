using System;
using System.Collections.Generic;
using System.Text;
using XUCore.NetCore.Data.DbService;
using XUCore.NetCore.DataTest.Entities;

namespace XUCore.NetCore.DataTest.DbService
{
    public interface IAdminUsersDbServiceProvider : IDbServiceProvider<AdminUserEntity>, IDbServiceProvider
    {

    }
}
