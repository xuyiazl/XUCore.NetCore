using XUCore.NetCore.Data.DbService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace XUCore.WebTests.Data.Repository.WriteRepository
{
    public class WriteRepository<T> : MsSqlRepository<T>, IWriteRepository<T> where T : class, new()
    {
        public WriteRepository(IWriteEntityContext context) : base(context)
        {

        }

    }
}
