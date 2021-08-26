using AspectCore.DynamicProxy;
using FreeSql;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.NetCore.AspectCore;
using XUCore.NetCore.FreeSql.Curd;

namespace XUCore.NetCore.FreeSql
{
    /// <summary>
    /// 使用事务执行
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionAttribute : InterceptorBase, IActionFilter
    {
        /// <summary>
        /// Type of UnitOfWorkManage
        /// </summary>
        public Type UowmType { get; set; } = typeof(FreeSqlUnitOfWorkManager);
        /// <summary>
        /// 事务传播方式
        /// </summary>
        public Propagation Propagation { get; set; } = Propagation.Required;
        public IsolationLevel IsolationLevel { get => _IsolationLevelPriv.Value; set => _IsolationLevelPriv = value; }

        IsolationLevel? _IsolationLevelPriv;

        IUnitOfWork _uow;
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var uowm = context.ServiceProvider.GetService(UowmType) as UnitOfWorkManager;
            try
            {
                await OnBefore(uowm);

                await next(context);

                await OnAfter(null);
            }
            catch (Exception ex)
            {
                await OnAfter(ex);

                throw;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context) => OnBefore(context.HttpContext.RequestServices.GetService(UowmType) as UnitOfWorkManager).Wait();

        public void OnActionExecuted(ActionExecutedContext context) => OnAfter(context.Exception).Wait();

        async Task OnBefore(UnitOfWorkManager uowm)
        {
            _uow = uowm.Begin(this.Propagation, this._IsolationLevelPriv);

            await Task.FromResult(false);
        }

        async Task OnAfter(Exception ex)
        {
            try
            {
                if (ex == null) _uow.Commit();
                else _uow.Rollback();
            }
            finally
            {
                _uow.Dispose();
            }
            await Task.FromResult(false);
        }
    }
}
