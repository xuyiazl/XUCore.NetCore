﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// SaveChange事务
        /// </summary>
        /// <returns></returns>
        int Commit();
        /// <summary>
        /// SaveChange事务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// <para>获取或设置一个值，该值指示是否创建事务由<see cref="DbContext"/>自动。</para>
        /// <para><see cref="DbContext.SaveChanges()"/>如果没有调用'BeginTransaction'或'UseTransaction'方法的。将此值设置为false也会禁用<see cref="IExecutionStrategy"/></para>
        /// <para>对于<see cref="DbContext.SaveChanges()"/>默认值为true，这意味着<see cref="DbContext.SaveChanges()"/>将始终使用事务在保存更改。</para>
        /// <para>将此值设置为false应该非常小心，因为数据库如果<see cref="DbContext.SaveChanges()"/>失败，可能会处于损坏状态。</para>
        /// </summary>
        bool AutoTransactionsEnabled { get; set; }
        /// <summary>
        /// 创建执行策略
        /// </summary>
        /// <returns></returns>
        IExecutionStrategy CreateExecutionStrategy();
        /// <summary>
        /// 事务开始
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction();
        /// <summary>
        /// 异步事务开始
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 设置DbTransaction用于在db上的数据库操作。
        /// </summary>
        /// <param name="contextTransaction"></param>
        /// <returns></returns>
        IDbContextTransaction UseTransaction(IDbContextTransaction contextTransaction);
        /// <summary>
        /// 设置DbTransaction用于在db上的数据库操作。
        /// </summary>
        /// <param name="contextTransaction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IDbContextTransaction> UseTransactionAsync(IDbContextTransaction contextTransaction, CancellationToken cancellationToken = default);
        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTransaction();
        /// <summary>
        /// 事务回滚
        /// </summary>
        void RollbackTransaction();
        /// <summary>
        /// 是否连接
        /// </summary>
        /// <returns></returns>
        bool CanConnect();
        /// <summary>
        /// 当前事务
        /// </summary>
        IDbContextTransaction CurrentTransaction { get; }
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        IDbConnection DbConnection { get; }
    }
}
