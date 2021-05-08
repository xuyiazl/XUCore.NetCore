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
        /// <param name="unitOfWork"></param>
        /// <param name="run">执行内容</param>
        /// <param name="error">异常消息</param>
        public static void CreateTransaction(this IUnitOfWork unitOfWork,
            Action<IDbContextTransaction> run,
            Action<IDbContextTransaction, Exception> error)
        {
            var strategy = unitOfWork.CreateExecutionStrategy();

            strategy.Execute(() =>
            {
                using (var tran = unitOfWork.BeginTransaction())
                {
                    try
                    {
                        run.Invoke(tran);

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();

                        error.Invoke(tran, ex);
                    }
                }
            });
        }
        /// <summary>
        /// 创建事务
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="run">执行内容</param>
        /// <param name="error">异常消息</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task CreateTransactionAsync(this IUnitOfWork unitOfWork,
            Func<IDbContextTransaction, CancellationToken, Task> run,
            Func<IDbContextTransaction, Exception, CancellationToken, Task> error,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var strategy = unitOfWork.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async (_cancel) =>
            {
                using (var tran = await unitOfWork.BeginTransactionAsync(_cancel))
                {
                    try
                    {
                        await run.Invoke(tran, _cancel);

                        await tran.CommitAsync(_cancel);
                    }
                    catch (Exception ex)
                    {
                        await tran.RollbackAsync(_cancel);

                        await error.Invoke(tran, ex, _cancel);
                    }
                }
            }, cancellationToken);
        }
        /// <summary>
        /// 创建事务
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="run">执行内容</param>
        /// <param name="error">异常消息</param>
        /// <returns></returns>
        public static TResult CreateTransaction<TResult>(this IUnitOfWork unitOfWork,
            Func<IDbContextTransaction, TResult> run,
            Func<IDbContextTransaction, Exception, TResult> error)
        {
            var strategy = unitOfWork.CreateExecutionStrategy();

            return strategy.Execute(() =>
            {
                var tResult = default(TResult);

                using (var tran = unitOfWork.BeginTransaction())
                {
                    try
                    {
                        tResult = run.Invoke(tran);

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();

                        tResult = error.Invoke(tran, ex);
                    }
                }

                return tResult;
            });
        }
        /// <summary>
        /// 创建事务
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="run">执行内容</param>
        /// <param name="error">异常消息</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<TResult> CreateTransactionAsync<TResult>(this IUnitOfWork unitOfWork,
            Func<IDbContextTransaction, CancellationToken, Task<TResult>> run,
            Func<IDbContextTransaction, Exception, CancellationToken, Task<TResult>> error,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var strategy = unitOfWork.CreateExecutionStrategy();

            return strategy.ExecuteAsync(async (_cancel) =>
            {
                var tResult = default(TResult);

                using (var tran = await unitOfWork.BeginTransactionAsync(_cancel))
                {
                    try
                    {
                        tResult = await run.Invoke(tran, _cancel);

                        await tran.CommitAsync(_cancel);
                    }
                    catch (Exception ex)
                    {
                        await tran.RollbackAsync(_cancel);

                        tResult = await error.Invoke(tran, ex, _cancel);
                    }
                }

                return tResult;
            },
            cancellationToken);
        }
        /// <summary>
        /// 创建事务（适用于多数据库连接的情况）
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="run">执行内容</param>
        /// <param name="error">异常消息</param>
        public static void CreateTransactionScope(this IUnitOfWork unitOfWork,
            Action<TransactionScope> run,
            Action<TransactionScope, Exception> error)
        {
            var strategy = unitOfWork.CreateExecutionStrategy();

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
                        Transaction.Current.Rollback();

                        error.Invoke(tran, ex);
                    }
                }
            }
            );
        }
        /// <summary>
        /// 创建事务（适用于多数据库连接的情况）
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="run">执行内容</param>
        /// <param name="error">异常消息</param>
        /// <param name="cancellationToken"></param>
        public static async Task CreateTransactionScopeAsync(this IUnitOfWork unitOfWork,
            Func<TransactionScope, CancellationToken, Task> run,
            Func<TransactionScope, Exception, CancellationToken, Task> error,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var strategy = unitOfWork.CreateExecutionStrategy();

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
                        Transaction.Current.Rollback();

                        await error.Invoke(tran, ex, _cancel);
                    }
                }
            },
            cancellationToken);
        }
        /// <summary>
        /// 创建事务（适用于多数据库连接的情况）
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="run">执行内容</param>
        /// <param name="error">异常消息</param>
        /// <returns></returns>
        public static TResult CreateTransactionScope<TResult>(this IUnitOfWork unitOfWork,
            Func<TransactionScope, TResult> run,
            Func<TransactionScope, Exception, TResult> error)
        {
            var strategy = unitOfWork.CreateExecutionStrategy();

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
                        Transaction.Current.Rollback();

                        tResult = error.Invoke(tran, ex);
                    }
                }

                return tResult;
            });
        }
        /// <summary>
        /// 创建事务（适用于多数据库连接的情况）
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="run">执行内容</param>
        /// <param name="error">异常消息</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<TResult> CreateTransactionScope<TResult>(this IUnitOfWork unitOfWork,
            Func<TransactionScope, CancellationToken, Task<TResult>> run,
            Func<TransactionScope, Exception, CancellationToken, Task<TResult>> error,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var strategy = unitOfWork.CreateExecutionStrategy();

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
                        Transaction.Current.Rollback();

                        tResult = await error.Invoke(tran, ex, _cancel);
                    }
                }

                return tResult;
            },
            cancellationToken);
        }
    }
}
