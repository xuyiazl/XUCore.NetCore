using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.Ddd.Domain.User.User;

namespace XUCore.Template.Ddd.Infrastructure.Authorization
{
    public interface IAuthService
    {
        string UserId { get; }

        string UserName { get; }

        bool IsAuthenticated { get; }

        Task<bool> IsCanAccessAsync(string accessKey);

        Task<(string, string)> LoginAsync(UserLoginCommand request, CancellationToken cancellationToken = default);

        Task LoginOutAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 验证token是否一致
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool VaildLoginToken(string token);
    }
}
