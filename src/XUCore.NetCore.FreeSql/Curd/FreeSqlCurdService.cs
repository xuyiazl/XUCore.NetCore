using AutoMapper;
using System;
using System.Linq.Expressions;
using XUCore.Ddd.Domain;
using XUCore.Ddd.Domain.Commands;
using XUCore.NetCore.FreeSql.Entity;

namespace XUCore.NetCore.FreeSql.Curd
{
    /// <summary>
    /// FreeSql CurdService（单库FreeSql实例）
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TCreateCommand"></typeparam>
    /// <typeparam name="TUpdateCommand"></typeparam>
    /// <typeparam name="TListCommand"></typeparam>
    /// <typeparam name="TPageCommand"></typeparam>
    public class FreeSqlCurdService<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand> :
        CurdService<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand>
        where TKey : struct
        where TEntity : EntityFull<TKey>, new()
        where TDto : class, new()
        where TCreateCommand : CreateCommand
        where TUpdateCommand : UpdateCommand<TKey>
        where TListCommand : ListCommand
        where TPageCommand : PageCommand
    {
        protected FreeSqlCurdService(FreeSqlUnitOfWorkManager muowm, IMapper mapper, IUser user) : base(muowm.Orm, mapper)
        {
            muowm.Binding(this);

            User = user;
        }

        protected FreeSqlCurdService(FreeSqlUnitOfWorkManager muowm, IMapper mapper, IUser user, Expression<Func<TEntity, bool>> filter, Func<string, string> asTable = null) :
            base(muowm.Orm, mapper, filter, asTable)
        {
            muowm.Binding(this);

            User = user;
        }
    }
}
