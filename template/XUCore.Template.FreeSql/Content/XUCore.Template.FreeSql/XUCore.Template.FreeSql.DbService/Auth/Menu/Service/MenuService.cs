using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.NetCore.FreeSql;
using XUCore.Paging;
using XUCore.Template.FreeSql.Core;
using XUCore.Template.FreeSql.Core.Auth;
using XUCore.Template.FreeSql.Persistence;
using XUCore.Template.FreeSql.Persistence.Entities.Sys.User;

namespace XUCore.Template.FreeSql.DbService.Auth.Menu
{
    public class MenuService : FreeSqlCurdService<MenuEntity, MenuDto, MenuCreateCommand, MenuUpdateCommand, MenuQueryCommand, MenuQueryPagedCommand>,
        IMenuService
    {
        public MenuService(IServiceProvider serviceProvider, FreeSqlUnitOfWorkManager muowm, IMapper mapper, IUser user) : base(muowm, mapper, user)
        {

        }

        public async Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken)
        {
            var entity = new MenuEntity();

            switch (field.ToLower())
            {
                case "icon":
                    entity = new MenuEntity() { Icon = value, ModifiedAtUserId = User.Id, ModifiedAtUserName = User.UserName };
                    break;
                case "url":
                    entity = new MenuEntity() { Url = value, ModifiedAtUserId = User.Id, ModifiedAtUserName = User.UserName };
                    break;
                case "onlycode":
                    entity = new MenuEntity() { OnlyCode = value, ModifiedAtUserId = User.Id, ModifiedAtUserName = User.UserName };
                    break;
                case "sort":
                    entity = new MenuEntity() { Sort = value.ToInt(), ModifiedAtUserId = User.Id, ModifiedAtUserName = User.UserName };
                    break;
            }

            return await freeSql.Update<MenuEntity>(id).Set(c => entity).ExecuteAffrowsAsync(cancellationToken);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="enabled"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(long[] ids, bool enabled, CancellationToken cancellationToken)
        {
            return await freeSql
                .Update<MenuEntity>(ids)
                .Set(c => new MenuEntity()
                {
                    Enabled = enabled,
                    ModifiedAtUserId = User.Id,
                    ModifiedAtUserName = User.UserName
                })
                .ExecuteAffrowsAsync(cancellationToken);
        }

        public override async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await freeSql.Delete<MenuEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
            {
                await freeSql.Delete<RoleMenuEntity>().Where(c => ids.Contains(c.MenuId)).ExecuteAffrowsAsync(cancellationToken);

                DeletedAction?.Invoke(ids);
            }

            return res;
        }

        public override async Task<IList<MenuDto>> GetListAsync(MenuQueryCommand request, CancellationToken cancellationToken)
        {
            var select = repo.Select
                .Where(c => c.IsMenu == request.IsMenu)
                .Where(c => c.Enabled == request.Enabled)
                .OrderBy(c => c.Id);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync<MenuDto>(cancellationToken);

            return res;
        }

        public override async Task<PagedModel<MenuDto>> GetPagedListAsync(MenuQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await repo.Select
                .Where(c => c.Enabled == request.Enabled)
                .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
                .OrderBy(c => c.Id)
                .ToPagedListAsync<MenuEntity, MenuDto>(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }

        public async Task<IList<MenuTreeDto>> GetListByTreeAsync(CancellationToken cancellationToken)
        {
            var res = await repo.Select.OrderByDescending(c => c.Sort).OrderBy(c => c.CreatedAt).ToListAsync<MenuTreeDto>(cancellationToken);

            var tree = res.ToTree(
                rootWhere: (r, c) => c.ParentId == 0,
                childsWhere: (r, c) => r.Id == c.ParentId,
                addChilds: (r, datalist) =>
                {
                    r.Child ??= new List<MenuTreeDto>();
                    r.Child.AddRange(datalist);
                });

            return tree;
        }
    }
}
