using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Template.FreeSql.Persistence
{
    /// <summary>
    /// 空闲管理工作单元，用于动态创建多租户的FreeSql实例
    /// </summary>
    public class IdleBusUnitOfWorkManager : UnitOfWorkManager
    {
        public IdleBusUnitOfWorkManager(IdleBus<IFreeSql> ib, IServiceProvider serviceProvider) : base(ib.GetFreeSql(serviceProvider))
        {
        }
    }
}
