using XUCore.NetCore.Data.DbService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XUCore.WebTests.Data.Repository.ReadRepository
{
    public class ReadRepository<T> : MySqlRepository<T>, IReadRepository<T> where T : class, new()
    {
        public ReadRepository(IReadEntityContext context) : base(context)
        {

        }
    }
}
