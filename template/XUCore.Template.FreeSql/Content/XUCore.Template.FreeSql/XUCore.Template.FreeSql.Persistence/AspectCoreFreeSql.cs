using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Template.FreeSql.Persistence
{
    public class AspectCoreFreeSql
    {
        public IFreeSql Orm { get; set; }
    }
    public class AspectCoreFreeSql<TMark>
    {
        public IFreeSql<TMark> Orm { get; set; }
    }
}
