using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.IO;

namespace XUCore.NetCore.Razors
{
    /// <summary>
    /// Razor生成Html静态文件（保存目录为wwwroot）
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class HtmlStaticAttribute : ActionFilterAttribute
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
        /// 结果执行之前 before
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if ((context.Result is PageResult || context.Result is ViewResult) && IsBuildHtml(RouteReplace(context, Template)))
            {
                var response = context.HttpContext.Response;
                if (!response.Body.CanRead || !response.Body.CanSeek)
                {
                    using (var ms = new MemoryStream())
                    {
                        var old = response.Body;
                        response.Body = ms;

                        await base.OnResultExecutionAsync(context, next);

                        if (response.StatusCode == 200)
                        {
                            await WriteHtml(context, response.Body);
                        }
                        ms.Position = 0;
                        await ms.CopyToAsync(old);
                        response.Body = old;
                    }
                }
                else
                {
                    await base.OnResultExecutionAsync(context, next);

                    var old = response.Body.Position;
                    if (response.StatusCode == 200)
                    {
                        await WriteHtml(context, response.Body);
                    }
                    response.Body.Position = old;
                }
            }
            else
            {
                await base.OnResultExecutionAsync(context, next);
            }
        }

        /// <summary>
        /// 根据路由参数进行模板替换
        /// </summary>
        /// <param name="context"></param>
        /// <param name="template">比如：static/{area}/{controller}/{action}/{id}.html</param>
        /// <returns></returns>
        public static string RouteReplace(ActionContext context, string template)
        {
            var path = template;

            foreach (var route in context.RouteData.Values)
                path = path.Replace("{" + route.Key + "}", route.Value.SafeString());

            return path.ToLower();
        }
        /// <summary>
        /// 根据条件判断是否允许生成HTML
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        protected bool IsBuildHtml(string relativePath)
        {
            if (MinInterval <= 0) return true;

            var path = Common.GetWebRootPath(relativePath);

            var fi = new FileInfo(path);

            if (!fi.Exists) return true;

            var time = fi.LastWriteTime.DateDiff(DateTime.Now);

            return time >= TimeSpan.FromSeconds(MinInterval);
        }
        /// <summary>
        /// 写HTML
        /// </summary>
        /// <param name="context"></param>
        /// <param name="stream"></param>
        protected async Task WriteHtml(ResultExecutingContext context, Stream stream)
        {
            stream.Position = 0;
            var responseReader = new StreamReader(stream);
            var responseContent = await responseReader.ReadToEndAsync();

            if (string.IsNullOrEmpty(responseContent)) return;

            var _logger = Web.GetService<ILogger<HtmlStaticAttribute>>();
            try
            {
                var path = Common.GetWebRootPath(RouteReplace(context, Template));

                FileHelper.Create(path, responseContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成html静态文件失败");
            }
        }
    }
}
