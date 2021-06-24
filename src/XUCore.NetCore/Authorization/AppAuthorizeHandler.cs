﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.Authorization
{
    /// <summary>
    /// 授权策略执行程序
    /// </summary>
    public abstract class AppAuthorizeHandler : IAuthorizationHandler
    {
        /// <summary>
        /// 授权验证核心方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual async Task HandleAsync(AuthorizationHandlerContext context)
        {
            // 判断是否授权
            var isAuthenticated = context.User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                await AuthorizeHandleAsync(context);
            }
            else
            {
                var httpContext = context.GetCurrentHttpContext();
                // 退出Swagger登录
                if (httpContext != null)
                    httpContext.Response.Headers["access-token"] = "invalid_token";
            }
        }

        /// <summary>
        /// 验证管道
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public virtual Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 策略验证管道
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpContext"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        public virtual Task<bool> PolicyPipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext, IAuthorizationRequirement requirement)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 授权处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected async Task AuthorizeHandleAsync(AuthorizationHandlerContext context)
        {
            // 获取所有未成功验证的需求
            var pendingRequirements = context.PendingRequirements;

            // 获取 HttpContext 上下文
            var httpContext = context.GetCurrentHttpContext();

            // 调用子类管道
            var pipeline = await PipelineAsync(context, httpContext);
            if (pipeline)
            {
                // 通过授权验证
                foreach (var requirement in pendingRequirements)
                {
                    // 验证策略管道
                    var policyPipeline = await PolicyPipelineAsync(context, httpContext, requirement);
                    if (policyPipeline) context.Succeed(requirement);
                }
            }
            else context.Fail();
        }
    }
}
