using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.EasyQuartz
{
    public abstract class EasyQuartzJob
    {
        public abstract string Cron { get; }
    }
}
