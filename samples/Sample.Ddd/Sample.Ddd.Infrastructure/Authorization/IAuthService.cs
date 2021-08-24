using System.Threading;
using System.Threading.Tasks;
using Sample.Ddd.Domain.Core;
using Sample.Ddd.Domain.User.User;

namespace Sample.Ddd.Infrastructure.Authorization
{
    public interface IAuthService: ILoginInfoService
    {
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
