using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.EasyQuartz
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TriggerCronAttribute : Attribute
    {
        public TriggerCronAttribute(string cron)
        {
            //Cron = string.IsNullOrWhiteSpace(cron) ? "0/1 * * * * ? *" : cron;
            Cron = cron;
        }

        public string Cron { get; }
    }
}
