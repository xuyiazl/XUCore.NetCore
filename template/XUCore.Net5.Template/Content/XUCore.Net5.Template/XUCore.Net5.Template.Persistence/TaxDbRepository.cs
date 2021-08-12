using XUCore.Net5.Template.Domain.Core;
using XUCore.NetCore.Data;

namespace XUCore.Net5.Template.Persistence
{
    public class TaxDbRepository : DbContextRepository<ITaxDbContext>, ITaxDbRepository
    {
        public TaxDbRepository(ITaxDbContext context) : base(context) { }
    }
}
