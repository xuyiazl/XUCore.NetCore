using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.Net5.Template.Domain.Sys.AdminUser;
using XUCore.Net5.Template.Domain.Sys.Permission;
using XUCore.NetCore.Authorization.JwtBearer;

namespace XUCore.Net5.Template.Infrastructure.Authorization
{
    public class AdminManagerService : IAdminManager
    {
        private readonly IMediatorHandler bus;
        private const string userId = "_admin_userid_";
        private const string userName = "_admin_username_";
        public AdminManagerService(IMediatorHandler bus)
        {
            this.bus = bus;
        }

        public async Task<(string, string)> LoginAsync(AdminUserLoginCommand command)
        {
            var user = await bus.SendCommand(command);

            // 生成 token
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { userId , user.Id},
                { userName  ,user.UserName }
            });

            // 生成 刷新token
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);

            return (accessToken, refreshToken);
        }

        public async Task LoginOutAsync()
        {
            await Task.CompletedTask;
        }

        public bool IsCanAccess(string accessKey)
        {
            if (!IsAuthenticated)
                return false;
            if (!string.IsNullOrWhiteSpace(accessKey))
                return bus.SendCommand(new PermissionQueryExists { AdminId = AdminId, OnlyCode = accessKey }).Result;
            return true;
        }

        public bool IsAuthenticated => Identity.IsAuthenticated;

        private IIdentity Identity => Web.HttpContext.User.Identity;

        public long AdminId => Identity.GetValue<long>(userId);

        public string AdminName => Identity.GetValue<string>(userName);
    }
}
