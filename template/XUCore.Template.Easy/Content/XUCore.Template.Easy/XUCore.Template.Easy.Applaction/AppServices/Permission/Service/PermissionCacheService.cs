using AutoMapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.DynamicWebApi;
using XUCore.Template.Easy.Core;
using XUCore.Template.Easy.Core.Enums;
using XUCore.Template.Easy.Persistence;

namespace XUCore.Template.Easy.Applaction.Permission
{
    [NonDynamicWebApi]
    public class PermissionCacheService : IPermissionCacheService
    {
        private readonly INigelDbRepository db;
        private readonly IMapper mapper;

        public PermissionCacheService(INigelDbRepository db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        [CacheMethod(Key = CacheKey.AuthTables, Seconds = CacheTime.Day1)]
        public async Task<PermissionViewModel> GetAllAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            return new PermissionViewModel
            {
                Menus = db.Context.AdminAuthMenus.Where(c => c.Status == Status.Show).ToList(),
                RoleMenus = db.Context.AdminAuthRoleMenus.ToList(),
                UserRoles = db.Context.AdminAuthUserRole.ToList()
            };
        }
    }
}
