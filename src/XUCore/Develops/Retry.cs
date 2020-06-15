using System;
using System.Threading.Tasks;

namespace XUCore.Develops
{
    /// <summary>
    /// 异常重试类
    /// <code>
    /// Retry.Run(
    ///     RetryAdapter.Create().Runs(5).Wait(500),
    ///     model,
    ///      (model,ndx) =>
    ///      {
    ///            throw new Exception("reconnect number ");
    ///      },
    ///      (ndx, error) =>
    ///      {
    ///            Console.WriteLine(error.Message + ndx);
    ///      });
    /// </code>
    /// </summary>
    public static class Retry
    {
        /// <summary>
        /// 重试运行
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model">传入的数据</param>
        /// <param name="retryAdapter">任务适配器</param>
        /// <param name="execHandler">执行任务</param>
        /// <param name="errorHandler">异常处理</param>
        public static void Run<TModel>(TModel model,
            RetryAdapter retryAdapter,
            Action<TModel, int> execHandler, Action<int, Exception> errorHandler = null)
        {
            int current = 1;
            while (current <= retryAdapter.MaxRun)
            {
                try
                {
                    execHandler.Invoke(model, current);
                }
                catch (Exception ex)
                {
                    if (errorHandler != null)
                        errorHandler.Invoke(current, ex);
                }
                if (current <= retryAdapter.MaxRun)
                    System.Threading.Thread.Sleep(retryAdapter.WaitTime);
                current++;
            }
        }
        /// <summary>
        /// 重试运行
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="model">传入的数据</param>
        /// <param name="retryAdapter">任务适配器</param>
        /// <param name="execHandler">执行任务</param>
        /// <param name="errorHandler">异常处理</param>
        /// <returns></returns>
        public static TResult Run<TModel, TResult>(TModel model,
            RetryAdapter retryAdapter,
             Func<TModel, int, TResult> execHandler, Action<int, Exception> errorHandler = null)
        {
            int current = 1;
            while (current <= retryAdapter.MaxRun)
            {
                try
                {
                    return execHandler.Invoke(model, current);
                }
                catch (Exception ex)
                {
                    if (errorHandler != null)
                        errorHandler.Invoke(current, ex);
                }
                if (current <= retryAdapter.MaxRun)
                    System.Threading.Thread.Sleep(retryAdapter.WaitTime);
                current++;
            }

            return default;
        }
        /// <summary>
        /// 重试运行
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model">传入的数据</param>
        /// <param name="retryAdapter">任务适配器</param>
        /// <param name="execHandler">执行任务</param>
        /// <param name="errorHandler">异常处理</param>
        /// <returns></returns>
        public static async Task RunAsync<TModel>(TModel model,
            RetryAdapter retryAdapter,
             Func<TModel, int, Task> execHandler, Action<int, Exception> errorHandler = null)
        {
            int current = 1;
            while (current <= retryAdapter.MaxRun)
            {
                try
                {
                    await execHandler.Invoke(model, current);
                }
                catch (Exception ex)
                {
                    if (errorHandler != null)
                        errorHandler.Invoke(current, ex);
                }
                if (current <= retryAdapter.MaxRun)
                    System.Threading.Thread.Sleep(retryAdapter.WaitTime);
                current++;
            }
        }
        /// <summary>
        /// 重试运行
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="model">传入的数据</param>
        /// <param name="retryAdapter">任务适配器</param>
        /// <param name="execHandler">执行任务</param>
        /// <param name="errorHandler">异常处理</param>
        /// <returns></returns>
        public static async Task<TResult> RunAsync<TModel, TResult>(TModel model,
            RetryAdapter retryAdapter,
            Func<TModel, int, Task<TResult>> execHandler, Action<int, Exception> errorHandler = null)
        {
            int current = 1;
            while (current <= retryAdapter.MaxRun)
            {
                try
                {
                    return await execHandler.Invoke(model, current);
                }
                catch (Exception ex)
                {
                    if (errorHandler != null)
                        errorHandler.Invoke(current, ex);
                }
                if (current <= retryAdapter.MaxRun)
                    System.Threading.Thread.Sleep(retryAdapter.WaitTime);
                current++;
            }

            return default;
        }
    }

    /// <summary>
    /// 任务适配器
    /// </summary>
    public class RetryAdapter : ICloneable
    {
        internal int MaxRun = 3;
        internal TimeSpan WaitTime = TimeSpan.FromMilliseconds(500);

        public static RetryAdapter Create()
        {
            return new RetryAdapter();
        }
        /// <summary>
        /// 最大执行次数（执行成功后截止运行）
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public RetryAdapter Runs(int number)
        {
            if (number <= 0) number = 1;
            MaxRun = number;
            return this;
        }
        /// <summary>
        /// 重试休眠时间
        /// </summary>
        /// <param name="milliseconds">毫秒</param>
        /// <returns></returns>
        public RetryAdapter Wait(int milliseconds) => Wait(TimeSpan.FromMilliseconds(milliseconds));
        /// <summary>
        /// 重试休眠时间
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public RetryAdapter Wait(TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.MinValue || timeSpan == TimeSpan.MaxValue)
                timeSpan = TimeSpan.FromMilliseconds(500);
            WaitTime = timeSpan;
            return this;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

}