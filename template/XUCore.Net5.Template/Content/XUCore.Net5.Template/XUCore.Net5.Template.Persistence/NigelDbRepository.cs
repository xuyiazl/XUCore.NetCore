using XUCore.Net5.Template.Domain.Core;
using XUCore.NetCore.Data.DbService;

namespace XUCore.Net5.Template.Persistence
{
    public class NigelDbRepository : DbContextRepository<INigelDbContext>, INigelDbRepository
    {
        public NigelDbRepository(INigelDbContext context) : base(context) { }
    }
}
