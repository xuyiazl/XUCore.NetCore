using AspectCore.DynamicProxy;
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
}
