using AspectCore.DynamicProxy.Parameters;
using System;
using System.Threading.Tasks;
using XUCore.Extensions;

namespace XUCore.NetCore.AspectCore
{
    /// <summary>
    /// 验证参数不可为空
    /// </summary>
    public class NotEmptyAttribute : ParameterInterceptorBase
    {
        public override Task Invoke(ParameterAspectContext context, ParameterAspectDelegate next)
        {
            if (context.Parameter.Value.SafeString().IsEmpty())
            {
                throw new ArgumentNullException(context.Parameter.Name);
            }
            return next(context);
        }
    }
}
