using Sample.Ddd.Domain.Core;
using XUCore.NetCore.Data;

namespace Sample.Ddd.Persistence
{
    public class NigelDbRepository : DbContextRepository<INigelDbContext>, INigelDbRepository
    {
        public NigelDbRepository(INigelDbContext context) : base(context) { }
    }
}
