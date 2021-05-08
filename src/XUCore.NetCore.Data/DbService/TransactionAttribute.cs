using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.AspectCore;

namespace XUCore.NetCore.Data.DbService
{
    /// <summary>
    /// 事务AOP
    /// </summary>
    public class TransactionAttribute : InterceptorBase
    {
        /// <summary>
        /// Type of DbContext
        /// </summary>
        public Type DbType { get; set; }
        /// <summary>
        /// 事务AOP
        /// </summary>
        /// <param name="dbType">上下文<see cref="IDbContext"/></param>
        public TransactionAttribute(Type dbType)
        {
            DbType = dbType;
        }

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
