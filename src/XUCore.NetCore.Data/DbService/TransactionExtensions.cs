using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace XUCore.NetCore.Data.DbService
{
    public static class TransactionExtensions
    {
        /// <summary>
        /// 创建事务
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="run">执行内容</param>
        /// <param name="error">异常消息</param>
        public static void CreateTransaction(this IDbContext dbContext,
            Action<IDbContextTransaction> run,
            Action<IDbContextTransaction, Exception> error)
        {
            var strategy = dbContext.CreateExecutionStrategy();

            strategy.Execute(() =>
            {
                using (var tran = dbContext.BeginTransaction())
                {
                    try
                    {
                        run.Invoke(tran);

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        error.Invoke(tran, ex);

                        tran.Rollback();
                    }
                }
            });
        }
        /// <summary>
        /// 创建事务
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="run">执行内容</param>
        /// <param name="error">异常消息</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task CreateTransactionAsync(this IDbContext dbContext,
            Func<IDbContextTransaction, CancellationToken, Task> run,
            Func<IDbContextTransaction, Exception, CancellationToken, Task> error,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var strategy = dbContext.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async (_cancel) =>
            {
                using (var tran = await dbContext.BeginTransactionAsync(_cancel))
                {
                    try
                    {
                        await run.Invoke(tran, _cancel);

                        await tran.CommitAsync(_cancel);
                    }
                    catch (Exception ex)
                    {
                        await error.Invoke(tran, ex, _cancel);

                        await tran.RollbackAsync(_cancel);
                    }
                }
            }, cancellationToken);
        }
        /// <summary>
        /// 创建事务
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="run">执行内容</param>
        /// <param name="error">异常消息</param>
        /// <returns></returns>
        public static TResult CreateTransaction<TResult>(this IDbContext dbContext,
            Func<IDbContextTransaction, TResult> run,
            Func<IDbContextTransaction, Exception, TResult> error)
        {
            var strategy = dbContext.CreateExecutionStrategy();

            return strategy.Execute(() =>
            {
                var tResult = default(TResult);

                using (var tran = dbContext.BeginTransaction())
                {
                    try
                    {
                        tResult = run.Invoke(tran);

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tResult = error.Invoke(tran, ex);

                        tran.Rollback();
                    }
                }

                return tResult;
            });
        }
        /// <summary>
        /// 创建事务
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="run">执行内容</param>
        /// <param name="error">异常消息</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<TResult> CreateTransactionAsync<TResult>(this IDbContext dbContext,
            Func<IDbContextTransaction, CancellationToken, Task<TResult>> run,
            Func<IDbContextTransaction, Exception, CancellationToken, Task<TResult>> error,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var strategy = dbContext.CreateExecutionStrategy();

            return strategy.ExecuteAsync(async (_cancel) =>
            {
                var tResult = default(TResult);

                using (var tran = await dbContext.BeginTransactionAsync(_cancel))
                {
                    try
                    {
                        tResult = await run.Invoke(tran, _cancel);

                        await tran.CommitAsync(_cancel);
                    }
                    catch (Exception ex)
                    {
                        tResult = await error.Invoke(tran, ex, _cancel);

                        await tran.RollbackAsync(_cancel);
                    }
                }

                return tResult;
            },
            cancellationToken);
        }
        /// <summary>
        /// 创建事务（适用于多数据库连接的情况）
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="run">执行内容</param>
        /// <param name="error">异常消息</param>
        public static void CreateTransactionScope(this IDbContext dbContext,
            Action<TransactionScope> run,
            Action<TransactionScope, Exception> error)
        {
            var strategy = dbContext.CreateExecutionStrategy();

            strategy.Execute(() =>
            {
                using (var tran = new TransactionScope())
                {
                    try
                    {
                        run.Invoke(tran);

                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        error.Invoke(tran, ex);

                        Transaction.Current.Rollback();
                    }
                }
            }
            );
        }
        /// <summary>
        /// 创建事务（适用于多数据库连接的情况）
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="run">执行内容</param>
        /// <param name="error">异常消息</param>
        /// <param name="cancellationToken"></param>
        public static async Task CreateTransactionScopeAsync(this IDbContext dbContext,
            Func<TransactionScope, CancellationToken, Task> run,
            Func<TransactionScope, Exception, CancellationToken, Task> error,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var strategy = dbContext.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async (_cancel) =>
            {
                using (var tran = new TransactionScope())
                {
                    try
                    {
                        await run.Invoke(tran, _cancel);

                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        await error.Invoke(tran, ex, _cancel);

                        Transaction.Current.Rollback();
                    }
                }
            },
            cancellationToken);
        }
        /// <summary>
        /// 创建事务（适用于多数据库连接的情况）
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="run">执行内容</param>
        /// <param name="error">异常消息</param>
        /// <returns></returns>
        public static TResult CreateTransactionScope<TResult>(this IDbContext dbContext,
            Func<TransactionScope, TResult> run,
            Func<TransactionScope, Exception, TResult> error)
        {
            var strategy = dbContext.CreateExecutionStrategy();

            return strategy.Execute(() =>
            {
                var tResult = default(TResult);

                using (var tran = new TransactionScope())
                {
                    try
                    {
                        tResult = run.Invoke(tran);

                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        tResult = error.Invoke(tran, ex);

                        Transaction.Current.Rollback();
                    }
                }

                return tResult;
            });
        }
        /// <summary>
        /// 创建事务（适用于多数据库连接的情况）
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="run">执行内容</param>
        /// <param name="error">异常消息</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> CreateTransactionScope<TResult>(this IDbContext dbContext,
            Func<TransactionScope, CancellationToken, Task<TResult>> run,
            Func<TransactionScope, Exception, CancellationToken, Task<TResult>> error,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var strategy = dbContext.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async (_cancel) =>
            {
                var tResult = default(TResult);

                using (var tran = new TransactionScope())
                {
                    try
                    {
                        tResult = await run.Invoke(tran, _cancel);

                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        tResult = await error.Invoke(tran, ex, _cancel);

                        Transaction.Current.Rollback();
                    }
                }

                return tResult;
            },
            cancellationToken);
        }
    }
}
