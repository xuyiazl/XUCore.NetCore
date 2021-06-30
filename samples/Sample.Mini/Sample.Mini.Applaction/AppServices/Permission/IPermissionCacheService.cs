using System.Threading;
using System.Threading.Tasks;
using Sample.Mini.Applaction;

namespace Sample.Mini.Applaction.Permission
{
    public interface IPermissionCacheService : IAppService
    {
        Task<PermissionViewModel> GetAllAsync(CancellationToken cancellationToken);
    }
}