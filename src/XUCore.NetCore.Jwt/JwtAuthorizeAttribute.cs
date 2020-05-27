using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Reflection;

namespace XUCore.NetCore.Jwt
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class JwtAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!VerifyAttribute(filterContext.ActionDescriptor))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            var user = filterContext.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                filterContext.Result = new Result(StateCode.Fail, "401", "Unauthorized");
                return;
            }

            //jwt为无状态，可以在此处维护一个状态机制

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 是否跳过验证
        /// </summary>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        private bool VerifyAttribute(ActionDescriptor actionDescriptor)
        {
            var controllerActionDescriptor = actionDescriptor as ControllerActionDescriptor;
            var htmlAttribute = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<JwtAllowAnonymousAttribute>() ??
                              controllerActionDescriptor.MethodInfo.GetCustomAttribute<JwtAllowAnonymousAttribute>();
            return htmlAttribute == null;
        }
    }
}