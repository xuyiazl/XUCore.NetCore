using XUCore.Template.Ddd.Domain.Core;
using XUCore.NetCore.Data;
using AutoMapper;

namespace XUCore.Template.Ddd.Persistence
{
    public class DefaultDbRepository : DbContextRepository<IDefaultDbContext>, IDefaultDbRepository
    {
        public DefaultDbRepository(IDefaultDbContext context, IMapper mapper) : base(context, mapper) { }
    }
}
