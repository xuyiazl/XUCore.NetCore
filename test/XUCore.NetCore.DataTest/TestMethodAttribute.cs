using AspectCore.DynamicProxy;
using System.Threading.Tasks;
using XUCore.NetCore.AspectCore;

namespace XUCore.NetCore.DataTest
{
    public class TestMethodAttribute : InterceptorBase
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            await next(context);
        }
    }
}
