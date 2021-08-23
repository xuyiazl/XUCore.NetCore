using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Paging;
using XUCore.Template.Layer.Core.Enums;
using XUCore.Template.Layer.Persistence;
using XUCore.Template.Layer.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.Layer.DbService.Sys.Admin.AdminRole
{
    public class AdminRoleService : CurdService<long, AdminRoleEntity, AdminRoleDto, AdminRoleCreateCommand, AdminRoleUpdateCommand, AdminRoleQueryCommand, AdminRoleQueryPagedCommand>,
        IAdminRoleService
    {
        public AdminRoleService(IDefaultDbRepository db, IMapper mapper) : base(db, mapper)
        {
        }

        public override async Task<int> CreateAsync(AdminRoleCreateCommand request, CancellationToken cancellationToken)
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
                CreatedAction?.Invoke(entity);

            return res;
        }

        public override async Task<int> UpdateAsync(AdminRoleUpdateCommand request, CancellationToken cancellationToken)
        {
            var entity = await db.GetByIdAsync<AdminRoleEntity>(request.Id, cancellationToken);

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
                UpdatedAction?.Invoke(entity);

            return res;
        }

        public async Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken)
        {
            switch (field.ToLower())
            {
                case "name":
                    return await db.UpdateAsync<AdminRoleEntity>(c => c.Id == id, c => new AdminRoleEntity() { Name = value, UpdatedAt = DateTime.Now }, cancellationToken);
                default:
                    return 0;
            }
        }

        public override async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await db.DeleteAsync<AdminRoleEntity>(c => ids.Contains(c.Id));

            if (res > 0)
            {
                //删除关联的导航
                await db.DeleteAsync<AdminRoleMenuEntity>(c => ids.Contains(c.RoleId));
                //删除用户关联的角色
                await db.DeleteAsync<AdminUserRoleEntity>(c => ids.Contains(c.RoleId));

                DeletedAction?.Invoke(ids);
            }

            return res;
        }

        public override async Task<IList<AdminRoleDto>> GetListAsync(AdminRoleQueryCommand request, CancellationToken cancellationToken)
        {
            var selector = db.AsQuery<AdminRoleEntity>()

                .And(c => c.Status == request.Status, request.Status != Status.Default)
                .And(c => c.Name.Contains(request.Keyword), request.Keyword.NotEmpty());

            var res = await db.GetListAsync<AdminRoleEntity, AdminRoleDto>(selector, $"{nameof(AdminRoleEntity.Id)} asc", limit: request.Limit, cancellationToken: cancellationToken);

            return res;
        }

        public async Task<IList<long>> GetRelevanceMenuAsync(int roleId, CancellationToken cancellationToken)
        {
            return await db.Context.AdminRoleMenu
                .Where(c => c.RoleId == roleId)
                .OrderBy(c => c.MenuId)
                .Select(c => c.MenuId)
                .ToListAsync();
        }

        public override async Task<PagedModel<AdminRoleDto>> GetPagedListAsync(AdminRoleQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var selector = db.AsQuery<AdminRoleEntity>()

                .And(c => c.Status == request.Status, request.Status != Status.Default)
                .And(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty());

            var res = await db.GetPagedListAsync<AdminRoleEntity, AdminRoleDto>(selector, $"{nameof(AdminRoleEntity.Id)} asc", request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }
    }
}
