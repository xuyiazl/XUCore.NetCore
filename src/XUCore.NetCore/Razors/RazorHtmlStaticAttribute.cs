using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using XUCore.NetCore.Extensions;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.IO;
using System;
using System.IO;
using System.Threading.Tasks;

namespace XUCore.NetCore.Razors
{
    /// <summary>
    /// Razor生成Html静态文件（保存目录为wwwroot）
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RazorHtmlStaticAttribute : ActionFilterAttribute
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
            if (IsBuildHtml(context))
                WriteHtml(context, (ViewResult)context.Result);

            base.OnResultExecuted(context);
        }

        /// <summary>
        /// 根据条件判断是否允许生成HTML
        /// </summary>
        /// <param name="routes"></param>
        /// <returns></returns>
        protected bool IsBuildHtml(ResultExecutedContext context)
        {
            if (MinInterval <= 0) return true;

            var path = Common.GetWebRootPath(context.RouteReplace(Template));

            var fi = new FileInfo(path);

            if (!fi.Exists) return true;

            var time = fi.LastWriteTime.DateDiff(DateTime.Now);

            return time >= TimeSpan.FromSeconds(MinInterval);
        }

        /// <summary>
        /// 写HTML
        /// </summary>
        /// <param name="context"></param>
        /// <param name="viewResult"></param>
        protected void WriteHtml(ResultExecutedContext context, ViewResult viewResult)
        {
            var _logger = Web.GetService<ILogger<RazorHtmlStaticAttribute>>();
            try
            {
                var html = viewResult?.ToHtml(context.HttpContext, IsPartialView);

                if (string.IsNullOrWhiteSpace(html)) return;

                var path = Common.GetWebRootPath(context.RouteReplace(Template));

                FileHelper.Create(path, html);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成html静态文件失败");
            }
        }
    }
}