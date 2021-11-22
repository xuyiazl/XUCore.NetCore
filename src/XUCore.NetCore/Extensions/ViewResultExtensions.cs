using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

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
        /// <summary>
        /// 转换成Html
        /// </summary>
        /// <param name="pageModel"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public static async Task<string> ToHtml(this PageModel pageModel, string pageName)
        {
            var actionContext = new ActionContext(
                pageModel.HttpContext,
                pageModel.RouteData,
                pageModel.PageContext.ActionDescriptor
            );

            using (var sw = new StringWriter())
            {
                IRazorViewEngine _razorViewEngine = pageModel.HttpContext.RequestServices.GetRequiredService(typeof(IRazorViewEngine)) as IRazorViewEngine;
                IRazorPageActivator _activator = pageModel.HttpContext.RequestServices.GetRequiredService(typeof(IRazorPageActivator)) as IRazorPageActivator;

                var result = _razorViewEngine.FindPage(actionContext, pageName);

                if (result.Page == null)
                {
                    throw new ArgumentNullException($"The page {pageName} cannot be found.");
                }

                var page = result.Page;

                var view = new RazorView(_razorViewEngine,
                    _activator,
                    new List<IRazorPage>(),
                    page,
                    HtmlEncoder.Default,
                    new DiagnosticListener("ViewRenderService"));


                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    pageModel.ViewData,
                    pageModel.TempData,
                    sw,
                    new HtmlHelperOptions()
                );


                var pageNormal = ((Page)result.Page);

                pageNormal.PageContext = pageModel.PageContext;

                pageNormal.ViewContext = viewContext;


                _activator.Activate(pageNormal, viewContext);

                await page.ExecuteAsync();

                return sw.ToString();
            }
        }
    }
}