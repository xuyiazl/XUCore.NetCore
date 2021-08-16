using XUCore.Template.Ddd.Domain.Core;
using XUCore.NetCore.Data;

namespace XUCore.Template.Ddd.Persistence
{
    public class DefaultDbRepository : DbContextRepository<IDefaultDbContext>, IDefaultDbRepository
    {
        public DefaultDbRepository(IDefaultDbContext context) : base(context) { }
    }
}
