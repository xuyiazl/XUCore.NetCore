using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.EasyQuartz
{

    public interface IJobManager
    {
        /// <summary>
        /// Quartz Scheduler.
        /// </summary>
        IScheduler Scheduler { get; }
        /// <summary>
        /// 增加任务
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <param name="cron">表达式</param>
        /// <param name="name">任务名（唯一id）</param>
        /// <param name="map">参数</param>
        /// <returns></returns>
        Task AddAsync<TJob>(string cron, string name, IDictionary<string, object> map = null) where TJob : IJob;
        /// <summary>
        /// 增加任务
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <param name="cron">表达式</param>
        /// <param name="name">任务名（唯一id）</param>
        /// <param name="map">参数</param>
        /// <returns></returns>
        Task AddAsync(Type jobType, string cron, string name, IDictionary<string, object> map = null);
        /// <summary>
        /// 获取所有的key
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <returns></returns>
        Task<List<JobKey>> GetJobKeysAsync<TJob>() where TJob : IJob;
        /// <summary>
        /// 获取所有的key
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <returns></returns>
        Task<List<JobKey>> GetJobKeysAsync(Type jobType);
        /// <summary>
        /// 检测当是否存在
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<bool> ExistAsync<TJob>(string name) where TJob : IJob;
        /// <summary>
        /// 检测当是否存在
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <param name="name">任务名（唯一id）</param>
        /// <returns></returns>
        Task<bool> ExistAsync(Type jobType, string name);
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <returns></returns>
        Task<bool> RemoveAllAsync<TJob>() where TJob : IJob;
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <returns></returns>
        Task<bool> RemoveAllAsync(Type jobType);
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <param name="name">任务名（唯一id）</param>
        /// <returns></returns>
        Task<bool> RemoveAsync<TJob>(string name) where TJob : IJob;
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <param name="name">任务名（唯一id）</param>
        /// <returns></returns>
        Task<bool> RemoveAsync(Type jobType, string name);
        /// <summary>
        /// 暂停
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        Task Pause<TJob>(string name) where TJob : IJob;
        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <param name="name">任务名（唯一id）</param>
        /// <returns></returns>
        Task Pause(Type jobType, string name);
        /// <summary>
        /// 暂停
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <returns></returns>
        Task Pause<TJob>() where TJob : IJob;
        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <returns></returns>
        Task Pause(Type jobType);
        /// <summary>
        /// （中断后）重新开始
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <param name="name">任务名（唯一id）</param>
        /// <returns></returns>
        Task Resume<TJob>(string name) where TJob : IJob;
        /// <summary>
        /// （中断后）重新开始
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <param name="name">任务名（唯一id）</param>
        /// <returns></returns>
        Task Resume(Type jobType, string name);
        /// <summary>
        /// （中断后）重新开始
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <returns></returns>
        Task Resume<TJob>() where TJob : IJob;
        /// <summary>
        /// （中断后）重新开始
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <returns></returns>
        Task Resume(Type jobType);
        /// <summary>
        /// 清除所有任务
        /// </summary>
        /// <returns></returns>
        Task Clear();
        /// <summary>
        /// 操作任务
        /// </summary>
        /// <typeparam name="TJob">实现<see cref="IJob"/>任务</typeparam>
        /// <param name="operate">操作类型</param>
        /// <param name="name">任务名（唯一id）</param>
        /// <returns></returns>
        Task Operate<TJob>(OperateEnum operate, string name) where TJob : IJob;
        /// <summary>
        /// 操作任务
        /// </summary>
        /// <param name="jobType">IJob任务类型</param>
        /// <param name="operate">操作类型</param>
        /// <param name="name">任务名（唯一id）</param>
        /// <returns></returns>
        Task Operate(Type jobType, OperateEnum operate, string name);
    }
}
