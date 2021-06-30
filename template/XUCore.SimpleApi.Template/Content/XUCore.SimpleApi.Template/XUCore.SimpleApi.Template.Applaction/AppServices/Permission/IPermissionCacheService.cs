using System.Threading;
using System.Threading.Tasks;
using XUCore.SimpleApi.Template.Applaction;

namespace XUCore.SimpleApi.Template.Applaction.Permission
{
    public interface IPermissionCacheService : IAppService
    {
        Task<PermissionViewModel> GetAllAsync(CancellationToken cancellationToken);
    }
}