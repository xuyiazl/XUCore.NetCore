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

namespace XUCore.Template.FreeSql.DbService.Auth.Role
{
    public class RoleService : FreeSqlCurdService<long, RoleEntity, RoleDto, RoleCreateCommand, RoleUpdateCommand, RoleQueryCommand, RoleQueryPagedCommand>,
        IRoleService
    {
        public RoleService(IServiceProvider serviceProvider, FreeSqlUnitOfWorkManager muowm, IMapper mapper, IUserInfo user) : base(muowm, mapper, user)
        {

        }

        public override async Task<long> CreateAsync(RoleCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<RoleCreateCommand, RoleEntity>(request);

            //保存关联导航
            if (request.MenuIds != null && request.MenuIds.Length > 0)
            {
                entity.RoleMenus = Array.ConvertAll(request.MenuIds, key => new RoleMenuEntity
                {
                    RoleId = entity.Id,
                    MenuId = key
                });
            }

            var res = await repo.InsertAsync(entity, cancellationToken);

            if (res != null)
            {
                await freeSql.Insert<RoleMenuEntity>(entity.RoleMenus).ExecuteAffrowsAsync();

                CreatedAction?.Invoke(entity);

                return res.Id;
            }

            return 0;
        }

        public override async Task<int> UpdateAsync(RoleUpdateCommand request, CancellationToken cancellationToken)
        {
            var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync<RoleEntity>(cancellationToken);

            if (entity == null)
                return 0;

            //先清空导航集合，确保没有冗余信息
            await freeSql.Delete<RoleMenuEntity>().Where(c => c.RoleId == request.Id).ExecuteAffrowsAsync(cancellationToken);

            //保存关联导航
            if (request.MenuIds != null && request.MenuIds.Length > 0)
            {
                entity.RoleMenus = Array.ConvertAll(request.MenuIds, key => new RoleMenuEntity
                {
                    RoleId = entity.Id,
                    MenuId = key
                });
            }

            var res = await repo.UpdateAsync(entity, cancellationToken);

            if (res > 0)
            {
                await freeSql.Insert<RoleMenuEntity>(entity.RoleMenus).ExecuteAffrowsAsync();

                UpdatedAction?.Invoke(entity);
            }
            return res;
        }

        public async Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken)
        {
            switch (field.ToLower())
            {
                case "name":
                    return await freeSql.Update<RoleEntity>(id).Set(c => new RoleEntity() { Name = value, ModifiedAtUserId = User.GetId<long>(), ModifiedAtUserName = User.UserName }).ExecuteAffrowsAsync(cancellationToken);
                case "sort":
                    return await freeSql.Update<RoleEntity>(id).Set(c => new RoleEntity() { Sort = value.ToInt(), ModifiedAtUserId = User.GetId<long>(), ModifiedAtUserName = User.UserName }).ExecuteAffrowsAsync(cancellationToken);
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
                .Update<RoleEntity>(ids)
                .Set(c => new RoleEntity()
                {
                    Enabled = enabled,
                    ModifiedAtUserId = User.GetId<long>(),
                    ModifiedAtUserName = User.UserName
                })
                .ExecuteAffrowsAsync(cancellationToken);
        }

        public override async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await freeSql.Delete<RoleEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
            {
                //删除关联的导航
                await freeSql.Delete<RoleMenuEntity>().Where(c => ids.Contains(c.RoleId)).ExecuteAffrowsAsync(cancellationToken);
                //删除用户关联的角色
                await freeSql.Delete<UserRoleEntity>().Where(c => ids.Contains(c.RoleId)).ExecuteAffrowsAsync(cancellationToken);

                DeletedAction?.Invoke(ids);
            }

            return res;
        }

        public override async Task<IList<RoleDto>> GetListAsync(RoleQueryCommand request, CancellationToken cancellationToken)
        {
            var select = repo.Select
                   .Where(c => c.Enabled == request.Enabled)
                   .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
                   .OrderBy(c => c.Id);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync<RoleDto>(cancellationToken);

            return res;
        }

        public async Task<IList<long>> GetRelevanceMenuAsync(int roleId, CancellationToken cancellationToken)
        {
            return await freeSql.Select<RoleMenuEntity>().Where(c => c.RoleId == roleId).OrderBy(c => c.MenuId).ToListAsync(c => c.MenuId, cancellationToken);
        }

        public override async Task<PagedModel<RoleDto>> GetPagedListAsync(RoleQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await repo.Select
                   .Where(c => c.Enabled == request.Enabled)
                   .WhereIf(request.Keyword.NotEmpty(), c => c.Name.Contains(request.Keyword))
                   .OrderBy(c => c.Id)
                   .ToPagedListAsync<RoleEntity, RoleDto>(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }
    }
}
