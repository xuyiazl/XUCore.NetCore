using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Cache;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.NetCore.Authorization.JwtBearer;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Swagger;
using XUCore.Template.Easy.Applaction.Admin;
using XUCore.Template.Easy.Applaction.Login;
using XUCore.Template.Easy.Applaction.Permission;
using XUCore.Template.Easy.Core.Enums;
using XUCore.Template.Easy.Persistence;
using XUCore.Template.Easy.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.Easy.Applaction.Authorization
{
    /// <summary>
    /// 身份认证
    /// </summary>
    [NonDynamicWebApi]
    public class AuthService : IAuthService
    {
        private const string userId = "_admin_userid_";
        private const string userName = "_admin_username_";
        private const string loginToken = "__login_token__";
        private readonly ICacheManager cacheManager;

        private readonly IPermissionService permissionService;
        private readonly IDefaultDbRepository db;
        private readonly IMapper mapper;
        public AuthService(IServiceProvider serviceProvider)
        {
            permissionService = serviceProvider.GetService<IPermissionService>();
            this.db = serviceProvider.GetService<IDefaultDbRepository>();
            this.mapper = serviceProvider.GetService<IMapper>();
            this.cacheManager = serviceProvider.GetService<ICacheManager>();
        }

        public async Task<(string, string)> LoginAsync(AdminUserLoginCommand request, CancellationToken cancellationToken = default)
        {
            var user = default(AdminUserEntity);

            request.Password = Encrypt.Md5By32(request.Password);

            var loginWay = "";

            if (!Valid.IsMobileNumberSimple(request.Account))
            {
                user = await db.GetFirstAsync<AdminUserEntity>(c => c.UserName.Equals(request.Account), cancellationToken: cancellationToken);
                if (user == null)
                    Failure.Error("账号不存在");

                loginWay = "UserName";
            }
            else
            {
                user = await db.GetFirstAsync<AdminUserEntity>(c => c.Mobile.Equals(request.Account), cancellationToken: cancellationToken);
                if (user == null)
                    Failure.Error("手机号码不存在");

                loginWay = "Mobile";
            }

            if (!user.Password.Equals(request.Password))
                Failure.Error("密码错误");
            if (user.Status != Status.Show)
                Failure.Error("您的帐号禁止登录,请与管理员联系!");


            user.LoginCount += 1;
            user.LoginLastTime = DateTime.Now;
            user.LoginLastIp = Web.IP;

            user.LoginRecords.Add(new LoginRecordEntity
            {
                AdminId = user.Id,
                LoginIp = user.LoginLastIp,
                LoginTime = user.LoginLastTime,
                LoginWay = loginWay
            });

            db.Update(user);

            // 生成 token
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { userId , user.Id},
                { userName  ,user.UserName }
            });

            // 生成 刷新token
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);

            // 设置 Swagger 自动登录
            Web.HttpContext.SigninToSwagger(accessToken);
            // 设置刷新 token
            Web.HttpContext.Response.Headers["x-access-token"] = refreshToken;

            SetLoginToken(user.Id, accessToken);

            return (accessToken, refreshToken);
        }

        public async Task LoginOutAsync(CancellationToken cancellationToken = default)
        {
            RemoveLoginToken();

            await Task.CompletedTask;
        }

        /// <summary>
        /// 将登录的用户写入内存作为标记，处理强制重新获取jwt，模拟退出登录（可以使用redis）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        private void SetLoginToken(long userId, string token)
        {
            cacheManager.Set($"{loginToken}{userId}", token);
        }
        /// <summary>
        /// 删除登录标记，模拟退出
        /// </summary>
        private void RemoveLoginToken()
        {
            cacheManager.Remove($"{loginToken}{AdminId}");
        }
        /// <summary>
        /// 验证token是否一致
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool VaildLoginToken(string token)
        {
            var cacheToken = cacheManager.Get<string>($"{loginToken}{AdminId}");

            return token == cacheToken;
        }

        public bool IsCanAccess(string accessKey)
        {
            if (!accessKey.IsEmpty())
                return permissionService.ExistsAsync(AdminId, accessKey, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
            return true;
        }

        public bool IsAuthenticated => Identity.IsAuthenticated;

        private IIdentity Identity => Web.HttpContext.User.Identity;

        public long AdminId => Identity.GetValue<long>(userId);

        public string AdminName => Identity.GetValue<string>(userName);
    }
}
