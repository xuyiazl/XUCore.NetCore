using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUCore.Extensions;

namespace XUCore.NetCore.AspectCore.Cache
{
    /// <summary>
    /// 刷新任务
    /// </summary>
    internal class QuartzRefreshJob : IJob, IDisposable
    {
        private readonly ILogger<QuartzRefreshJob> logger;
        private readonly IServiceProvider serviceProvider;

        public QuartzRefreshJob(IServiceProvider serviceProvider, ILogger<QuartzRefreshJob> logger)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobKey = context.JobDetail.JobDataMap.GetString("JobKey");
            var jobIdentity = context.JobDetail.JobDataMap.GetString("JobIdentity");

            var taskService = serviceProvider.GetService<QuartzService>();

            if (!taskService.CacheContainer.ContainsKey(jobKey)) return;

            try
            {
                var job = taskService.CacheContainer[jobKey];

                job.Invoke();
            }
            catch (Exception ex)
            {
                logger.LogError($"{DateTime.Now}，执行缓存同步：{jobKey}，异常：{ex.FormatMessage()}");
            }

            Console.WriteLine($"{DateTime.Now}，执行缓存同步：{jobKey}");
        }

        public void Dispose()
        {

        }
    }
}
