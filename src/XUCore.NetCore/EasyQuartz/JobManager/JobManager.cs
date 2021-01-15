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
    /// <summary>
    /// Job管理
    /// </summary>
    public class JobManager : IJobManager
    {
        private readonly IJobFactory _jobFactory;
        private readonly ISchedulerFactory _schedulerFactory;
        /// <summary>
        /// Job管理
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="jobFactory"></param>
        public JobManager(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
        }
        /// <summary>
        /// Quartz Scheduler.
        /// </summary>
        public IScheduler Scheduler
        {
            get
            {
                return _schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            }
        }
        /// <summary>
        /// 增加任务
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <param name="cron">表达式</param>
        /// <param name="name">任务名（唯一id）</param>
        /// <param name="map">参数</param>
        /// <returns></returns>
        public async Task AddAsync<TJob>(string cron, string name, IDictionary<string, object> map = null) where TJob : IJob
            => await AddAsync(typeof(TJob), cron, name, map);
        /// <summary>
        /// 增加任务
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <param name="cron">表达式</param>
        /// <param name="name">任务名（唯一id）</param>
        /// <param name="map">参数</param>
        /// <returns></returns>
        public async Task AddAsync(Type jobType, string cron, string name, IDictionary<string, object> map = null)
        {
            var group = $"{jobType.FullName}.Group";

            var scheduler = Scheduler;
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
        /// <summary>
        /// 获取所有的key
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <returns></returns>
        public async Task<List<JobKey>> GetJobKeysAsync<TJob>() where TJob : IJob
            => await GetJobKeysAsync(typeof(TJob));
        /// <summary>
        /// 获取所有的key
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <returns></returns>
        public async Task<List<JobKey>> GetJobKeysAsync(Type jobType)
        {
            var group = $"{jobType.FullName}.Group";

            var jobKeys = await Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));

            return jobKeys.ToList();
        }
        /// <summary>
        /// 检测当是否存在
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> ExistAsync<TJob>(string name) where TJob : IJob
            => await ExistAsync(typeof(TJob), name);
        /// <summary>
        /// 检测当是否存在
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <param name="name">任务名（唯一id）</param>
        /// <returns></returns>
        public async Task<bool> ExistAsync(Type jobType, string name)
        {
            var group = $"{jobType.FullName}.Group";

            var exist = await Scheduler.CheckExists(new JobKey(name, group));

            return exist;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <returns></returns>
        public async Task<bool> RemoveAllAsync<TJob>() where TJob : IJob
            => await RemoveAllAsync(typeof(TJob));
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <returns></returns>
        public async Task<bool> RemoveAllAsync(Type jobType)
        {
            var group = $"{jobType.FullName}.Group";

            var jobKeys = await Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));

            return await Scheduler.DeleteJobs(jobKeys);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <param name="name">任务名（唯一id）</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync<TJob>(string name) where TJob : IJob
            => await RemoveAsync(typeof(TJob), name);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <param name="name">任务名（唯一id）</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(Type jobType, string name)
        {
            var group = $"{jobType.FullName}.Group";

            return await Scheduler.DeleteJob(new JobKey(name, group));
        }
        /// <summary>
        /// 暂停
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task Pause<TJob>(string name) where TJob : IJob
            => await Pause(typeof(TJob), name);
        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <param name="name">任务名（唯一id）</param>
        /// <returns></returns>
        public async Task Pause(Type jobType, string name)
        {
            var group = $"{jobType.FullName}.Group";

            await Scheduler.PauseJob(new JobKey(name, group));
        }
        /// <summary>
        /// 暂停
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <returns></returns>
        public async Task Pause<TJob>() where TJob : IJob
            => await Pause(typeof(TJob));
        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <returns></returns>
        public async Task Pause(Type jobType)
        {
            var group = $"{jobType.FullName}.Group";

            await Scheduler.PauseJobs(GroupMatcher<JobKey>.GroupEquals(group));
        }
        /// <summary>
        /// （中断后）重新开始
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <param name="name">任务名（唯一id）</param>
        /// <returns></returns>
        public async Task Resume<TJob>(string name) where TJob : IJob
            => await Resume(typeof(TJob), name);
        /// <summary>
        /// （中断后）重新开始
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <param name="name">任务名（唯一id）</param>
        /// <returns></returns>
        public async Task Resume(Type jobType, string name)
        {
            var group = $"{jobType.FullName}.Group";

            await Scheduler.ResumeJob(new JobKey(name, group));
        }
        /// <summary>
        /// （中断后）重新开始
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <returns></returns>
        public async Task Resume<TJob>() where TJob : IJob
            => await Resume(typeof(TJob));
        /// <summary>
        /// （中断后）重新开始
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <returns></returns>
        public async Task Resume(Type jobType)
        {
            var group = $"{jobType.FullName}.Group";

            await Scheduler.ResumeJobs(GroupMatcher<JobKey>.GroupEquals(group));
        }
        /// <summary>
        /// 清除所有任务
        /// </summary>
        /// <returns></returns>
        public async Task Clear()
        {
            await Scheduler.Clear();
        }
        /// <summary>
        /// 操作任务
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <param name="operate">操作类型</param>
        /// <param name="name">任务名（唯一id）</param>
        /// <returns></returns>
        public async Task Operate<TJob>(OperateEnum operate, string name) where TJob : IJob
            => await Operate(typeof(TJob), operate, name);
        /// <summary>
        /// 操作任务
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <param name="operate">操作类型</param>
        /// <param name="name">任务名（唯一id）</param>
        /// <returns></returns>
        public async Task Operate(Type jobType, OperateEnum operate, string name)
        {
            var group = $"{jobType.FullName}.Group";

            switch (operate)
            {
                case OperateEnum.Delete:
                    await Scheduler.DeleteJob(new JobKey(name, group));
                    break;
                case OperateEnum.Pause:
                    await Scheduler.PauseJob(new JobKey(name, group));
                    break;
                case OperateEnum.Resume:
                    await Scheduler.ResumeJob(new JobKey(name, group));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operate), operate, null);
            }
        }
    }
}
