using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Template.FreeSql.Persistence
{
    /// <summary>
    /// 单库FreeSql实例
    /// </summary>
    public class FreeSqlUnitOfWorkManager : UnitOfWorkManager
    {
        public FreeSqlUnitOfWorkManager(AspectCoreFreeSql aspectCoreFreeSql) : base(aspectCoreFreeSql.Orm)
        {
        }
    }
}
