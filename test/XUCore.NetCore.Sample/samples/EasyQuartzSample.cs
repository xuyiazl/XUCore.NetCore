using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUCore.NetCore.EasyQuartz;

namespace XUCore.NetCore.Sample.samples
{
    public class EasyQuartzSample : ISample
    {
        public void Run()
        {
            new HostBuilder()
              .ConfigureServices(x => x.AddEasyQuartzService())
              .Build()
              .Run();
        }
    }

    [TriggerCron("0/5 * * * * ? *")]
    public class Test1Job : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now} Test1Job");
            return Task.CompletedTask;
        }
    }

    public class Test2Job : EasyQuartzJob, IJob
    {
        private readonly IConfiguration _configuration;

        public Test2Job(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override string Cron => _configuration["Test2JobCron"];

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now}我是  Test2Job");
            return Task.CompletedTask;
        }
    }

    [StartNow]
    [TriggerCron("0 0/5 * * * ? ")]
    public class Test3Job : IJob
    {
        private readonly IJobManager _jobManager;

        public Test3Job(IJobManager jobManager)
        {
            _jobManager = jobManager;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now}我是  Test3Job");
            await _jobManager.AddJobAsync(typeof(Test4Job), CronCommon.SecondInterval(2), "111111");
        }
    }

    [JobIgnore]
    public class Test4Job : IJob
    {
        private readonly IJobManager _jobManager;

        public Test4Job(IJobManager jobManager)
        {
            _jobManager = jobManager;
        }

        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.MergedJobDataMap;
            string id = dataMap.GetString("Id");

            Console.WriteLine($"{DateTime.Now}Test4Job,参数{id}");
            //await _jobManager.RemoveJobAsync(typeof(Test4Job), id);
            return Task.CompletedTask;
        }
    }
}
