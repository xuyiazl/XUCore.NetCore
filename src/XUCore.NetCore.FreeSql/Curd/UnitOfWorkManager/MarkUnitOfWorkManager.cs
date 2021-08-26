using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.FreeSql.Curd
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
