using System.Threading;
using System.Threading.Tasks;
using Simple.Applaction;

namespace Simple.Applaction.Permission
{
    public interface IPermissionCacheService : IAppService
    {
        Task<PermissionViewModel> GetAllAsync(CancellationToken cancellationToken);
    }
}