using XUCore.NetCore.Data.DbService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XUCore.WebTests.Data.Repository.WriteRepository
{
    public class WriteRepository<T> : MySqlRepository<T>, IWriteRepository<T> where T : class, new()
    {
        public WriteRepository(IWriteEntityContext context) : base(context)
        {

        }
    }
}
