using Microsoft.AspNetCore.Mvc.Filters;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;

namespace XUCore.SimpleApi.Template.Applaction.Filters
{
    public class ApiFluentValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.ActionArguments?.ForEach(c => (c.Value as Command<bool>)?.IsVaild());

            base.OnActionExecuting(context);
        }
    }
}
