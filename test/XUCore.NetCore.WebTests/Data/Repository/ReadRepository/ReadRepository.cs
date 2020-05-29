using XUCore.NetCore.Data.DbService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XUCore.WebTests.Data.Repository.ReadRepository
{
    public class ReadRepository<TEntity> : MsSqlRepository<TEntity>, IReadRepository<TEntity> where TEntity : class, new()
    {
        public ReadRepository(IReadEntityContext context) : base(context)
        {

        }
    }
}
