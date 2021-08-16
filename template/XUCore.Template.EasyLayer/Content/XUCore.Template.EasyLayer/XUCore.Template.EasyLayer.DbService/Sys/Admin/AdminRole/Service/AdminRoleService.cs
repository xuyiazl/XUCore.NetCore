using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.Paging;
using XUCore.Template.EasyLayer.Core;
using XUCore.Template.EasyLayer.Core.Enums;
using XUCore.Template.EasyLayer.Persistence;
using XUCore.Template.EasyLayer.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.EasyLayer.DbService.Sys.Admin.AdminRole
{
    public class AdminRoleService : IAdminRoleService
    {
        private readonly IDefaultDbRepository db;
        private readonly IMapper mapper;

        public AdminRoleService(IDefaultDbRepository db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<int> CreateAsync(AdminRoleCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<AdminRoleCreateCommand, AdminRoleEntity>(request);

            //保存关联导航
            if (request.MenuIds != null && request.MenuIds.Length > 0)
            {
                entity.RoleMenus = Array.ConvertAll(request.MenuIds, key => new AdminRoleMenuEntity
                {
                    RoleId = entity.Id,
                    MenuId = key
                });
            }

            var res = await db.AddAsync(entity, cancellationToken: cancellationToken);

            if (res > 0)
            {
                return res;
            }
            else
                return res;
        }

        public async Task<int> UpdateAsync(AdminRoleUpdateCommand request, CancellationToken cancellationToken)
        {
            var entity = await db.Context.AdminAuthRole.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (entity == null)
                return 0;

            entity = mapper.Map(request, entity);

            //先清空导航集合，确保没有冗余信息
            await db.DeleteAsync<AdminRoleMenuEntity>(c => c.RoleId == entity.Id);

            //保存关联导航
            if (request.MenuIds != null && request.MenuIds.Length > 0)
            {
                entity.RoleMenus = Array.ConvertAll(request.MenuIds, key => new AdminRoleMenuEntity
                {
                    RoleId = entity.Id,
                    MenuId = key
                });
            }

            var res = db.Update(entity);

            if (res > 0)
            {
                return res;
            }
            return res;
        }

        public async Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken)
        {
            switch (field.ToLower())
            {
                case "name":
                    return await db.UpdateAsync<AdminRoleEntity>(c => c.Id == id, c => new AdminRoleEntity() { Name = value, Updated_At = DateTime.Now }, cancellationToken);
                default:
                    return 0;
            }
        }

        public async Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken)
        {
            switch (status)
            {
                case Status.Show:
                    return await db.UpdateAsync<AdminRoleEntity>(c => ids.Contains(c.Id), c => new AdminRoleEntity { Status = Status.Show, Updated_At = DateTime.Now }, cancellationToken);
                case Status.SoldOut:
                    return await db.UpdateAsync<AdminRoleEntity>(c => ids.Contains(c.Id), c => new AdminRoleEntity { Status = Status.SoldOut, Updated_At = DateTime.Now }, cancellationToken);
                case Status.Trash:
                    return await db.UpdateAsync<AdminRoleEntity>(c => ids.Contains(c.Id), c => new AdminRoleEntity { Status = Status.Trash, Deleted_At = DateTime.Now }, cancellationToken);
                default:
                    return 0;
            }
        }

        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await db.DeleteAsync<AdminRoleEntity>(c => ids.Contains(c.Id));

            if (res > 0)
            {
                //删除关联的导航
                await db.DeleteAsync<AdminRoleMenuEntity>(c => ids.Contains(c.RoleId));
                //删除用户关联的角色
                await db.DeleteAsync<AdminUserRoleEntity>(c => ids.Contains(c.RoleId));
            }

            return res;
        }

        public async Task<AdminRoleDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            var res = await db.Context.AdminAuthRole
                .Where(c => c.Id == id)
                .ProjectTo<AdminRoleDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return res;
        }

        public async Task<IList<AdminRoleDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var res = await db.Context.AdminAuthRole
                .Where(c => c.Status == Status.Show)
                .ProjectTo<AdminRoleDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return res;
        }

        public async Task<IList<long>> GetRelevanceMenuIdsAsync(int roleId, CancellationToken cancellationToken)
        {
            return await db.Context.AdminAuthRoleMenus
                .Where(c => c.RoleId == roleId)
                .OrderBy(c => c.MenuId)
                .Select(c => c.MenuId)
                .ToListAsync();
        }

        public async Task<PagedModel<AdminRoleDto>> GetPageListAsync(AdminRoleQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await db.Context.AdminAuthRole

                .WhereIf(c => c.Status == request.Status, request.Status != Status.Default)
                .WhereIf(c => c.Name.Contains(request.Search), !request.Search.IsEmpty())

                .OrderByBatch($"{request.Sort} {request.Order}", !request.Sort.IsEmpty() && !request.Order.IsEmpty())

                .ProjectTo<AdminRoleDto>(mapper.ConfigurationProvider)
                .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }
    }
}
