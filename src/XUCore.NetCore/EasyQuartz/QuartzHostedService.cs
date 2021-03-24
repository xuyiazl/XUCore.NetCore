using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.EasyQuartz
{
    public class QuartzHostedService : IHostedService
    {
        private readonly ILogger<QuartzHostedService> _logger;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IEnumerable<JobSchedule> _jobSchedules;
        private readonly IJobFactory _jobFactory;

        public QuartzHostedService(
            ILogger<QuartzHostedService> logger,
            ISchedulerFactory schedulerFactory,
            IEnumerable<JobSchedule> jobSchedules,
            IJobFactory jobFactory)
        {
            _logger = logger;
            _schedulerFactory = schedulerFactory;
            _jobSchedules = jobSchedules;
            _jobFactory = jobFactory;
        }

        private IScheduler Scheduler { get; set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("QuartzHostedService is Running ...");
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;
            foreach (var jobSchedule in _jobSchedules)
            {
                if (jobSchedule.StartNow)
                {
                    var nowJob = CreateJob(jobSchedule, "_Now");
                    var nowTrigger = CreateNowTrigger();
                    await Scheduler.ScheduleJob(nowJob, nowTrigger, cancellationToken);
                }
                var job = CreateJob(jobSchedule);
                var trigger = CreateTrigger(jobSchedule);

                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }

            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // ReSharper disable once PossibleNullReferenceException
            await Scheduler?.Shutdown(cancellationToken);
        }

        private static IJobDetail CreateJob(JobSchedule schedule, string now = "")
        {
            var jobType = schedule.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity($"{jobType.FullName}{now}", schedule.Group)
                .WithDescription(jobType.Name)
                .Build();
        }

        private static ITrigger CreateTrigger(JobSchedule schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.JobType.FullName}.trigger", schedule.Group)
                .WithCronSchedule(schedule.CronExpression)
                .WithDescription(schedule.CronExpression)
                .Build();
        }

        private static ITrigger CreateNowTrigger()
        {
            return TriggerBuilder
                .Create()
                .StartNow()
                .Build();
        }
    }
}
