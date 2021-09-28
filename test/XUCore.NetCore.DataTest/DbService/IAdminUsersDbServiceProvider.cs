using System;
using System.Collections.Generic;
using System.Text;
using XUCore.Ddd.Domain;
using XUCore.NetCore.Data;
using XUCore.NetCore.DataTest.Entities;

namespace XUCore.NetCore.DataTest.DbService
{
    public interface IAdminUsersDbServiceProvider : IDbServiceProvider<AdminUserEntity>, IScoped
    {

    }
}
