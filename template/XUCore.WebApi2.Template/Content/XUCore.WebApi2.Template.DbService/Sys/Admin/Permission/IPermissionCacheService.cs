using System.Threading;
using System.Threading.Tasks;

namespace XUCore.WebApi2.Template.DbService.Sys.Admin.Permission
{
    public interface IPermissionCacheService : IDbService
    {
        Task<PermissionViewModel> GetAllAsync(CancellationToken cancellationToken);
    }
}