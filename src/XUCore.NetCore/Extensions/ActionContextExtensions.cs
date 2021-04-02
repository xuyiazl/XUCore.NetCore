using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using XUCore.Extensions;

namespace XUCore.NetCore.Extensions
{
    /// <summary>
    /// 操作上下文(<see cref="ActionContext"/>) 扩展
    /// </summary>
    public static class ActionContextExtensions
    {
        /// <summary>
        /// 获取Area名称
        /// </summary>
        /// <param name="context">操作上下文</param>
        /// <returns></returns>
        public static string GetAreaName(this ActionContext context)
        {
            string area = null;
            if (context.RouteData.Values.TryGetValue("area", out object value))
            {
                area = value.SafeString();
                if (area.IsEmpty())
                {
                    area = null;
                }
            }

            return area;
        }

        /// <summary>
        /// 获取Controller名称
        /// </summary>
        /// <param name="context">操作上下文</param>
        /// <returns></returns>
        public static string GetControllerName(this ActionContext context)
        {
            string controller = null;
            if (context.RouteData.Values.TryGetValue("controller", out object value))
            {
                controller = value.SafeString();
                if (controller.IsEmpty())
                {
                    controller = null;
                }
            }

            return controller;
        }

        /// <summary>
        /// 获取Action名称
        /// </summary>
        /// <param name="context">操作上下文</param>
        /// <returns></returns>
        public static string GetActionName(this ActionContext context)
        {
            string action = null;
            if (context.RouteData.Values.TryGetValue("action", out object value))
            {
                action = value.SafeString();
                if (action.IsEmpty())
                {
                    action = null;
                }
            }

            return action;
        }

        /// <summary>
        /// 获取所有路由信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static RouteValueDictionary GetRouteValues(this ActionContext context) => context.RouteData.Values;

        /// <summary>
        /// 根据路由参数进行模板替换
        /// </summary>
        /// <param name="context"></param>
        /// <param name="template">比如：static/{area}/{controller}/{action}/{id}.html</param>
        /// <returns></returns>
        public static string RouteReplace(this ActionContext context, string template)
        {
            var path = template;

            foreach (var route in context.GetRouteValues())
                path = path.Replace("{" + route.Key + "}", route.Value.SafeString());

            return path.ToLower();
        }
    }
}