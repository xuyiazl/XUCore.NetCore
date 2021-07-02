using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using XUCore.Extensions;
using XUCore.Extensions.Datas;
using XUCore.Helpers;

namespace XUCore.NetCore.Data.DbService
{
    public class MySqlRepository<TEntity> : DbRepository<TEntity>, IMySqlRepository<TEntity> where TEntity : class, new()
    {
        public MySqlRepository(IDbContext context) : base(context) { }

    }
}
