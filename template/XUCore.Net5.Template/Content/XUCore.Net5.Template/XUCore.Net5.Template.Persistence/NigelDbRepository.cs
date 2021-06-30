using XUCore.Net5.Template.Domain.Core;
using XUCore.NetCore.Data.DbService;

namespace XUCore.Net5.Template.Persistence
{
    public class NigelDbRepository : Repository<INigelDbContext>, INigelDbRepository
    {
        public NigelDbRepository(INigelDbContext context) : base(context) { }
    }
}
