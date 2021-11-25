using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.IO;

namespace XUCore.NetCore.RazorTests
{
    /// <summary>
    /// Razor生成Html静态文件（保存目录为wwwroot）
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class PageModelHtmlStaticAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 路径模板，范例：static/{area}/{controller}/{action}.component.html
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// 生成的最小间隔，单位（秒），比如设置5分钟，那么5分钟之内不会再生成
        /// </summary>
        public int MinInterval { get; set; } = 0;

        /// <summary>
        /// 是否部分视图，默认：false
        /// </summary>
        public bool IsPartialView { get; set; } = false;

        /// <summary>
        /// 结果执行之前 before
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            await base.OnResultExecutionAsync(context, next);
        }

        /// <summary>
        /// 结果执行之后 after
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (IsBuildHtml(context.RouteReplace(Template)))
            {
                WriteHtml(context, (PageResult)context.Result);
            }
            base.OnResultExecuted(context);
        }

        /// <summary>
        /// 根据条件判断是否允许生成HTML
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        protected bool IsBuildHtml(string relativePath)
        {
            if (MinInterval <= 0) return true;

            var path = Web.GetWebRootPath(relativePath);

            var fi = new FileInfo(path);

            if (!fi.Exists) return true;

            var time = fi.LastWriteTime.DateDiff(DateTime.Now);

            return time >= TimeSpan.FromSeconds(MinInterval);
        }
        /// <summary>
        /// 写HTML
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pageResult"></param>
        protected void WriteHtml(ResultExecutedContext context, PageResult pageResult)
        {
            if (pageResult == null) return;
            
            var _logger = Web.GetService<ILogger<PageModelHtmlStaticAttribute>>();
            try
            {
                var routeData = context.HttpContext.GetRouteData();

                var pageName = routeData.Values["page"] as string;

                var html = ToHtml(pageResult, pageName.Substring(1, pageName.Length - 1)).Result;

                if (string.IsNullOrWhiteSpace(html)) return;

                var path = Web.GetWebRootPath(context.RouteReplace(Template));

                FileHelper.Create(path, html);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成html静态文件失败");
            }
        }


        /// <summary>
        /// 转换成Html
        /// </summary>
        /// <param name="pageModel"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        protected async Task<string> ToHtml(PageResult pageResult, string pageName)
        {
            var pageModel = (pageResult.Model as PageModel);

            var actionContext = new ActionContext(
                pageModel.HttpContext,
                pageModel.RouteData,
                pageModel.PageContext.ActionDescriptor
            );

            using (var sw = new StringWriter())
            {
                IRazorViewEngine _razorViewEngine = pageModel.HttpContext.RequestServices.GetService(typeof(IRazorViewEngine)) as IRazorViewEngine;
                IRazorPageActivator _activator = pageModel.HttpContext.RequestServices.GetService(typeof(IRazorPageActivator)) as IRazorPageActivator;

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