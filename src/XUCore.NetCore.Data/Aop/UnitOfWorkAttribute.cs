using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.AspectCore;

namespace XUCore.NetCore.Data
{
    /// <summary>
    /// 工作单元AOP（请求自动启用工作单元模式，要么成功，要么失败。）
    /// </summary>
    public class UnitOfWorkAttribute : InterceptorBase
    {
        /// <summary>
        /// Type of DbContext
        /// </summary>
        public Type DbType { get; set; }
        /// <summary>
        /// 工作单元AOP
        /// </summary>
        /// <param name="dbType">上下文<see cref="IDbContext"/></param>
        public UnitOfWorkAttribute(Type dbType)
        {
            DbType = dbType;
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var dbContext = context.ServiceProvider.GetService(DbType) as IDbContext;

            if (dbContext == null) throw new ArgumentNullException("DbType is null");

            IUnitOfWork unitOfWork = new UnitOfWorkService(dbContext);

            //await next(context);

            //await unitOfWork.CommitAsync();

            await unitOfWork.CreateTransactionAsync(
                async (tran, cancel) =>
                {
                    await next(context);

                    await unitOfWork.CommitAsync();
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
