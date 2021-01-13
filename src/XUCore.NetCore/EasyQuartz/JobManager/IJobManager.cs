using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.EasyQuartz
{
    public interface IJobManager
    {
        Task AddJobAsync<TJob>(string cron, string id, IDictionary<string, object> map = null) where TJob : IJob;

        Task AddJobAsync(Type jobType, string cron, string id, IDictionary<string, object> map = null);

        Task<List<JobKey>> GetJobsAsync<TJob>() where TJob : IJob;

        Task<List<JobKey>> GetJobsAsync(Type jobType);

        Task<bool> ExistJobAsync<TJob>(string id) where TJob : IJob;

        Task<bool> ExistJobAsync(Type jobType, string id);

        Task<bool> RemoveJobAsync<TJob>(string id) where TJob : IJob;

        Task<bool> RemoveJobAsync(Type jobType, string id);

        Task PauseJob<TJob>(string id) where TJob : IJob;

        Task PauseJob(Type jobType, string id);

        Task OperateJob<TJob>(OperateEnum operate, string id) where TJob : IJob;

        Task OperateJob(Type jobType, OperateEnum operate, string id);
    }
}
