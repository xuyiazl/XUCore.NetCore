using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Template.FreeSql.Persistence
{
    /// <summary>
    /// 多库FreeSql实例
    /// </summary>
    /// <typeparam name="TMark"></typeparam>
    public class MarkUnitOfWorkManager<TMark> : UnitOfWorkManager
    {
        public MarkUnitOfWorkManager(AspectCoreFreeSql<TMark> aspectCoreFreeSql) : base(aspectCoreFreeSql.Orm)
        {
        }
    }
}
