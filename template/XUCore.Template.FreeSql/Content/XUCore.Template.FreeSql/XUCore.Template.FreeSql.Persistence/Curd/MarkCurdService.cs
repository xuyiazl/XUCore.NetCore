using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.NetCore.Data;
using XUCore.Template.FreeSql.Core.Auth;
using XUCore.Template.FreeSql.Persistence.Entities;

namespace XUCore.Template.FreeSql.Persistence
{
    public class MarkCurdService<TMark, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand> :
        CurdService<TEntity, long, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand>
        where TEntity : EntityFull<long>, new()
        where TCreateCommand : CreateCommand
        where TUpdateCommand : UpdateCommand<long>
        where TListCommand : ListCommand
        where TPageCommand : PageCommand
    {
        protected MarkCurdService(MarkUnitOfWorkManager<TMark> muowm, IMapper mapper, IUser user) : base(muowm.Orm, mapper)
        {
            muowm.Binding(this);

            User = user;
        }
    }
}
