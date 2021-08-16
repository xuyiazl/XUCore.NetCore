﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.Template.Layer.Core;
using XUCore.Template.Layer.Core.Enums;
using XUCore.Template.Layer.Persistence;
using XUCore.Template.Layer.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.Layer.DbService.Sys.Admin.AdminMenu
{
    public class AdminMenuService : IAdminMenuService
    {
        private readonly INigelDbRepository db;
        private readonly IMapper mapper;

        public AdminMenuService(INigelDbRepository db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        [CacheRemove(Key = CacheKey.AuthTables)]
        public async Task<int> CreateAsync(AdminMenuCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<AdminMenuCreateCommand, AdminMenuEntity>(request);

            var res = await db.AddAsync(entity, cancellationToken: cancellationToken);

            if (res > 0)
            {
                return res;
            }
            else
                return res;
        }

        [CacheRemove(Key = CacheKey.AuthTables)]
        public async Task<int> UpdateAsync(AdminMenuUpdateCommand request, CancellationToken cancellationToken)
        {
            var entity = await db.Context.AdminAuthMenus.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (entity == null)
                return 0;

            entity = mapper.Map(request, entity);

            var res = db.Update(entity);

            if (res > 0)
            {
                return res;
            }
            return res;
        }

        [CacheRemove(Key = CacheKey.AuthTables)]
        public async Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken)
        {
            switch (field.ToLower())
            {
                case "icon":
                    return await db.UpdateAsync<AdminMenuEntity>(c => c.Id == id, c => new AdminMenuEntity() { Icon = value, Updated_At = DateTime.Now }, cancellationToken);
                case "url":
                    return await db.UpdateAsync<AdminMenuEntity>(c => c.Id == id, c => new AdminMenuEntity() { Url = value, Updated_At = DateTime.Now }, cancellationToken);
                case "onlycode":
                    return await db.UpdateAsync<AdminMenuEntity>(c => c.Id == id, c => new AdminMenuEntity() { OnlyCode = value, Updated_At = DateTime.Now }, cancellationToken);
                case "weight":
                    return await db.UpdateAsync<AdminMenuEntity>(c => c.Id == id, c => new AdminMenuEntity() { Weight = value.ToInt(), Updated_At = DateTime.Now }, cancellationToken);
                default:
                    return 0;
            }
        }

        [CacheRemove(Key = CacheKey.AuthTables)]
        public async Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken)
        {
            switch (status)
            {
                case Status.Show:
                    return await db.UpdateAsync<AdminMenuEntity>(c => ids.Contains(c.Id), c => new AdminMenuEntity { Status = Status.Show, Updated_At = DateTime.Now }, cancellationToken);
                case Status.SoldOut:
                    return await db.UpdateAsync<AdminMenuEntity>(c => ids.Contains(c.Id), c => new AdminMenuEntity { Status = Status.SoldOut, Updated_At = DateTime.Now }, cancellationToken);
                case Status.Trash:
                    return await db.UpdateAsync<AdminMenuEntity>(c => ids.Contains(c.Id), c => new AdminMenuEntity { Status = Status.Trash, Deleted_At = DateTime.Now }, cancellationToken);
                default:
                    return 0;
            }
        }

        [CacheRemove(Key = CacheKey.AuthTables)]
        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await db.DeleteAsync<AdminMenuEntity>(c => ids.Contains(c.Id), cancellationToken);

            if (res > 0)
            {
                await db.DeleteAsync<AdminRoleMenuEntity>(c => ids.Contains(c.MenuId), cancellationToken);
            }

            return res;
        }

        public async Task<AdminMenuDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            var res = await db.Context.AdminAuthMenus
                .Where(c => c.Id == id)
                .ProjectTo<AdminMenuDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return res;
        }

        public async Task<IList<AdminMenuDto>> GetListByWeightAsync(bool isMenu, CancellationToken cancellationToken)
        {
            var res = await db.Context.AdminAuthMenus
                .Where(c => c.IsMenu == isMenu)
                .OrderByDescending(c => c.Weight)
                .ProjectTo<AdminMenuDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return res;
        }

        public async Task<IList<AdminMenuTreeDto>> GetListByTreeAsync(CancellationToken cancellationToken)
        {
            var res = await db.Context.AdminAuthMenus
                .OrderByDescending(c => c.Weight)
                .ToListAsync(cancellationToken);

            return AuthMenuTree(res, 0);
        }

        private IList<AdminMenuTreeDto> AuthMenuTree(IList<AdminMenuEntity> entities, long parentId)
        {
            IList<AdminMenuTreeDto> menus = new List<AdminMenuTreeDto>();

            entities.Where(c => c.FatherId == parentId).ToList().ForEach(entity =>
            {
                var dto = mapper.Map<AdminMenuEntity, AdminMenuTreeDto>(entity);

                dto.Child = AuthMenuTree(entities, dto.Id);

                menus.Add(dto);
            });

            return menus;
        }
    }
}
