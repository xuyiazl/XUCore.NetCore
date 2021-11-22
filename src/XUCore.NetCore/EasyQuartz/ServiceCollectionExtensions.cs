using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XUCore.NetCore.EasyQuartz
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEasyQuartzService(this IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            #region Add JobSchedule

            var jobTypes = AppDomain.CurrentDomain.GetAssemblies()
                           .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IJob)) && t.IsClass && !t.IsAbstract))
                           .ToArray();

            foreach (var jobType in jobTypes)
            {
                services.AddTransient(jobType);
                var ignore = jobType.GetTypeInfo().IsDefined(typeof(JobIgnoreAttribute), false);
                var startNow = jobType.GetTypeInfo().IsDefined(typeof(StartNowAttribute), false);
                if (ignore)
                    continue;

                string cron;
                if (jobType.BaseType == typeof(EasyQuartzJob))
                {
                    var jobService = services.BuildServiceProvider().GetRequiredService(jobType);
                    cron = ((EasyQuartzJob)jobService).Cron;
                }
                else
                {
                    var triggerCron = jobType.GetCustomAttributes().OfType<TriggerCronAttribute>().FirstOrDefault();
                    if (triggerCron == null)
                        continue;

                    cron = triggerCron.Cron;
                }

                if (string.IsNullOrWhiteSpace(cron)) continue;

                var schedule = new JobSchedule(jobType, cron, $"{jobType.Name}Group", startNow);
                services.AddSingleton(schedule);
            }

            #endregion

            services.AddHostedService<QuartzHostedService>();
            services.AddTransient<IJobManager, JobManager>();
        }
    }
}
