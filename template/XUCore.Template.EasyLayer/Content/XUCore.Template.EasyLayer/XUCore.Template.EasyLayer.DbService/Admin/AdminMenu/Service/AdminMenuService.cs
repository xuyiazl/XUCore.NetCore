using AutoMapper;
using AutoMapper.QueryableExtensions;
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

namespace XUCore.Template.EasyLayer.DbService.Admin.AdminMenu
{
    public class AdminMenuService : CurdService<long, AdminMenuEntity, AdminMenuDto, AdminMenuCreateCommand, AdminMenuUpdateCommand, AdminMenuQueryCommand, AdminMenuQueryPagedCommand>,
        IAdminMenuService
    {
        private readonly IDefaultDbRepository<AdminRoleMenuEntity> roleMenu;
        public AdminMenuService(IServiceProvider serviceProvider, IDefaultDbRepository<AdminMenuEntity> db, IMapper mapper) : base(db, mapper)
        {
            roleMenu = serviceProvider.GetService<IDefaultDbRepository<AdminRoleMenuEntity>>();
        }

        public async Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken)
        {
            switch (field.ToLower())
            {
                case "icon":
                    return await db.UpdateAsync(c => c.Id == id, c => new AdminMenuEntity() { Icon = value, UpdatedAt = DateTime.Now }, cancellationToken);
                case "url":
                    return await db.UpdateAsync(c => c.Id == id, c => new AdminMenuEntity() { Url = value, UpdatedAt = DateTime.Now }, cancellationToken);
                case "onlycode":
                    return await db.UpdateAsync(c => c.Id == id, c => new AdminMenuEntity() { OnlyCode = value, UpdatedAt = DateTime.Now }, cancellationToken);
                case "weight":
                    return await db.UpdateAsync(c => c.Id == id, c => new AdminMenuEntity() { Weight = value.ToInt(), UpdatedAt = DateTime.Now }, cancellationToken);
                default:
                    return 0;
            }
        }

        public override async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await db.DeleteAsync(c => ids.Contains(c.Id), cancellationToken);

            if (res > 0)
            {
                await roleMenu.DeleteAsync(c => ids.Contains(c.MenuId), cancellationToken);

                DeletedAction?.Invoke(ids);
            }

            return res;
        }

        public override async Task<IList<AdminMenuDto>> GetListAsync(AdminMenuQueryCommand request, CancellationToken cancellationToken)
        {
            var selector = db.BuildFilter()

                .And(c => c.IsMenu == request.IsMenu)
                .And(c => c.Status == request.Status, request.Status != Status.Default);

            var res = await db.GetListAsync<AdminMenuDto>(selector, $"{nameof(AdminMenuEntity.Id)} asc", limit: request.Limit, cancellationToken: cancellationToken);

            return res;
        }

        public override async Task<PagedModel<AdminMenuDto>> GetPagedListAsync(AdminMenuQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var selector = db.BuildFilter()

                .And(c => c.Name.Contains(request.Keyword), request.Keyword.NotEmpty())
                .And(c => c.Status == request.Status, request.Status != Status.Default);

            var res = await db.GetPagedListAsync<AdminMenuDto>(selector, $"{nameof(AdminMenuEntity.Id)} asc", request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }

        public async Task<IList<AdminMenuTreeDto>> GetListByTreeAsync(CancellationToken cancellationToken)
        {
            var res = await db.GetListAsync(orderby: "Weight desc", cancellationToken: cancellationToken);

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
