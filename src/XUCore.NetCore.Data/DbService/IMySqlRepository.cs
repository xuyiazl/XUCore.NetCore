using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XUCore.NetCore.Data.DbService
{

    public interface IMySqlRepository<TEntity> : IDbRepository<TEntity> where TEntity : class, new()
    {

    }
}
