﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.EasyLayer.Persistence;
using XUCore.Template.EasyLayer.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.EasyLayer.DbService.Sys.Admin.Permission
{
    public class PermissionService : IPermissionService
    {
        private readonly IDefaultDbRepository db;
        private readonly IMapper mapper;

        private readonly IPermissionCacheService permissionCacheService;

        public PermissionService(IDefaultDbRepository db, IMapper mapper, IPermissionCacheService permissionCacheService)
        {
            this.db = db;
            this.mapper = mapper;
            this.permissionCacheService = permissionCacheService;
        }

        public async Task<bool> ExistsAsync(long adminId, string onlyCode, CancellationToken cancellationToken)
        {
            var menus = await permissionCacheService.GetAllAsync(adminId, cancellationToken);

            return menus.Any(c => c.OnlyCode == onlyCode);
        }

        public async Task<IList<PermissionMenuTreeDto>> GetMenusAsync(long adminId, CancellationToken cancellationToken)
        {
            var menus = await permissionCacheService.GetAllAsync(adminId, cancellationToken);

            var list = menus
                    .Where(c => c.IsMenu == true)
                    .OrderByDescending(c => c.Weight)
                    .ToList();

            return AuthMenuTree(list, 0);
        }

        private IList<PermissionMenuTreeDto> AuthMenuTree(IList<AdminMenuEntity> entities, long parentId)
        {
            IList<PermissionMenuTreeDto> menus = new List<PermissionMenuTreeDto>();

            entities.Where(c => c.FatherId == parentId).ToList().ForEach(entity =>
            {
                var dto = mapper.Map<AdminMenuEntity, PermissionMenuTreeDto>(entity);

                dto.Child = AuthMenuTree(entities, dto.Id);

                menus.Add(dto);
            });

            return menus;
        }

        public async Task<IList<PermissionMenuDto>> GetMenuExpressAsync(long adminId, CancellationToken cancellationToken)
        {
            var menus = await permissionCacheService.GetAllAsync(adminId, cancellationToken);

            var list = menus
                    .Where(c => c.IsMenu == true && c.IsExpress == true)
                    .OrderByDescending(c => c.Weight)
                    .ToList();

            var dto = mapper.Map<IList<AdminMenuEntity>, IList<PermissionMenuDto>>(list);

            return dto;
        }
    }
}