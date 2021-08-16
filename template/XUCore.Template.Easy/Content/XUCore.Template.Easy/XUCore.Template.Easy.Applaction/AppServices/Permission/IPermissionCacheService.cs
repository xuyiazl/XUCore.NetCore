using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.Easy.Applaction;

namespace XUCore.Template.Easy.Applaction.Permission
{
    public interface IPermissionCacheService : IAppService
    {
        Task<PermissionViewModel> GetAllAsync(CancellationToken cancellationToken);
    }
}