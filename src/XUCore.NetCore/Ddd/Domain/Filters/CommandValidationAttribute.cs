using Microsoft.AspNetCore.Mvc.Filters;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;

namespace XUCore.Ddd.Domain.Filters
{
    /// <summary>
    /// FluentValidation 验证拦截（因使用动态API，自动验证失效，导致需要手工操作。）
    /// </summary>
    public class CommandValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.ActionArguments?.ForEach(c => (c.Value as Command)?.IsVaild());
            base.OnActionExecuting(context);
        }
    }

    /// <summary>
    /// FluentValidation 验证拦截（因使用动态API，自动验证失效，导致需要手工操作。）
    /// </summary>
    public class CommandValidationActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.ActionArguments?.ForEach(c => (c.Value as Command)?.IsVaild());
        }
    }
}
