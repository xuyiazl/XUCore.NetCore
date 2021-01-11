using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.EasyQuartz
{
    public interface IJobManager
    {

        Task AddJobAsync(Type jobType, string cron, string id = "");
        Task RemoveJobAsync(Type jobType, string id = "");
        Task PauseJob(Type jobType, string id = "");
        Task OperateJob(Type jobType, OperateEnum operate, string id = "");
    }
}
