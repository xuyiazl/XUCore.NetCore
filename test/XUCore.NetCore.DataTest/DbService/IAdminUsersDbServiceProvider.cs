using System;
using System.Collections.Generic;
using System.Text;
using XUCore.NetCore.Data.DbService.ServiceProvider;
using XUCore.NetCore.DataTest.Entities;

namespace XUCore.NetCore.DataTest.DbService
{
    public interface IAdminUsersDbServiceProvider : IDbServiceBase<AdminUsersEntity>, IDbServiceProvider
    {

    }
}
