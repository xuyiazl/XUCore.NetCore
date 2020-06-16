using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using XUCore.Extensions;
using XUCore.Helpers;

namespace XUCore.Develops
{
    /// <summary>
    /// 数据流量控制
    /// </summary>
    public class FlowMonitoring : TimerBase, IFlowMonitoring
    {
        private static object syncLock = new object();
        private decimal _setSize = 128 * 8;
        private decimal _netBytes = 0;
        private decimal _Kbyte = 1024;
        private Action<decimal> _actionMonitoring = null;
        private int _interval = 1000;

        public FlowMonitoring(int interval = 1000)
            : base(interval)
        {
            this._interval = interval;
            this.timer.AutoReset = true;
            this.timer.Start();
        }
        /// <summary>
        /// 触发周期
        /// <remarks>默认1000毫秒一次</remarks>
        /// </summary>
        public int Interval
        {
            get { return _interval; }
            set
            {
                lock (syncLock)
                {
                    _interval = value;
                    this.timer.Interval = value;
                }
            }
        }

        /// <summary>
        /// 每秒写入大小
        /// <remarks>单位KB</remarks>
        /// </summary>
        public decimal Size
        {
            get { return _setSize / 8; }
            set
            {
                lock (syncLock)
                    _setSize = value * 8;
            }
        }

        public override void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_actionMonitoring != null)
                _actionMonitoring.Invoke(this._netBytes / 8);
        }

        /// <summary>
        /// 监控每秒数据流量
        /// </summary>
        /// <param name="action"></param>
        public void Monitoring(Action<decimal> action)
        {
            _actionMonitoring = action;
        }

        /// <summary>
        /// 数据流量控制
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="callback"></param>
        public void Control(DataTable dataTable, Action<DataRow[]> callback)
        {
            Monitor.Enter(syncLock);
            try
            {
                if (dataTable == null || callback == null)
                    return;

                decimal _mSize = _setSize;

                //复制表结构
                DataTable newDataTable = dataTable.Clone();

                List<DataRow> rows = new List<DataRow>();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Array.ForEach<DataRow>(dataTable.Select(), (dr) =>
                {
                    //清除计算表
                    newDataTable.Clear();
                    //复制新的DataRow行进行计算
                    newDataTable.ImportRow(dr);

                    rows.Add(dr);
                    //获取单条记录byte
                    byte[] newDataTableByte = Serialize.ToBinary(newDataTable);
                    decimal drKB = newDataTableByte.Length / _Kbyte;
                    //单位限制的剩余数据流量
                    _mSize -= drKB;
                    //总数据流量
                    _netBytes += drKB;

                    //如果数据流量超过了限制数
                    if (_mSize < 0)
                    {
                        //时间再一秒内 则休眠
                        if (stopwatch.Elapsed.Milliseconds < _interval)
                            System.Threading.Thread.Sleep(_interval - stopwatch.Elapsed.Milliseconds);

                        callback.Invoke(rows.ToArray());

                        rows.Clear();
                        //重启计时器  
                        stopwatch.Reset();
                        stopwatch.Start();
                        //初始化限制
                        _mSize = _setSize;
                        _netBytes = 0;
                    }
                });
                //时间再一秒内 则休眠
                if (stopwatch.Elapsed.Milliseconds < _interval)
                    System.Threading.Thread.Sleep(_interval - stopwatch.Elapsed.Milliseconds);
                callback.Invoke(rows.ToArray());
                _netBytes = 0;
            }
            finally
            {
                Monitor.Exit(syncLock);
            }
        }

        /// <summary>
        /// 数据流量控制
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="callback"></param>
        public void Control<TEntity>(IList<TEntity> entities, Action<TEntity[]> callback)
            where TEntity : class, new()
        {
            if (entities == null || entities.Count == 0 || callback == null)
                return;

            Control<TEntity>(entities.ToArray(), callback);
        }

        /// <summary>
        /// 数据流量控制
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="callback"></param>
        public void Control<TEntity>(TEntity[] entities, Action<TEntity[]> callback)
            where TEntity : class, new()
        {
            Monitor.Enter(syncLock);
            try
            {
                if (entities == null || entities.Length == 0 || callback == null)
                    return;

                decimal _mSize = _setSize;

                //复制表结构
                List<TEntity> newEntities = new List<TEntity>();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Array.ForEach<TEntity>(entities.ToArray(), (entity) =>
                {
                    newEntities.Add(entity);

                    //获取单条记录byte
                    byte[] newEntityByte = Serialize.ToBinary(entity);
                    decimal kb = newEntityByte.Length / _Kbyte;
                    //单位限制的剩余数据流量
                    _mSize -= kb;
                    //总数据流量
                    _netBytes += kb;

                    //如果数据流量超过了限制数
                    if (_mSize < 0)
                    {
                        //时间再一秒内 则休眠
                        if (stopwatch.Elapsed.Milliseconds < _interval)
                            System.Threading.Thread.Sleep(_interval - stopwatch.Elapsed.Milliseconds);

                        callback.Invoke(newEntities.ToArray());

                        newEntities.Clear();
                        //重启计时器  
                        stopwatch.Reset();
                        stopwatch.Start();
                        //初始化限制
                        _mSize = _setSize;
                        _netBytes = 0;
                    }
                });
                //时间再一秒内 则休眠
                if (stopwatch.Elapsed.Milliseconds < _interval)
                    System.Threading.Thread.Sleep(_interval - stopwatch.Elapsed.Milliseconds);
                callback.Invoke(newEntities.ToArray());
                _netBytes = 0;
            }
            finally
            {
                Monitor.Exit(syncLock);
            }
        }
    }
}
