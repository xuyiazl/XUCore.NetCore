using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace XUCore.NetCore.AccessControl
{
    /// <summary>
    /// 权限控制
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class AccessControlAttribute : Attribute, IAuthorizationFilter

    {
        public string AccessKey { get; set; }

        public virtual void OnAuthorization(AuthorizationFilterContext filterContext)

        {
            if (filterContext == null)
                throw new ArgumentNullException(nameof(filterContext));

            var isDefinedNoControl = filterContext.ActionDescriptor.IsDefined(typeof(NoAccessControlAttribute), true);

            if (!isDefinedNoControl)
            {
                IResourceAccessStrategy accessStrategy;

                accessStrategy = filterContext.HttpContext.RequestServices.GetRequiredService<IResourceAccessStrategy>();

                if (accessStrategy == null)
                    throw new ArgumentException("IResourceAccessStrategy not initialized，please register your ResourceAccessStrategy", nameof(IResourceAccessStrategy));

                if (!accessStrategy.IsCanAccess(AccessKey))
                {
                    //if Ajax request
                    filterContext.Result = filterContext.HttpContext.Request.IsAjaxRequest() ?
                        accessStrategy.DisallowedAjaxResult :
                        accessStrategy.DisallowedCommonResult;
                }
            }
        }
    }

    public static class AjaxRequestExtensions
    {
        public static bool IsAjaxRequest(this Microsoft.AspNetCore.Http.HttpRequest request)
        {
            return request?.Headers != null && string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsDefined(this Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor actionDescriptor,
            Type attributeType, bool inherit)
        {
            if (actionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                if (controllerActionDescriptor.MethodInfo.GetCustomAttribute(attributeType) == null)
                {
                    if (inherit && controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute(attributeType) != null)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
    }
}
