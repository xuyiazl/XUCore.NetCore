using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Swagger
{
    /// <summary>
    /// 指定字段输出
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class FieldResponseAttribute : ActionFilterAttribute
    {
    }
}
