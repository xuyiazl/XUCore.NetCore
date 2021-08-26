using AspectCore.DynamicProxy;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;
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
    /// 工作单元AOP（使用事务执行）
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitOfWorkAttribute : InterceptorBase, IActionFilter
    {
        /// <summary>
        /// Type of DbContext
        /// </summary>
        public Type DbType { get; set; }
        /// <summary>
        /// 工作单元AOP
        /// </summary>
        public UnitOfWorkAttribute()
        {

        }

        IUnitOfWork unitOfWork;
        IDbContextTransaction tran;

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var dbContext = context.ServiceProvider.GetService(DbType) as IDbContext;

            if (dbContext == null) throw new ArgumentNullException("DbType is null");

            try
            {
                await OnBefore(dbContext);

                await next(context);

                await OnAfter(null);
            }
            catch (Exception ex)
            {
                await OnAfter(ex);

                throw;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context) => OnBefore(context.HttpContext.RequestServices.GetService(DbType) as IDbContext);
        public void OnActionExecuted(ActionExecutedContext context) => OnAfter(context.Exception);

        Task OnBefore(IDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException("DbType is null");

            unitOfWork = new UnitOfWorkService(dbContext);

            tran = unitOfWork.BeginTransaction();

            return Task.FromResult(false);
        }

        Task OnAfter(Exception ex)
        {
            try
            {
                if (ex == null)
                    tran.Commit();
                else
                    tran.Rollback();
            }
            finally
            {
                tran.Dispose();
            }
            return Task.FromResult(false);
        }
    }
}
