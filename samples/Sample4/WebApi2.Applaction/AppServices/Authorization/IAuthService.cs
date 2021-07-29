using System.Threading;
using System.Threading.Tasks;
using WebApi2.DbService.Sys.Admin.AdminUser;

namespace WebApi2.Applaction.Authorization
{
    public interface IAuthService : IAppService
    {
        long AdminId { get; }

        string AdminName { get; }

        bool IsAuthenticated { get; }

        bool IsCanAccess(string accessKey);

        Task<(string, string)> LoginAsync(AdminUserLoginCommand request, CancellationToken cancellationToken = default);

        Task LoginOutAsync();
    }
}
