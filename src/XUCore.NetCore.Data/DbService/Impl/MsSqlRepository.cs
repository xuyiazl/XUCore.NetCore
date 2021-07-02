using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using XUCore.Extensions.Datas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.Common;
using XUCore.Extensions;
using System.Linq;
using XUCore.Helpers;

namespace XUCore.NetCore.Data.DbService
{
    public class MsSqlRepository<TEntity> : DbRepository<TEntity>, IMsSqlRepository<TEntity> where TEntity : class, new()
    {
        public MsSqlRepository(IDbContext context) : base(context) { }

    }
}
