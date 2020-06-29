using XUCore.NetCore.Data.DbService;
using XUCore.Paging;
using XUCore.WebTests.Data.Entity;
using XUCore.WebTests.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using XUCore.NetCore.Data.DbService.ServiceProvider;

namespace XUCore.WebTests.Data.DbService
{
    public class DbAdminUsersServiceProvider : DbServiceBaseProvider<AdminUsers>, IDbAdminUsersServiceProvider
    {
        public DbAdminUsersServiceProvider(IReadRepository<AdminUsers> readRepository, IWriteRepository<AdminUsers> writeRepository)
            : base(readRepository, writeRepository)
        {
            
        }

    }
}
