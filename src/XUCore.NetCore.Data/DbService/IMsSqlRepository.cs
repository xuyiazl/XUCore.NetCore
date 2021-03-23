﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XUCore.NetCore.Data.DbService
{
    public interface IMsSqlRepository<TEntity> : IDbRepository<TEntity> where TEntity : class, new()
    {

    }
}
