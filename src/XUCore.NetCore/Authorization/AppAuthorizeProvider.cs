﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.Authorization
{
    /// <summary>
    /// 授权策略提供器
    /// </summary>
    internal sealed class AppAuthorizeProvider : IAuthorizationPolicyProvider
    {
        /// <summary>
        /// 默认回退策略
        /// </summary>
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public AppAuthorizeProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        /// <summary>
        /// 获取默认策略
        /// </summary>
        /// <returns></returns>
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return FallbackPolicyProvider.GetDefaultPolicyAsync();
        }

        /// <summary>
        /// 获取回退策略
        /// </summary>
        /// <returns></returns>
        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return FallbackPolicyProvider.GetFallbackPolicyAsync();
        }

        /// <summary>
        /// 获取策略
        /// </summary>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // 判断是否是包含授权策略前缀
            if (policyName.StartsWith(Penetrates.AppAuthorizePrefix))
            {
                // 解析策略名并获取策略参数
                var policies = policyName[Penetrates.AppAuthorizePrefix.Length..].Split(',', StringSplitOptions.RemoveEmptyEntries);

                // 添加策略需求
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new AppAuthorizeRequirement(policies));

                return Task.FromResult(policy.Build());
            }

            // 如果策略不匹配，则返回回退策略
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
