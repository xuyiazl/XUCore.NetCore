using AspectCore.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.AspectCore;
using XUCore.NetCore.Data.DbService;

namespace XUCore.NetCore.DataTest
{
    /// <summary>
    /// 测试事务AOP - demo
    /// </summary>
    public class TransactionAttribute : InterceptorBase
    {
        /// <summary>
        /// Type of DbContext
        /// </summary>
        public Type DbType { get; set; }
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var dbContext = context.ServiceProvider.GetService(DbType) as IDbContext;

            if (dbContext == null) throw new ArgumentNullException("DbType is null");

            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);

            await unitOfWork.CreateTransactionAsync(
                async (tran, cancel) =>
                {
                    await next(context);
                },
                async (tran, error, cancel) =>
                {
                    await Task.CompletedTask;

                    throw error;
                },
                CancellationToken.None);
        }
    }
    /// <summary>
    /// 测试事务AOP - demo
    /// </summary>
    public class Transaction1Attribute : InterceptorBase
    {
        /// <summary>
        /// Type of DbContext
        /// </summary>
        public Type DbType { get; set; }
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var dbContext = context.ServiceProvider.GetService(DbType) as IDbContext;

            if (dbContext == null) throw new ArgumentNullException("DbType is null");

            IUnitOfWork unitOfWork = new UnitOfWork(dbContext);

            var strategy = unitOfWork.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async (_cancel) =>
            {
                using (var tran = await unitOfWork.BeginTransactionAsync(_cancel))
                {
                    try
                    {
                        await next(context);

                        await tran.CommitAsync(_cancel);
                    }
                    catch (Exception ex)
                    {
                        await tran.RollbackAsync(_cancel);

                        throw;
                    }
                }
            }, CancellationToken.None);
        }
    }
}
