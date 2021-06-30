using System.Threading;
using System.Threading.Tasks;
using XUCore.WebApi.Template.DbService.Sys.Admin.AdminUser;

namespace XUCore.WebApi.Template.Applaction.Authorization
{
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
