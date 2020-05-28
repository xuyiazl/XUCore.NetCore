using XUCore.Paging;
using XUCore.WebTests.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.Data.DbService;

namespace XUCore.WebTests.Data.DbService
{
    public interface IDbAdminUsersServiceProvider : IDbServiceBase<AdminUsers>, IDbDependencyService
    {
    }
}