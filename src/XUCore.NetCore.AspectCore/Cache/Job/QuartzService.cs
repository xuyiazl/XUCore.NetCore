using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace XUCore.NetCore.AspectCore.Cache
{
    /// <summary>
    /// Quartz.Net启动后注册job和trigger
    /// </summary>
    internal class QuartzService
    {
        private IScheduler _scheduler;
        private readonly ILogger _logger;
        private readonly IJobFactory jobfactory;
        public readonly ConcurrentDictionary<string, Func<Task>> CacheContainer;
        /// <summary>
        /// Quartz.Net启动后注册job和trigger
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="loggerFactory"></param>
        public QuartzService(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            this.CacheContainer = new ConcurrentDictionary<string, Func<Task>>();

            _logger = loggerFactory.CreateLogger<QuartzService>();

            jobfactory = new QuartzJobFactory(serviceProvider);

            var schedulerFactory = new StdSchedulerFactory();

            _scheduler = schedulerFactory.GetScheduler().Result;

            _scheduler.JobFactory = jobfactory;
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start()
        {
            _scheduler.Start().Wait();
        }

        public async System.Threading.Tasks.Task JoinJobAsync(string key, TimeSpan validity)
        {
            var exist = _scheduler.CheckExists(new JobKey(key)).ConfigureAwait(false).GetAwaiter().GetResult();

            if (!exist)
            {
                var clickJob = JobBuilder.Create<QuartzRefreshJob>().WithIdentity(key).Build();

                var jobIdentity = $"{key}-Cron";

                var clickJobTrigger = TriggerBuilder.Create()
                    .WithIdentity(jobIdentity)
                    .WithDailyTimeIntervalSchedule(a =>
                    {
                        a.OnEveryDay().WithIntervalInSeconds((int)validity.TotalSeconds);
                    })
                    .Build();

                clickJob.JobDataMap.Add("JobKey", key);
                clickJob.JobDataMap.Add("JobIdentity", jobIdentity);

                await _scheduler.ScheduleJob(clickJob, clickJobTrigger);

                //await _scheduler.TriggerJob(new JobKey(key));
            }

        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            if (_scheduler == null)
                return;
            if (_scheduler.Shutdown(waitForJobsToComplete: true).Wait(30000))
                _scheduler = null;
        }
    }


}
