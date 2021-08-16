using System.Threading;
using System.Threading.Tasks;

namespace XUCore.Template.EasyLayer.DbService.Sys.Admin.Permission
{
    public interface IPermissionCacheService : IDbService
    {
        Task<PermissionViewModel> GetAllAsync(CancellationToken cancellationToken);
    }
}