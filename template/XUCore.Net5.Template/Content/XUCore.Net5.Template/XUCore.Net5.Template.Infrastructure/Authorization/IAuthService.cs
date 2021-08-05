using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Net5.Template.Domain.Sys.AdminUser;

namespace XUCore.Net5.Template.Infrastructure.Authorization
{
    public interface IAuthService
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
