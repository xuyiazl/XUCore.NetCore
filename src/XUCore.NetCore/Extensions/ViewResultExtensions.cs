using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;
using System.Linq;
using System.Text;

namespace XUCore.NetCore.Extensions
{
    /// <summary>
    /// 视图结果 扩展
    /// </summary>
    public static class ViewResultExtensions
    {
        /// <summary>
        /// 转换成Html
        /// </summary>
        /// <param name="result">视图结果</param>
        /// <param name="httpContext">Http上下文</param>
        /// <param name="IsPartialView">是否部分视图，默认：false</param>
        /// <returns></returns>
        public static string ToHtml(this ViewResult result, HttpContext httpContext, bool IsPartialView)
        {
            if (result == null) return string.Empty;

            var routeData = httpContext.GetRouteData();

            var viewName = result.ViewName ?? routeData.Values["action"] as string;
            var actionContext = new ActionContext(httpContext, routeData, new CompiledPageActionDescriptor());
            var options = httpContext.RequestServices.GetRequiredService<IOptions<MvcViewOptions>>();
            var htmlHelperOptions = options.Value.HtmlHelperOptions;
            var viewEngineResult = result.ViewEngine?.FindView(actionContext, viewName, !IsPartialView) ?? options.Value
                                       .ViewEngines.Select(x => x.FindView(actionContext, viewName, !IsPartialView))
                                       .FirstOrDefault(x => x != null);
            var view = viewEngineResult.View;
            var builder = new StringBuilder();

            using (var output = new StringWriter(builder))
            {
                var viewContext = new ViewContext(actionContext, view, result.ViewData, result.TempData, output,
                    htmlHelperOptions);

                view.RenderAsync(viewContext).GetAwaiter().GetResult();
            }

            return builder.ToString();
        }
    }
}