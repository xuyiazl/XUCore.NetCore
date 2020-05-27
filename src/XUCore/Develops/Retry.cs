using System;

namespace XUCore.Develops
{
    /// <summary>
    /// 异常重试类
    ///
    /// Retry.Task(RetryMode.Sync,
    ///      RetryAdapter.Create().MaxRuns(5).Waits(TimeSpan.FromMilliseconds(100)),
    ///      (ndx) =>
    ///      {
    ///            throw new Exception("reconnect number ");
    ///      },
    ///      (ndx, error) =>
    ///      {
    ///            Console.WriteLine(error.Message + ndx);
    ///            return false;
    ///      });
    /// </summary>
    public static class Retry
    {
        /// <summary>
        /// 任务
        /// </summary>
        /// <param name="mode">枚举 同步、异步</param>
        /// <param name="callback">任务委托</param>
        /// <param name="errorCallback">异常信息委托</param>
        public static void Run(RetryMode mode, Action<int> callback,
            Func<int, Exception, bool> errorCallback = null)
        {
            Run(mode, RetryAdapter.Create().MaxRuns(3), callback, errorCallback);
        }

        /// <summary>
        /// 任务
        /// </summary>
        /// <param name="mode">枚举 同步、异步</param>
        /// <param name="trigger">任务适配器 <see cref="RetryAdapter"/></param>
        /// <param name="callback">任务委托</param>
        /// <param name="errorCallback">异常信息委托</param>
        public static void Run(RetryMode mode, RetryAdapter trigger, Action<int> callback,
            Func<int, Exception, bool> errorCallback = null)
        {
            switch (mode)
            {
                case RetryMode.Async:
                    AsyncExecute(() => { Run(RetryMode.Sync, trigger, callback, errorCallback); });
                    break;

                default:
                    int current = 1;
                    while (current <= trigger.MaxRun)
                    {
                        if (ActionExecute(current, callback, errorCallback))
                            break;
                        else
                        {
                            if (current <= trigger.MaxRun)
                                System.Threading.Thread.Sleep(trigger.Wait);
                            current++;
                        }
                    }
                    break;
            }
        }

        private static bool ActionExecute(int number, Action<int> callback,
            Func<int, Exception, bool> errorCallback)
        {
            try
            {
                callback.Invoke(number);
                return true;
            }
            catch (Exception ex)
            {
                if (errorCallback != null)
                    return errorCallback.Invoke(number, ex);
                return false;
            }
        }

        private static void AsyncExecute(Action action)
        {
            action.BeginInvoke(mCallback =>
            {
                action.EndInvoke(mCallback);
            }, null);
        }
    }

    /// <summary>
    /// 任务适配器
    /// </summary>
    public class RetryAdapter : ICloneable
    {
        internal int MaxRun = 0;
        internal TimeSpan Wait = TimeSpan.FromMilliseconds(500);

        public static RetryAdapter Create()
        {
            return new RetryAdapter();
        }

        /// <summary>
        /// 最大执行次数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public RetryAdapter MaxRuns(int number)
        {
            if (number <= 0) number = 1;
            MaxRun = number;
            return this;
        }

        /// <summary>
        /// 重试休眠时间
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public RetryAdapter Waits(TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.MinValue || timeSpan == TimeSpan.MaxValue)
                timeSpan = TimeSpan.FromMilliseconds(500);
            Wait = timeSpan;
            return this;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public enum RetryMode
    {
        /// <summary>
        /// 同步
        /// </summary>
        Sync,

        /// <summary>
        /// 异步
        /// </summary>
        Async
    }
}