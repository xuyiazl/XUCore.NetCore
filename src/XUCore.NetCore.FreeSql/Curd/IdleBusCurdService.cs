using AutoMapper;
using XUCore.Ddd.Domain;
using XUCore.NetCore.FreeSql.Entity;

namespace XUCore.NetCore.FreeSql.Curd
{
    /// <summary>
    /// IdleBus FreeSql CurdService（租户FreeSql实例）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class IdleBusCurdService<TKey, TEntity> : CurdService<TKey, TEntity> where TEntity : EntityFull<TKey>, new()
    {
        /// <summary>
        /// IdleBus FreeSql CurdService（租户FreeSql实例）
        /// </summary>
        /// <param name="muowm"></param>
        /// <param name="mapper"></param>
        /// <param name="user"></param>
        protected IdleBusCurdService(IdleBusUnitOfWorkManager muowm, IMapper mapper, IUser user) : base(muowm.Orm, mapper)
        {
            muowm.Binding(repo);
        }
    }
    /// <summary>
    /// IdleBus FreeSql CurdService（租户FreeSql实例）
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TCreateCommand"></typeparam>
    /// <typeparam name="TUpdateCommand"></typeparam>
    /// <typeparam name="TListCommand"></typeparam>
    /// <typeparam name="TPageCommand"></typeparam>
    public class IdleBusCurdService<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand> :
        CurdService<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand>

        where TEntity : EntityFull<TKey>, new()
        where TDto : class, new()
        where TCreateCommand : CreateCommand
        where TUpdateCommand : UpdateCommand<TKey>
        where TListCommand : ListCommand
        where TPageCommand : PageCommand
    {
        /// <summary>
        /// IdleBus FreeSql CurdService（租户FreeSql实例）
        /// </summary>
        /// <param name="muowm"></param>
        /// <param name="mapper"></param>
        /// <param name="user"></param>
        protected IdleBusCurdService(IdleBusUnitOfWorkManager muowm, IMapper mapper, IUser user) : base(muowm.Orm, mapper)
        {
            muowm.Binding(repo);

            User = user;
        }

        //protected IdleBusCurdService(IdleBusUnitOfWorkManager muowm, IMapper mapper, IUser user, Expression<Func<TEntity, bool>> filter, Func<string, string> asTable = null) :
        //    base(muowm.Orm, mapper, filter, asTable)
        //{
        //    muowm.Binding(this);

        //    User = user;
        //}
    }
}
