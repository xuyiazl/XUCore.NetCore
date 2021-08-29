using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Paging;
using XUCore.Template.EasyLayer.Core.Enums;
using XUCore.Template.EasyLayer.Persistence;
using XUCore.Template.EasyLayer.Persistence.Entities.Admin;

namespace XUCore.Template.EasyLayer.DbService.Admin.AdminRole
{
    public class AdminRoleService : CurdService<long, AdminRoleEntity, AdminRoleDto, AdminRoleCreateCommand, AdminRoleUpdateCommand, AdminRoleQueryCommand, AdminRoleQueryPagedCommand>,
        IAdminRoleService
    {
        private readonly IDefaultDbRepository<AdminUserRoleEntity> userRole;
        private readonly IDefaultDbRepository<AdminRoleMenuEntity> roleMenu;
        public AdminRoleService(IServiceProvider serviceProvider, IDefaultDbRepository<AdminRoleEntity> db, IMapper mapper) : base(db, mapper)
        {
            userRole = serviceProvider.GetService<IDefaultDbRepository<AdminUserRoleEntity>>();
            roleMenu = serviceProvider.GetService<IDefaultDbRepository<AdminRoleMenuEntity>>();
        }

        public override async Task<long> CreateAsync(AdminRoleCreateCommand request, CancellationToken cancellationToken)
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
                CreatedAction?.Invoke(entity);

                return entity.Id;
            }

            return 0;
        }

        public override async Task<int> UpdateAsync(AdminRoleUpdateCommand request, CancellationToken cancellationToken)
        {
            var entity = await db.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
                return 0;

            entity = mapper.Map(request, entity);

            //先清空导航集合，确保没有冗余信息
            await roleMenu.DeleteAsync(c => c.RoleId == entity.Id);

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
                    return await db.UpdateAsync(c => c.Id == id, c => new AdminRoleEntity() { Name = value, UpdatedAt = DateTime.Now }, cancellationToken);
                default:
                    return 0;
            }
        }

        public override async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await db.DeleteAsync(c => ids.Contains(c.Id));

            if (res > 0)
            {
                //删除关联的导航
                await roleMenu.DeleteAsync(c => ids.Contains(c.RoleId));
                //删除用户关联的角色
                await userRole.DeleteAsync(c => ids.Contains(c.RoleId));

                DeletedAction?.Invoke(ids);
            }

            return res;
        }

        public override async Task<IList<AdminRoleDto>> GetListAsync(AdminRoleQueryCommand request, CancellationToken cancellationToken)
        {
            var selector = db.BuildFilter()

                .And(c => c.Status == request.Status, request.Status != Status.Default)
                .And(c => c.Name.Contains(request.Keyword), request.Keyword.NotEmpty());

            var res = await db.GetListAsync<AdminRoleDto>(selector, $"{nameof(AdminRoleEntity.Id)} asc", limit: request.Limit, cancellationToken: cancellationToken);

            return res;
        }

        public async Task<IList<long>> GetRelevanceMenuAsync(int roleId, CancellationToken cancellationToken)
        {
            return await roleMenu.Table.Where(c => c.RoleId == roleId).OrderBy(c => c.MenuId).Select(c => c.MenuId).ToListAsync();
        }

        public override async Task<PagedModel<AdminRoleDto>> GetPagedListAsync(AdminRoleQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var selector = db.BuildFilter()

                .And(c => c.Status == request.Status, request.Status != Status.Default)
                .And(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty());

            var res = await db.GetPagedListAsync<AdminRoleDto>(selector, $"{nameof(AdminRoleEntity.Id)} asc", request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }
    }
}
