using System.Threading;
using System.Threading.Tasks;

namespace WebApi.DbService.Sys.Admin.Permission
{
    public interface IPermissionCacheService : IDbService
    {
        Task<PermissionViewModel> GetAllAsync(CancellationToken cancellationToken);
    }
}