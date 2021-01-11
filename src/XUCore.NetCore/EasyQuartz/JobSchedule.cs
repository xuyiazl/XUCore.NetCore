using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.EasyQuartz
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType, string cronExpression, string group = "defaultGroup", bool startNow = false)
        {
            JobType = jobType;
            CronExpression = cronExpression;
            Group = group;
            StartNow = startNow;
        }

        public Type JobType { get; }

        public bool StartNow { get; }

        public string CronExpression { get; }

        public string Group { get; }
    }
}
