using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.EasyQuartz
{
    public interface IJobManager
    {

        Task AddJobAsync(Type jobType, string cron, string id = "", IDictionary<string, object> map = null);
        Task<bool> ExistJobAsync(Type jobType, string id);
        Task RemoveJobAsync(Type jobType, string id = "");
        Task PauseJob(Type jobType, string id = "");
        Task OperateJob(Type jobType, OperateEnum operate, string id = "");
    }
}
