using AutoMapper;
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
using XUCore.SimpleApi.Template.Core;
using XUCore.SimpleApi.Template.Core.Enums;
using XUCore.SimpleApi.Template.Persistence;
using XUCore.SimpleApi.Template.Persistence.Entities.Sys.Admin;

namespace XUCore.SimpleApi.Template.Applaction.Admin
{
    /// <summary>
    /// 管理员
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class AdminAppService : AppService, IAdminAppService
    {
        private readonly IMediator mediator;

        private readonly IDefaultDbRepository db;
        private readonly IMapper mapper;

        public AdminAppService(IServiceProvider serviceProvider)
        {
            this.mediator = serviceProvider.GetService<IMediator>();
            this.db = serviceProvider.GetService<IDefaultDbRepository>();
            this.mapper = serviceProvider.GetService<IMapper>();
        }

        #region [ 账号管理 ]

        /// <summary>
        /// 创建管理员账号
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<int>> CreateUserAsync([Required][FromBody] AdminUserCreateCommand request, CancellationToken cancellationToken = default)
        {
            var entity = mapper.Map<AdminUserCreateCommand, AdminUserEntity>(request);

            //角色操作
            if (request.Roles != null && request.Roles.Length > 0)
            {
                //转换角色对象 并写入
                entity.UserRoles = Array.ConvertAll(request.Roles, roleid => new AdminUserRoleEntity
                {
                    RoleId = roleid,
                    AdminId = entity.Id
                });
            }

            var res = await db.AddAsync(entity, cancellationToken: cancellationToken);

            if (res > 0)
            {
                await mediator.Publish(new AdminUserCreateEvent(entity.Id, entity), cancellationToken);

                return RestFull.Success(data: res);
            }
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新账号信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result<int>> UpdateUserAsync([Required][FromBody] AdminUserUpdateInfoCommand request, CancellationToken cancellationToken = default)
        {
            var entity = await db.Context.AdminUser.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (entity == null)
                return RestFull.Fail(data: 0);

            entity = mapper.Map(request, entity);

            var res = db.Update(entity);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Password")]
        public async Task<Result<int>> UpdateUserAsync([Required][FromBody] AdminUserUpdatePasswordCommand request, CancellationToken cancellationToken = default)
        {
            var admin = await db.Context.AdminUser.FindAsync(request.Id);

            request.NewPassword = Encrypt.Md5By32(request.NewPassword);
            request.OldPassword = Encrypt.Md5By32(request.OldPassword);

            if (!admin.Password.Equals(request.OldPassword))
                Failure.Error("旧密码错误");

            var res = await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity { Password = request.NewPassword }, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("/api/[controller]/User/Field")]
        public async Task<Result<int>> UpdateUserFieldAsync([Required] long id, [Required] string field, string value, CancellationToken cancellationToken = default)
        {
            var res = 0;
            switch (field.ToLower())
            {
                case "name":
                    res = await db.UpdateAsync<AdminUserEntity>(c => c.Id == id, c => new AdminUserEntity() { Name = value, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "username":
                    res = await db.UpdateAsync<AdminUserEntity>(c => c.Id == id, c => new AdminUserEntity() { UserName = value, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "mobile":
                    res = await db.UpdateAsync<AdminUserEntity>(c => c.Id == id, c => new AdminUserEntity() { Mobile = value, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "password":
                    res = await db.UpdateAsync<AdminUserEntity>(c => c.Id == id, c => new AdminUserEntity() { Password = Encrypt.Md5By32(value), UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "position":
                    res = await db.UpdateAsync<AdminUserEntity>(c => c.Id == id, c => new AdminUserEntity() { Position = value, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "location":
                    res = await db.UpdateAsync<AdminUserEntity>(c => c.Id == id, c => new AdminUserEntity() { Location = value, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "company":
                    res = await db.UpdateAsync<AdminUserEntity>(c => c.Id == id, c => new AdminUserEntity() { Company = value, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "picture":
                    res = await db.UpdateAsync<AdminUserEntity>(c => c.Id == id, c => new AdminUserEntity() { Picture = value, UpdatedAt = DateTime.Now }, cancellationToken);
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
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("/api/[controller]/User/Status")]
        public async Task<Result<int>> UpdateUserStatusAsync([Required] long[] ids, [Required] Status status, CancellationToken cancellationToken = default)
        {
            var res = 0;
            switch (status)
            {
                case Status.Show:
                    res = await db.UpdateAsync<AdminUserEntity>(c => ids.Contains(c.Id), c => new AdminUserEntity { Status = Status.Show, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case Status.SoldOut:
                    res = await db.UpdateAsync<AdminUserEntity>(c => ids.Contains(c.Id), c => new AdminUserEntity { Status = Status.SoldOut, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case Status.Trash:
                    res = await db.UpdateAsync<AdminUserEntity>(c => ids.Contains(c.Id), c => new AdminUserEntity { Status = Status.Trash, DeletedAt = DateTime.Now }, cancellationToken);
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
        /// 删除账号（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Result<int>> DeleteUserAsync([Required] long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await db.DeleteAsync<AdminUserEntity>(c => ids.Contains(c.Id), cancellationToken);

            if (res > 0)
            {
                //删除登录记录
                await db.DeleteAsync<LoginRecordEntity>(c => ids.Contains(c.AdminId), cancellationToken);
                //删除关联的角色
                await db.DeleteAsync<AdminUserRoleEntity>(c => ids.Contains(c.AdminId), cancellationToken);

                return RestFull.Success(data: res);
            }
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/User/{id}")]
        public async Task<Result<AdminUserDto>> GetUserByIdAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            var res = await db.Context.AdminUser
                .Where(c => c.Id == id)
                .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取账号信息（根据账号或手机号码）
        /// </summary>
        /// <param name="accountMode"></param>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/User/Account")]
        public async Task<Result<AdminUserDto>> GetUserByAccountAsync([Required] AccountMode accountMode, [Required] string account, CancellationToken cancellationToken = default)
        {
            switch (accountMode)
            {
                case AccountMode.UserName:

                    var res = await db.Context.AdminUser
                        .Where(c => c.UserName.Equals(account))
                        .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);

                    return RestFull.Success(data: res);

                case AccountMode.Mobile:

                    res = await db.Context.AdminUser
                        .Where(c => c.Mobile.Equals(account))
                        .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);

                    return RestFull.Success(data: res);
            }

            return RestFull.Fail(data: default(AdminUserDto));
        }
        /// <summary>
        /// 检查账号或者手机号是否存在
        /// </summary>
        /// <param name="accountMode"></param>
        /// <param name="account"></param>
        /// <param name="notId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/User/Any")]
        public async Task<Result<bool>> AnyUserAsync([Required] AccountMode accountMode, [Required] string account, [Required] long notId, CancellationToken cancellationToken = default)
        {
            var res = false;

            if (notId > 0)
            {
                switch (accountMode)
                {
                    case AccountMode.UserName:
                        res = await db.Context.AdminUser.AnyAsync(c => c.Id != notId && c.UserName == account, cancellationToken);
                        break;
                    case AccountMode.Mobile:
                        res = await db.Context.AdminUser.AnyAsync(c => c.Id != notId && c.Mobile == account, cancellationToken);
                        break;
                }
            }
            else
            {
                switch (accountMode)
                {
                    case AccountMode.UserName:
                        res = await db.Context.AdminUser.AnyAsync(c => c.UserName == account, cancellationToken);
                        break;
                    case AccountMode.Mobile:
                        res = await db.Context.AdminUser.AnyAsync(c => c.Mobile == account, cancellationToken);
                        break;
                }
            }

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取账号分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/User/Page")]
        public async Task<Result<PagedModel<AdminUserDto>>> GetUserPageAsync([Required][FromQuery] AdminUserQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            var res = await db.Context.AdminUser

                   .WhereIf(c => c.Status == request.Status, request.Status != Status.Default)
                   .WhereIf(c =>
                               c.Name.Contains(request.Search) ||
                               c.Mobile.Contains(request.Search) ||
                               c.UserName.Contains(request.Search), !request.Search.IsEmpty())

                   .OrderByBatch($"{request.Sort} {request.Order}", !request.Sort.IsEmpty() && !request.Order.IsEmpty())

                   .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                   .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

            return RestFull.Success(data: res.ToModel());
        }

        #endregion

        #region [ 账号&角色 关联操作 ]

        /// <summary>
        /// 账号关联角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/api/[controller]/User/RelevancRole")]
        public async Task<Result<int>> CreateUserRelevanceRoleAsync([Required][FromBody] AdminUserRelevanceRoleCommand request, CancellationToken cancellationToken = default)
        {
            //先清空用户的角色，确保没有冗余的数据
            await db.DeleteAsync<AdminUserRoleEntity>(c => c.AdminId == request.AdminId, cancellationToken);

            var userRoles = Array.ConvertAll(request.RoleIds, roleid => new AdminUserRoleEntity
            {
                RoleId = roleid,
                AdminId = request.AdminId
            });

            //添加角色
            if (userRoles.Length > 0)
            {
                var res = await db.AddAsync(userRoles, cancellationToken: cancellationToken);
            }

            return RestFull.Success(data: 1);
        }
        /// <summary>
        /// 获取账号关联的角色id集合
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/User/RelevancRole/{adminId}")]
        public async Task<Result<IList<long>>> GetUserRelevanceRoleAsync([Required] long adminId, CancellationToken cancellationToken = default)
        {
            var res = await db.Context.AdminAuthUserRole
                .Where(c => c.AdminId == adminId)
                .Select(c => c.RoleId)
                .ToListAsync(cancellationToken);

            return RestFull.Success(data: res.As<IList<long>>());
        }

        #endregion

        #region [ 角色管理 ]

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<int>> CreateRoleAsync([Required][FromBody] AdminRoleCreateCommand request, CancellationToken cancellationToken = default)
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
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result<int>> UpdateRoleAsync([Required][FromBody] AdminRoleUpdateCommand request, CancellationToken cancellationToken = default)
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
                return RestFull.Success(data: res);
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
        [HttpPut("/api/[controller]/Role/Field")]
        public async Task<Result<int>> UpdateRoleFieldAsync([Required] long id, [Required] string field, string value, CancellationToken cancellationToken = default)
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
        /// 更新角色状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("/api/[controller]/Role/Status")]
        public async Task<Result<int>> UpdateRoleStatusAsync([Required] long[] ids, Status status, CancellationToken cancellationToken = default)
        {
            var res = 0;

            switch (status)
            {
                case Status.Show:
                    res = await db.UpdateAsync<AdminRoleEntity>(c => ids.Contains(c.Id), c => new AdminRoleEntity { Status = Status.Show, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case Status.SoldOut:
                    res = await db.UpdateAsync<AdminRoleEntity>(c => ids.Contains(c.Id), c => new AdminRoleEntity { Status = Status.SoldOut, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case Status.Trash:
                    res = await db.UpdateAsync<AdminRoleEntity>(c => ids.Contains(c.Id), c => new AdminRoleEntity { Status = Status.Trash, DeletedAt = DateTime.Now }, cancellationToken);
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
        [HttpDelete]
        public async Task<Result<int>> DeleteRoleAsync([Required] long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await db.DeleteAsync<AdminRoleEntity>(c => ids.Contains(c.Id));

            if (res > 0)
            {
                //删除关联的导航
                await db.DeleteAsync<AdminRoleMenuEntity>(c => ids.Contains(c.RoleId));
                //删除用户关联的角色
                await db.DeleteAsync<AdminUserRoleEntity>(c => ids.Contains(c.RoleId));

                return RestFull.Success(data: res);
            }
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Role/{id}")]
        public async Task<Result<AdminRoleDto>> GetRoleByIdAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            var res = await db.Context.AdminAuthRole
                .Where(c => c.Id == id)
                .ProjectTo<AdminRoleDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Role/All")]
        public async Task<Result<IList<AdminRoleDto>>> GetRoleAllAsync(CancellationToken cancellationToken = default)
        {
            var res = await db.Context.AdminAuthRole
                .Where(c => c.Status == Status.Show)
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
        [HttpGet("/api/[controller]/Role/Page")]
        public async Task<Result<PagedModel<AdminRoleDto>>> GetRolePageAsync([Required][FromQuery] AdminRoleQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            var res = await db.Context.AdminAuthRole

                .WhereIf(c => c.Status == request.Status, request.Status != Status.Default)
                .WhereIf(c => c.Name.Contains(request.Search), !request.Search.IsEmpty())

                .OrderByBatch($"{request.Sort} {request.Order}", !request.Sort.IsEmpty() && !request.Order.IsEmpty())

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
        [HttpGet("/api/[controller]/Role/RelevanceMenu/{roleId}")]
        public async Task<Result<IList<long>>> GetRoleRelevanceMenuAsync([Required] int roleId, CancellationToken cancellationToken = default)
        {
            var res = await db.Context.AdminAuthRoleMenus
                          .Where(c => c.RoleId == roleId)
                          .OrderBy(c => c.MenuId)
                          .Select(c => c.MenuId)
                          .ToListAsync();

            return RestFull.Success(data: res.As<IList<long>>());
        }

        #endregion

        #region [ 权限导航操作 ]

        /// <summary>
        /// 创建导航
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<int>> CreateMenuAsync([Required][FromBody] AdminMenuCreateCommand request, CancellationToken cancellationToken = default)
        {
            var entity = mapper.Map<AdminMenuCreateCommand, AdminMenuEntity>(request);

            var res = await db.AddAsync(entity, cancellationToken: cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新导航信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result<int>> UpdateMenuAsync([Required][FromBody] AdminMenuUpdateCommand request, CancellationToken cancellationToken = default)
        {
            var entity = await db.Context.AdminAuthMenus.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (entity == null)
                return RestFull.Fail(data: 0);

            entity = mapper.Map(request, entity);

            var res = db.Update(entity);

            if (res > 0)
                return RestFull.Success(data: res);
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
        [HttpPut("/api/[controller]/Menu/Field")]
        public async Task<Result<int>> UpdateMenuFieldAsync([Required] long id, [Required] string field, string value, CancellationToken cancellationToken = default)
        {
            var res = 0;
            switch (field.ToLower())
            {
                case "icon":
                    res = await db.UpdateAsync<AdminMenuEntity>(c => c.Id == id, c => new AdminMenuEntity() { Icon = value, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "url":
                    res = await db.UpdateAsync<AdminMenuEntity>(c => c.Id == id, c => new AdminMenuEntity() { Url = value, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "onlycode":
                    res = await db.UpdateAsync<AdminMenuEntity>(c => c.Id == id, c => new AdminMenuEntity() { OnlyCode = value, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "weight":
                    res = await db.UpdateAsync<AdminMenuEntity>(c => c.Id == id, c => new AdminMenuEntity() { Weight = value.ToInt(), UpdatedAt = DateTime.Now }, cancellationToken);
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
        /// 更新导航状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("/api/[controller]/Menu/Status")]
        public async Task<Result<int>> UpdateMenuStatusAsync([Required] long[] ids, [Required] Status status, CancellationToken cancellationToken = default)
        {
            var res = 0;
            switch (status)
            {
                case Status.Show:
                    res = await db.UpdateAsync<AdminMenuEntity>(c => ids.Contains(c.Id), c => new AdminMenuEntity { Status = Status.Show, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case Status.SoldOut:
                    res = await db.UpdateAsync<AdminMenuEntity>(c => ids.Contains(c.Id), c => new AdminMenuEntity { Status = Status.SoldOut, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case Status.Trash:
                    res = await db.UpdateAsync<AdminMenuEntity>(c => ids.Contains(c.Id), c => new AdminMenuEntity { Status = Status.Trash, DeletedAt = DateTime.Now }, cancellationToken);
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
        /// 删除导航（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Result<int>> DeleteMenuAsync([Required] long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await db.DeleteAsync<AdminMenuEntity>(c => ids.Contains(c.Id), cancellationToken);

            if (res > 0)
            {
                await db.DeleteAsync<AdminRoleMenuEntity>(c => ids.Contains(c.MenuId), cancellationToken);

                return RestFull.Success(data: res);
            }
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取导航信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Menu/{id}")]
        public async Task<Result<AdminMenuDto>> GetMenuByIdAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            var res = await db.Context.AdminAuthMenus
                  .Where(c => c.Id == id)
                  .ProjectTo<AdminMenuDto>(mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取导航树形结构
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Menu/Tree")]
        public async Task<Result<IList<AdminMenuTreeDto>>> GetMenuTreeAsync(CancellationToken cancellationToken = default)
        {
            var list = await db.Context.AdminAuthMenus
                 .OrderByDescending(c => c.Weight)
                 .ToListAsync(cancellationToken);

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
        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <param name="isMenu"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Menu/List")]
        public async Task<Result<IList<AdminMenuDto>>> GetMenuListAsync([Required] bool isMenu = true, CancellationToken cancellationToken = default)
        {
            var res = await db.Context.AdminAuthMenus
                .Where(c => c.IsMenu == isMenu)
                .OrderByDescending(c => c.Weight)
                .ProjectTo<AdminMenuDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return RestFull.Success(data: res.As<IList<AdminMenuDto>>());
        }

        #endregion
    }
}
