using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.NetCore;
using XUCore.Paging;
using XUCore.Template.Easy.Core;
using XUCore.Template.Easy.Core.Enums;
using XUCore.Template.Easy.Persistence;
using XUCore.Template.Easy.Persistence.Entities.Admin;

namespace XUCore.Template.Easy.Applaction.Admin
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class AdminMenuAppService : CurdAppService<long, AdminMenuEntity, AdminMenuDto, AdminMenuCreateCommand, AdminMenuUpdateCommand, AdminMenuQueryCommand, AdminMenuQueryPagedCommand>,
        IAdminMenuAppService
    {
        private readonly IDefaultDbRepository<AdminRoleMenuEntity> roleMenu;
        public AdminMenuAppService(IServiceProvider serviceProvider, IDefaultDbRepository<AdminMenuEntity> db, IMapper mapper) : base(db, mapper)
        {
            roleMenu = serviceProvider.GetService<IDefaultDbRepository<AdminRoleMenuEntity>>();
        }

        /// <summary>
        /// 删除导航（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        public override async Task<Result<int>> DeleteAsync([Required] long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await db.DeleteAsync(c => ids.Contains(c.Id), cancellationToken);

            if (res > 0)
            {
                await roleMenu.DeleteAsync(c => ids.Contains(c.MenuId), cancellationToken);

                DeletedAction?.Invoke(ids);

                return RestFull.Success(data: res);
            }
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新导航指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateFieldAsync([Required] long id, [Required] string field, string value, CancellationToken cancellationToken = default)
        {
            var res = 0;
            switch (field.ToLower())
            {
                case "icon":
                    res = await db.UpdateAsync(c => c.Id == id, c => new AdminMenuEntity() { Icon = value, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "url":
                    res = await db.UpdateAsync(c => c.Id == id, c => new AdminMenuEntity() { Url = value, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "onlycode":
                    res = await db.UpdateAsync(c => c.Id == id, c => new AdminMenuEntity() { OnlyCode = value, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "weight":
                    res = await db.UpdateAsync(c => c.Id == id, c => new AdminMenuEntity() { Weight = value.ToInt(), UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                default:
                    res = 0;
                    break;
            }

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<Result<IList<AdminMenuDto>>> GetListAsync([Required][FromQuery] AdminMenuQueryCommand request, CancellationToken cancellationToken = default)
        {
            var selector = db.BuildFilter()

                .And(c => c.IsMenu == request.IsMenu)
                .And(c => c.Status == request.Status, request.Status != Status.Default);

            var res = await db.GetListAsync<AdminMenuDto>(selector, $"{nameof(AdminMenuEntity.Id)} asc", limit: request.Limit, cancellationToken: cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取导航分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<Result<PagedModel<AdminMenuDto>>> GetPagedListAsync(AdminMenuQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var selector = db.BuildFilter()

                .And(c => c.Name.Contains(request.Keyword), request.Keyword.NotEmpty())
                .And(c => c.Status == request.Status, request.Status != Status.Default);

            var res = await db.GetPagedListAsync<AdminMenuDto>(selector, $"{nameof(AdminMenuEntity.Id)} asc", request.CurrentPage, request.PageSize, cancellationToken);

            return RestFull.Success(data: res.ToModel());
        }

        /// <summary>
        /// 获取导航树形结构
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Tree")]
        public async Task<Result<IList<AdminMenuTreeDto>>> GetListByTreeAsync(CancellationToken cancellationToken = default)
        {
            var list = await db.GetListAsync<AdminMenuEntity>(orderby: "Weight desc", cancellationToken: cancellationToken);

            var res = AuthMenuTree(list, 0);

            return RestFull.Success(data: res);
        }

        private IList<AdminMenuTreeDto> AuthMenuTree(IList<AdminMenuEntity> entities, long parentId)
        {
            var menus = new List<AdminMenuTreeDto>();

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
