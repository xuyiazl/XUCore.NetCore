using System.Threading;
using System.Threading.Tasks;

namespace Sample.Plain.DbService.Sys.Admin.Permission
{
    public interface IPermissionCacheService : IDbService
    {
        Task<PermissionViewModel> GetAllAsync(CancellationToken cancellationToken);
    }
}