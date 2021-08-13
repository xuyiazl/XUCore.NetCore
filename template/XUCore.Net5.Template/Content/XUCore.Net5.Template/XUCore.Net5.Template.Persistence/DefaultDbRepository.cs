using XUCore.Net5.Template.Domain.Core;
using XUCore.NetCore.Data;

namespace XUCore.Net5.Template.Persistence
{
    public class DefaultDbRepository : DbContextRepository<IDefaultDbContext>, IDefaultDbRepository
    {
        public DefaultDbRepository(IDefaultDbContext context) : base(context) { }
    }
}
