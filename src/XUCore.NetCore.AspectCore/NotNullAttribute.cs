using AspectCore.DynamicProxy.Parameters;
using System;
using System.Threading.Tasks;

namespace XUCore.NetCore.AspectCore
{
    /// <summary>
    /// 验证参数不可为null
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class NotNullAttribute : ParameterInterceptorBase
    {
        public override Task Invoke(ParameterAspectContext context, ParameterAspectDelegate next)
        {
            if (context.Parameter.Value == null)
            {
                throw new ArgumentNullException(context.Parameter.Name);
            }
            return next(context);
        }
    }
}
