using System.Threading;
using System.Threading.Tasks;
using Sample.Mini.Applaction.Login;

namespace Sample.Mini.Applaction.Authorization
{
    /// <summary>
    /// 身份认证
    /// </summary>
    public interface IAuthService : IAppService
    {
        long AdminId { get; }

        string AdminName { get; }

        bool IsAuthenticated { get; }

        bool IsCanAccess(string accessKey);

        Task<(string, string)> LoginAsync(AdminUserLoginCommand command, CancellationToken cancellationToken = default);

        Task LoginOutAsync();
    }
}
