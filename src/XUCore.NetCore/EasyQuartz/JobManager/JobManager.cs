using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.EasyQuartz
{
    public class JobManager : IJobManager
    {
        private readonly IJobFactory _jobFactory;
        private readonly ISchedulerFactory _schedulerFactory;

        public JobManager(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
        }

        public async Task AddJobAsync<TJob>(string cron, string name, IDictionary<string, object> map = null) where TJob : IJob
            => await AddJobAsync(typeof(TJob), cron, name, map);

        public async Task AddJobAsync(Type jobType, string cron, string name, IDictionary<string, object> map = null)
        {
            var group = $"{jobType.FullName}.Group";

            var scheduler = await _schedulerFactory.GetScheduler();
            scheduler.JobFactory = _jobFactory;

            var exist = await scheduler.CheckExists(new JobKey(name, group));

            if (exist) return;

            var job = JobBuilder.Create(jobType).WithIdentity(name, group)
                .WithDescription(jobType.Name)
                .UsingJobData("JobId", name)
                .Build();

            var trigger = TriggerBuilder
                .Create()
                .WithIdentity($"{name}.trigger", group)
                .WithCronSchedule(cron)
                .WithDescription(jobType.Name)
                .Build();

            if (map != null)
                foreach (var item in map)
                    job.JobDataMap.Add(item);

            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }

        public async Task<List<JobKey>> GetJobsAsync<TJob>() where TJob : IJob
            => await GetJobsAsync(typeof(TJob));

        public async Task<List<JobKey>> GetJobsAsync(Type jobType)
        {
            var group = $"{jobType.FullName}.Group";

            var scheduler = await _schedulerFactory.GetScheduler();

            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));

            return jobKeys.ToList();
        }

        public async Task<bool> ExistJobAsync<TJob>(string name) where TJob : IJob
            => await ExistJobAsync(typeof(TJob), name);

        public async Task<bool> ExistJobAsync(Type jobType, string name)
        {
            var group = $"{jobType.FullName}.Group";

            var scheduler = await _schedulerFactory.GetScheduler();
            var exist = await scheduler.CheckExists(new JobKey(name, group));

            return exist;
        }

        public async Task<bool> RemoveJobAsync<TJob>(string name) where TJob : IJob
            => await RemoveJobAsync(typeof(TJob), name);

        public async Task<bool> RemoveJobAsync(Type jobType, string name)
        {
            var group = $"{jobType.FullName}.Group";

            var scheduler = await _schedulerFactory.GetScheduler();

            return await scheduler.DeleteJob(new JobKey(name, group));
        }

        public async Task PauseJob<TJob>(string name) where TJob : IJob
            => await PauseJob(typeof(TJob), name);

        public async Task PauseJob(Type jobType, string name)
        {
            var group = $"{jobType.FullName}.Group";

            var scheduler = await _schedulerFactory.GetScheduler();

            await scheduler.PauseJob(new JobKey(name, group));
        }

        public async Task OperateJob<TJob>(OperateEnum operate, string name) where TJob : IJob
            => await OperateJob(typeof(TJob), operate, name);

        public async Task OperateJob(Type jobType, OperateEnum operate, string name)
        {
            var group = $"{jobType.FullName}.Group";

            var scheduler = await _schedulerFactory.GetScheduler();

            switch (operate)
            {
                case OperateEnum.Delete:
                    await scheduler.DeleteJob(new JobKey(name, group));
                    break;
                case OperateEnum.Pause:
                    await scheduler.PauseJob(new JobKey(name, group));
                    break;
                case OperateEnum.Resume:
                    await scheduler.ResumeJob(new JobKey(name, group));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operate), operate, null);
            }
        }
    }
}
