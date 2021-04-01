using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.Extensions;

namespace XUCore.NetCore.AspectCore.Cache
{
    /// <summary>
    /// JobFactory ：实现在Timer触发的时候注入生成对应的Job组件
    /// </summary>
    internal class QuartzJobFactory : IJobFactory
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceProvider serviceProvider;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public QuartzJobFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Called by the scheduler at the time of the trigger firing, in order to produce a Quartz.IJob instance on which to call Execute.
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return serviceProvider.GetService(bundle.JobDetail.JobType).As<IJob>();
        }

        /// <summary>
        /// Allows the job factory to destroy/cleanup the job if needed.
        /// </summary>
        /// <param name="job"></param>
        public void ReturnJob(IJob job)
        {
        }
    }
}
