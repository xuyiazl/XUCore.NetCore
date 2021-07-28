using System.Threading;
using System.Threading.Tasks;
using XUCore.WebApi2.Template.DbService.Sys.Admin.AdminUser;

namespace XUCore.WebApi2.Template.Applaction.Authorization
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
