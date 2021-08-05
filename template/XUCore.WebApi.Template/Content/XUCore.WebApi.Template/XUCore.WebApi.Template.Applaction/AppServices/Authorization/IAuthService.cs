﻿using System.Threading;
using System.Threading.Tasks;
using XUCore.WebApi.Template.DbService.Sys.Admin.AdminUser;

namespace XUCore.WebApi.Template.Applaction.Authorization
{
    public interface IAuthService : IAppService
    {
        long AdminId { get; }

        string AdminName { get; }

        bool IsAuthenticated { get; }

        Task<bool> IsCanAccessAsync(string accessKey);

        Task<(string, string)> LoginAsync(AdminUserLoginCommand request, CancellationToken cancellationToken = default);

        Task LoginOutAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 验证token是否一致
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool VaildLoginToken(string token);
    }
}
