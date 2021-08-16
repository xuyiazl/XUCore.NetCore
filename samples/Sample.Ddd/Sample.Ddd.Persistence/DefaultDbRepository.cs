using Sample.Ddd.Domain.Core;
using XUCore.NetCore.Data;

namespace Sample.Ddd.Persistence
{
    public class DefaultDbRepository : DbContextRepository<IDefaultDbContext>, IDefaultDbRepository
    {
        public DefaultDbRepository(IDefaultDbContext context) : base(context) { }
    }
}
