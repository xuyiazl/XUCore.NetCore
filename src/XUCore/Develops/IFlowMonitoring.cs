using System;
using System.Collections.Generic;
using System.Data;
using System.Timers;

namespace XUCore.Develops
{
    /// <summary>
    /// 数据流量控制
    /// </summary>
    public interface IFlowMonitoring
    {
        /// <summary>
        /// 触发周期
        /// <remarks>默认1000毫秒一次</remarks>
        /// </summary>
        int Interval { get; set; }
        /// <summary>
        /// 每秒写入大小
        /// <remarks>单位KB</remarks>
        /// </summary>
        decimal Size { get; set; }
        /// <summary>
        /// 数据流量控制
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="callback"></param>
        void Control(DataTable dataTable, Action<DataRow[]> callback);
        /// <summary>
        /// 数据流量控制
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="callback"></param>
        void Control<TEntity>(IList<TEntity> entities, Action<TEntity[]> callback) where TEntity : class, new();
        /// <summary>
        /// 数据流量控制
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="callback"></param>
        void Control<TEntity>(TEntity[] entities, Action<TEntity[]> callback) where TEntity : class, new();
        /// <summary>
        /// 监控每秒数据流量
        /// </summary>
        /// <param name="action"></param>
        void Monitoring(Action<decimal> action);
    }
}