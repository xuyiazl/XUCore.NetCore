using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.NetCore.FreeSql;
using XUCore.NetCore.FreeSql.Curd;
using XUCore.Paging;
using XUCore.Template.FreeSql.Core;
using XUCore.Template.FreeSql.Persistence.Entities.User;

namespace XUCore.Template.FreeSql.DbService.Auth.Menu
{
    public class MenuService : FreeSqlCurdService<long, MenuEntity, MenuDto, MenuCreateCommand, MenuUpdateCommand, MenuQueryCommand, MenuQueryPagedCommand>,
        IMenuService
    {
        public MenuService(IServiceProvider serviceProvider, FreeSqlUnitOfWorkManager muowm, IMapper mapper, IUserInfo user) : base(muowm, mapper, user)
        {

        }

        public async Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken)
        {
            switch (field.ToLower())
            {
                case "name":
                    return await freeSql.Update<MenuEntity>(id).Set(c => new MenuEntity() { Name = value, ModifiedAtUserId = User.GetId<long>(), ModifiedAtUserName = User.UserName }).ExecuteAffrowsAsync(cancellationToken);
                case "icon":
                    return await freeSql.Update<MenuEntity>(id).Set(c => new MenuEntity() { Icon = value, ModifiedAtUserId = User.GetId<long>(), ModifiedAtUserName = User.UserName }).ExecuteAffrowsAsync(cancellationToken);
                case "url":
                    return await freeSql.Update<MenuEntity>(id).Set(c => new MenuEntity() { Url = value, ModifiedAtUserId = User.GetId<long>(), ModifiedAtUserName = User.UserName }).ExecuteAffrowsAsync(cancellationToken);
                case "onlycode":
                    return await freeSql.Update<MenuEntity>(id).Set(c => new MenuEntity() { OnlyCode = value, ModifiedAtUserId = User.GetId<long>(), ModifiedAtUserName = User.UserName }).ExecuteAffrowsAsync(cancellationToken);
                case "sort":
                    return await freeSql.Update<MenuEntity>(id).Set(c => new MenuEntity() { Sort = value.ToInt(), ModifiedAtUserId = User.GetId<long>(), ModifiedAtUserName = User.UserName }).ExecuteAffrowsAsync(cancellationToken);
            }
            return 0;
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
                    ModifiedAtUserId = User.GetId<long>(),
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
            //var res = await repo.Select.OrderByDescending(c => c.Sort).OrderBy(c => c.CreatedAt).ToListAsync<MenuTreeDto>(cancellationToken);

            //var tree = res.ToTree(
            //    rootWhere: (r, c) => c.ParentId == 0,
            //    childsWhere: (r, c) => r.Id == c.ParentId,
            //    addChilds: (r, datalist) =>
            //    {
            //        r.Childs ??= new List<MenuTreeDto>();
            //        r.Childs.AddRange(datalist);
            //    });

            var res = repo.Select.OrderByDescending(c => c.Sort).OrderBy(c => c.CreatedAt).OrderBy(c => c.CreatedAt).ToTreeList();

            var tree = mapper.Map<IList<MenuEntity>, IList<MenuTreeDto>>(res);

            return tree;
        }
    }
}
