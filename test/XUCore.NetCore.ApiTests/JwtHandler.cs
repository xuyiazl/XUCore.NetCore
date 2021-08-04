using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using XUCore.Helpers;
using XUCore.NetCore.Authorization;
using XUCore.NetCore.Authorization.JwtBearer;

namespace XUCore.NetCore.ApiTests
{
    /// <summary>
    /// JWT 授权自定义处理程序
    /// </summary>
    public class JwtHandler : AppAuthorizeHandler
    {
        /// <summary>
        /// 重写 Handler 添加自动刷新收取逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            string url = context.GetCurrentHttpContext().Request.GetRefererUrlAddress();
            if (url.Contains("xx.com")) //if (url.Contains("localhost"))
            {
                var isAuthenticated = context.User.Identity.IsAuthenticated;
                var pendingRequirements = context.PendingRequirements;
                foreach (var requirement in pendingRequirements)
                {
                    // 授权成功
                    context.Succeed(requirement);
                }
            }
            else
            {
                // 自动刷新 token
                if (JWTEncryption.AutoRefreshToken(context, context.GetCurrentHttpContext()))
                    await AuthorizeHandleAsync(context);
                else
                    context.Fail();    // 授权失败
            }
        }

        /// <summary>
        /// 验证管道
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public override Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
        {
            // 检查权限，如果方法时异步的就不用 Task.FromResult 包裹，直接使用 async/await 即可
            return Task.FromResult(true);
        }
    }
}
