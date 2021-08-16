using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.Easy.Applaction.Login;

namespace XUCore.Template.Easy.Applaction.Authorization
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
