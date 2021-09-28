using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace XUCore.NetCore.Filters
{
    /// <summary>
    /// API查询时间
    /// </summary>
    public class ApiElapsedTimeActionFilter : IActionFilter
    {
        private string ElapsedField = "ElapsedTime";
        private bool Open = true;

        public ApiElapsedTimeActionFilter(bool Open = true, string ElapsedField = "ElapsedTime")
        {
            this.Open = Open;
            this.ElapsedField = ElapsedField;
        }

        private Stopwatch stopwatch = new Stopwatch();

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (Open)
            {
                if (stopwatch == null)
                    stopwatch = new Stopwatch();
                stopwatch.Reset();
                stopwatch.Restart();
            }
        }

        public void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            if (Open)
            {
                if (stopwatch == null)
                    stopwatch = new Stopwatch();
                stopwatch.Stop();

                var reType = actionExecutedContext.Result?.GetType();

                if (reType == typeof(Result))
                {
                    var res = (Result)actionExecutedContext.Result;
                    if (res != null)
                    {
                        res.ElapsedTime = stopwatch.ElapsedMilliseconds;
                        //res.Value?.GetType().GetProperty("elapsedTime").SetValue(res.Value, stopwatch.ElapsedMilliseconds);

                        actionExecutedContext.Result = res;
                    }
                }
                else if (reType == typeof(ObjectResult))
                {
                    var res = (ObjectResult)actionExecutedContext.Result;
                    if (res != null && res.Value.GetType().Name == typeof(Result<>).Name)
                    {
                        res.Value?.GetType().GetProperty(ElapsedField).SetValue(res.Value, stopwatch.ElapsedMilliseconds);

                        actionExecutedContext.Result = res;
                    }
                }
            }
        }
    }
}