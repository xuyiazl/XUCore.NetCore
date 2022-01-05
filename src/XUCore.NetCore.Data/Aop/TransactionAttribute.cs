using AspectCore.DynamicProxy;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
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
        /// 事务范围选项
        /// </summary>
        public TransactionScopeOption ScopeOption { get; set; } = TransactionScopeOption.Required;
        /// <summary>
        /// 隔离级别
        /// </summary>
        public IsolationLevel Level { get; set; } = IsolationLevel.ReadCommitted;
        /// <summary>
        /// 工作单元AOP
        /// </summary>
        public UnitOfWorkAttribute()
        {

        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            using (var scope = new TransactionScope(ScopeOption, new TransactionOptions { IsolationLevel = Level }))
            {
                await next(context);

                scope.Complete();
            }
        }

        private TransactionScope scope;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            scope = new TransactionScope(ScopeOption, new TransactionOptions
            {
                IsolationLevel = Level
            });
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                if (context.Exception == null)
                    scope.Complete();

                scope.Dispose();
            }
            catch { }
        }
    }
}
