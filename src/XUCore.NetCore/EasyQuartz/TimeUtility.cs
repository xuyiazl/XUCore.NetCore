using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.EasyQuartz
{
    public class TimeUtility
    {
        /// <summary>
        /// 时间格式转换成Quartz任务调度器Cron表达式
        /// </summary>
        /// <returns></returns>
        public static string TimeToQuartzCron(DateTime time)
        {
            var cronValue = $"{time.Second} {time.Minute} {time.Hour} {time.Day} {time.Month} ?";
            try
            {
                return cronValue;
            }
            catch (Exception ea)
            {
                Console.WriteLine(ea.Message);
                throw;
            }
        }
    }
}
