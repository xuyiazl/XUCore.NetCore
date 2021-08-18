﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.NetCore;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.Paging;
using XUCore.Template.Easy.Applaction.Permission;
using XUCore.Template.Easy.Core;
using XUCore.Template.Easy.Core.Enums;
using XUCore.Template.Easy.Persistence;
using XUCore.Template.Easy.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.Easy.Applaction.Admin
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class AdminRoleAppService : CurdAppService<long, AdminRoleEntity, AdminRoleDto, AdminRoleCreateCommand, AdminRoleUpdateCommand, AdminRoleQueryCommand, AdminRoleQueryPagedCommand>,
        IAdminRoleAppService
    {
        public AdminRoleAppService(IDefaultDbRepository db, IMapper mapper) : base(db, mapper)
        {

        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<Result<int>> CreateAsync([Required][FromBody] AdminRoleCreateCommand request, CancellationToken cancellationToken = default)
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

                return RestFull.Success(data: res);
            }
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<Result<int>> UpdateAsync([Required][FromBody] AdminRoleUpdateCommand request, CancellationToken cancellationToken = default)
        {
            var entity = await db.Context.AdminAuthRole.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (entity == null)
                return RestFull.Fail(data: 0);

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
                UpdatedAction?.Invoke(entity);

                return RestFull.Success(data: res);
            }
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新角色指定字段内容
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
                case "name":
                    res = await db.UpdateAsync<AdminRoleEntity>(c => c.Id == id, c => new AdminRoleEntity() { Name = value, UpdatedAt = DateTime.Now }, cancellationToken);
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
        /// 删除角色（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<Result<int>> DeleteAsync([Required] long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await db.DeleteAsync<AdminRoleEntity>(c => ids.Contains(c.Id));

            if (res > 0)
            {
                //删除关联的导航
                await db.DeleteAsync<AdminRoleMenuEntity>(c => ids.Contains(c.RoleId));
                //删除用户关联的角色
                await db.DeleteAsync<AdminUserRoleEntity>(c => ids.Contains(c.RoleId));

                DeletedAction?.Invoke(ids);

                return RestFull.Success(data: res);
            }
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<Result<IList<AdminRoleDto>>> GetListAsync([Required][FromQuery] AdminRoleQueryCommand request, CancellationToken cancellationToken = default)
        {
            var res = await db.Context.AdminAuthRole
                .WhereIf(c => c.Status == request.Status, request.Status != Status.Default)
                .WhereIf(c => c.Name.Contains(request.Keyword), request.Keyword.NotEmpty())
                .OrderByBatch(request.Orderby, request.Orderby.NotEmpty())
                .Take(request.Limit, request.Limit > 0)
                .ProjectTo<AdminRoleDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return RestFull.Success(data: res.As<IList<AdminRoleDto>>());
        }
        /// <summary>
        /// 获取角色分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<Result<PagedModel<AdminRoleDto>>> GetPageListAsync([Required][FromQuery] AdminRoleQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            var res = await db.Context.AdminAuthRole

                .WhereIf(c => c.Status == request.Status, request.Status != Status.Default)
                .WhereIf(c => c.Name.Contains(request.Keyword), request.Keyword.NotEmpty())

                .OrderByBatch(request.Orderby, request.Orderby.NotEmpty())

                .ProjectTo<AdminRoleDto>(mapper.ConfigurationProvider)
                .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

            return RestFull.Success(data: res.ToModel());
        }
        /// <summary>
        /// 获取角色关联的所有导航id集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<long>>> GetRelevanceMenuAsync([Required] int roleId, CancellationToken cancellationToken = default)
        {
            var res = await db.Context.AdminAuthRoleMenus
                          .Where(c => c.RoleId == roleId)
                          .OrderBy(c => c.MenuId)
                          .Select(c => c.MenuId)
                          .ToListAsync();

            return RestFull.Success(data: res.As<IList<long>>());
        }
    }
}
