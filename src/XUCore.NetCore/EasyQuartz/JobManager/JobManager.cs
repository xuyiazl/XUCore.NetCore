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

        public async Task AddJobAsync<TJob>(string cron, string id, IDictionary<string, object> map = null) where TJob : IJob
            => await AddJobAsync(typeof(TJob), cron, id, map);

        public async Task AddJobAsync(Type jobType, string cron, string id, IDictionary<string, object> map = null)
        {
            var group = $"{jobType.FullName}.Group";

            var name = id;

            var scheduler = await _schedulerFactory.GetScheduler();
            scheduler.JobFactory = _jobFactory;

            //var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));
            //if (jobKeys.Count > 0)
            //    if (jobKeys.Any(x => x.Name == name))
            //        return;

            var exist = await scheduler.CheckExists(new JobKey(name, group));

            if (exist) return;

            var job = JobBuilder.Create(jobType).WithIdentity(name, group)
                .WithDescription(jobType.Name)
                .UsingJobData("JobId", id)
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

        public async Task<bool> ExistJobAsync<TJob>(string id) where TJob : IJob
            => await ExistJobAsync(typeof(TJob), id);

        public async Task<bool> ExistJobAsync(Type jobType, string id)
        {
            var group = $"{jobType.FullName}.Group";

            var name = id;

            var scheduler = await _schedulerFactory.GetScheduler();
            var exist = await scheduler.CheckExists(new JobKey(name, group));

            return exist;

            //var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));
            //return jobKeys.Any(x => x.Name == name);
        }

        public async Task<bool> RemoveJobAsync<TJob>(string id) where TJob : IJob
            => await RemoveJobAsync(typeof(TJob), id);

        public async Task<bool> RemoveJobAsync(Type jobType, string id)
        {
            var group = $"{jobType.FullName}.Group";

            var name = id;

            var scheduler = await _schedulerFactory.GetScheduler();
            //var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));
            //await scheduler.DeleteJobs(jobKeys.Where(x => x.Name == name).ToList());

            return await scheduler.DeleteJob(new JobKey(name, group));
        }

        public async Task PauseJob<TJob>(string id) where TJob : IJob
            => await PauseJob(typeof(TJob), id);

        public async Task PauseJob(Type jobType, string id)
        {
            var group = $"{jobType.FullName}.Group";

            var name = id;

            var scheduler = await _schedulerFactory.GetScheduler();
            //var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));
            //await scheduler.PauseJob(jobKeys.First(x => x.Name == name));

            await scheduler.PauseJob(new JobKey(name, group));
        }

        public async Task OperateJob<TJob>(OperateEnum operate, string id) where TJob : IJob
            => await OperateJob(typeof(TJob), operate, id);

        public async Task OperateJob(Type jobType, OperateEnum operate, string id)
        {
            var group = $"{jobType.FullName}.Group";

            var name = id;

            var scheduler = await _schedulerFactory.GetScheduler();
            //var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));
            //var key = jobKeys.FirstOrDefault(x => x.Name == name);

            //if (key == null) return;

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
