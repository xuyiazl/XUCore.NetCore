using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Template.FreeSql.Core;
using XUCore.Template.FreeSql.Persistence.Entities.User;

namespace XUCore.Template.FreeSql.DbService.Auth.Permission
{
    public class PermissionService : IPermissionService
    {
        private readonly IMapper mapper;

        private readonly IPermissionCacheService permissionCacheService;

        public PermissionService(IMapper mapper, IPermissionCacheService permissionCacheService)
        {
            this.mapper = mapper;
            this.permissionCacheService = permissionCacheService;
        }

        public async Task<bool> ExistsAsync(long userId, string onlyCode, CancellationToken cancellationToken)
        {
            var menus = await permissionCacheService.GetAllAsync(userId, cancellationToken);

            return menus.Any(c => c.OnlyCode == onlyCode);
        }

        public async Task<IList<PermissionMenuTreeDto>> GetMenusAsync(long userId, CancellationToken cancellationToken)
        {
            var menus = await permissionCacheService.GetAllAsync(userId, cancellationToken);

            var list = menus
                    .Where(c => c.IsMenu == true)
                    .OrderByDescending(c => c.Sort)
                    .ToList();

            var treeDtos = mapper.Map<IList<MenuEntity>, IList<PermissionMenuTreeDto>>(list);

            var tree = treeDtos.ToTree(
                rootWhere: (r, c) => c.ParentId == 0,
                childsWhere: (r, c) => r.Id == c.ParentId,
                addChilds: (r, datalist) =>
                {
                    r.Child ??= new List<PermissionMenuTreeDto>();
                    r.Child.AddRange(datalist);
                });

            return tree;
        }

        public async Task<IList<PermissionMenuDto>> GetMenuExpressAsync(long userId, CancellationToken cancellationToken)
        {
            var menus = await permissionCacheService.GetAllAsync(userId, cancellationToken);

            var list = menus
                    .Where(c => c.IsMenu == true && c.IsExpress == true)
                    .OrderByDescending(c => c.Sort)
                    .ToList();

            var dto = mapper.Map<IList<MenuEntity>, IList<PermissionMenuDto>>(list);

            return dto;
        }
    }
}
