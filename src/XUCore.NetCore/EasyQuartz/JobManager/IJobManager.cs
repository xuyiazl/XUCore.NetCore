using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.EasyQuartz
{
    public interface IJobManager
    {
        Task AddJobAsync<TJob>(string cron, string name, IDictionary<string, object> map = null) where TJob : IJob;

        Task AddJobAsync(Type jobType, string cron, string name, IDictionary<string, object> map = null);

        Task<List<JobKey>> GetJobsAsync<TJob>() where TJob : IJob;

        Task<List<JobKey>> GetJobsAsync(Type jobType);

        Task<bool> ExistJobAsync<TJob>(string name) where TJob : IJob;

        Task<bool> ExistJobAsync(Type jobType, string name);

        Task<bool> RemoveJobAsync<TJob>(string name) where TJob : IJob;

        Task<bool> RemoveJobAsync(Type jobType, string name);

        Task PauseJob<TJob>(string name) where TJob : IJob;

        Task PauseJob(Type jobType, string name);

        Task OperateJob<TJob>(OperateEnum operate, string name) where TJob : IJob;

        Task OperateJob(Type jobType, OperateEnum operate, string name);
    }
}
