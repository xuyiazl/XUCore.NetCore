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

        public async Task AddJobAsync(Type jobType, string cron, string id, IDictionary<string, object> map = null)
        {
            var group = $"{jobType.FullName}.Group";

            var name = id;

            var scheduler = await _schedulerFactory.GetScheduler();
            scheduler.JobFactory = _jobFactory;

            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));
            if (jobKeys.Count > 0)
                if (jobKeys.Any(x => x.Name == name))
                    return;

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

        public async Task<bool> ExistJobAsync(Type jobType, string id)
        {
            var group = $"{jobType.FullName}.Group";

            var name = id;

            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));
            return jobKeys.Any(x => x.Name == name);
        }

        public async Task RemoveJobAsync(Type jobType, string id)
        {
            var group = $"{jobType.FullName}.Group";

            var name = id;

            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));
            await scheduler.DeleteJobs(jobKeys.Where(x => x.Name == name).ToList());
        }

        public async Task PauseJob(Type jobType, string id)
        {
            var group = $"{jobType.FullName}.Group";

            var name = id;

            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));
            await scheduler.PauseJob(jobKeys.First(x => x.Name == name));
        }

        public async Task OperateJob(Type jobType, OperateEnum operate, string id)
        {
            var group = $"{jobType.FullName}.Group";

            var name = id;

            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));
            var key = jobKeys.FirstOrDefault(x => x.Name == name);

            if (key == null) return;
            switch (operate)
            {
                case OperateEnum.Delete:
                    await scheduler.DeleteJob(key);
                    break;
                case OperateEnum.Pause:
                    await scheduler.PauseJob(key);
                    break;
                case OperateEnum.Resume:
                    await scheduler.ResumeJob(key);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operate), operate, null);
            }
        }
    }
}
