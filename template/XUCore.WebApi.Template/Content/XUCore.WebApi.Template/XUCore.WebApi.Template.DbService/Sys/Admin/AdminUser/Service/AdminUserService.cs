using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.Paging;
using XUCore.WebApi.Template.Core;
using XUCore.WebApi.Template.Core.Enums;
using XUCore.WebApi.Template.DbService.Events;
using XUCore.WebApi.Template.Persistence;
using XUCore.WebApi.Template.Persistence.Entities.Sys.Admin;

namespace XUCore.WebApi.Template.DbService.Sys.Admin.AdminUser
{
    public class AdminUserService : IAdminUserService
    {
        private readonly INigelDbRepository db;
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public AdminUserService(INigelDbRepository db, IMapper mapper, IMediator mediator)
        {
            this.db = db;
            this.mapper = mapper;
            this.mediator = mediator;
        }

        [CacheRemove(Key = CacheKey.AuthTables)]
        public async Task<int> CreateAsync(AdminUserCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<AdminUserCreateCommand, AdminUserEntity>(request);

            //角色操作
            if (request.Roles != null && request.Roles.Length > 0)
            {
                //转换角色对象 并写入
                entity.UserRoles = Array.ConvertAll(request.Roles, roleid => new AdminUserRoleEntity
                {
                    RoleId = roleid,
                    UserId = entity.Id
                });
            }

            var res = await db.AddAsync(entity, cancellationToken: cancellationToken);

            if (res > 0)
            {
                await mediator.Publish(new AdminUserCreateEvent(entity.Id, entity), cancellationToken);

                return res;
            }
            else
                return res;
        }

        [CacheRemove(Key = CacheKey.AuthTables)]
        public async Task<int> UpdateAsync(AdminUserUpdateInfoCommand request, CancellationToken cancellationToken)
        {
            var entity = await db.Context.AdminUser.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (entity == null)
                return 0;

            entity = mapper.Map(request, entity);

            var res = db.Update(entity);

            if (res > 0)
            {
                return res;
            }
            return res;
        }

        [CacheRemove(Key = CacheKey.AuthTables)]
        public async Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken)
        {
            switch (field.ToLower())
            {
                case "name":
                    return await db.UpdateAsync<AdminUserEntity>(c => c.Id == id, c => new AdminUserEntity() { Name = value, Updated_At = DateTime.Now }, cancellationToken);
                case "username":
                    return await db.UpdateAsync<AdminUserEntity>(c => c.Id == id, c => new AdminUserEntity() { UserName = value, Updated_At = DateTime.Now }, cancellationToken);
                case "mobile":
                    return await db.UpdateAsync<AdminUserEntity>(c => c.Id == id, c => new AdminUserEntity() { Mobile = value, Updated_At = DateTime.Now }, cancellationToken);
                case "password":
                    return await db.UpdateAsync<AdminUserEntity>(c => c.Id == id, c => new AdminUserEntity() { Password = Encrypt.Md5By32(value), Updated_At = DateTime.Now }, cancellationToken);
                case "position":
                    return await db.UpdateAsync<AdminUserEntity>(c => c.Id == id, c => new AdminUserEntity() { Position = value, Updated_At = DateTime.Now }, cancellationToken);
                case "location":
                    return await db.UpdateAsync<AdminUserEntity>(c => c.Id == id, c => new AdminUserEntity() { Location = value, Updated_At = DateTime.Now }, cancellationToken);
                case "company":
                    return await db.UpdateAsync<AdminUserEntity>(c => c.Id == id, c => new AdminUserEntity() { Company = value, Updated_At = DateTime.Now }, cancellationToken);
                case "picture":
                    return await db.UpdateAsync<AdminUserEntity>(c => c.Id == id, c => new AdminUserEntity() { Picture = value, Updated_At = DateTime.Now }, cancellationToken);
                default:
                    return 0;
            }
        }

        [CacheRemove(Key = CacheKey.AuthTables)]
        public async Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken)
        {
            switch (status)
            {
                case Status.Show:
                    return await db.UpdateAsync<AdminUserEntity>(c => ids.Contains(c.Id), c => new AdminUserEntity { Status = Status.Show, Updated_At = DateTime.Now }, cancellationToken);
                case Status.SoldOut:
                    return await db.UpdateAsync<AdminUserEntity>(c => ids.Contains(c.Id), c => new AdminUserEntity { Status = Status.SoldOut, Updated_At = DateTime.Now }, cancellationToken);
                case Status.Trash:
                    return await db.UpdateAsync<AdminUserEntity>(c => ids.Contains(c.Id), c => new AdminUserEntity { Status = Status.Trash, Deleted_At = DateTime.Now }, cancellationToken);
                default:
                    return 0;
            }
        }

        public async Task<int> UpdateAsync(AdminUserUpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var admin = await db.Context.AdminUser.FindAsync(request.Id);

            request.NewPassword = Encrypt.Md5By32(request.NewPassword);
            request.OldPassword = Encrypt.Md5By32(request.OldPassword);

            if (!admin.Password.Equals(request.OldPassword))
                Failure.Error("旧密码错误");

            return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity { Password = request.NewPassword }, cancellationToken);
        }

        [CacheRemove(Key = CacheKey.AuthTables)]
        public async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await db.DeleteAsync<AdminUserEntity>(c => ids.Contains(c.Id), cancellationToken);

            if (res > 0)
            {
                //删除登录记录
                await db.DeleteAsync<LoginRecordEntity>(c => ids.Contains(c.AdminId), cancellationToken);
                //删除关联的角色
                await db.DeleteAsync<AdminUserRoleEntity>(c => ids.Contains(c.UserId), cancellationToken);
            }

            return res;
        }

        [CacheRemove(Key = CacheKey.AuthTables)]
        public async Task<int> RelevanceRoleAsync(AdminUserRelevanceRoleCommand request, CancellationToken cancellationToken)
        {
            //先清空用户的角色，确保没有冗余的数据
            await db.DeleteAsync<AdminUserRoleEntity>(c => c.UserId == request.AdminId, cancellationToken);

            var userRoles = Array.ConvertAll(request.RoleIds, roleid => new AdminUserRoleEntity
            {
                RoleId = roleid,
                UserId = request.AdminId
            });

            //添加角色
            if (userRoles.Length > 0)
                return await db.AddAsync(userRoles, cancellationToken: cancellationToken);

            return 1;
        }

        public async Task<AdminUserDto> LoginAsync(AdminUserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = default(AdminUserEntity);

            request.Password = Encrypt.Md5By32(request.Password);

            var loginWay = "";

            if (!Valid.IsMobileNumberSimple(request.Account))
            {
                user = await db.Context.AdminUser.Where(c => c.UserName.Equals(request.Account)).FirstOrDefaultAsync(cancellationToken);
                if (user == null)
                    Failure.Error("账号不存在");

                loginWay = "Mobile";
            }
            else
            {
                user = await db.Context.AdminUser.Where(c => c.Mobile.Equals(request.Account)).FirstOrDefaultAsync(cancellationToken);
                if (user == null)
                    Failure.Error("手机号码不存在");

                loginWay = "UserName";
            }

            if (!user.Password.Equals(request.Password))
                Failure.Error("密码错误");
            if (user.Status != Status.Show)
                Failure.Error("您的帐号禁止登录,请与管理员联系!");


            user.LoginCount += 1;
            user.LoginLastTime = DateTime.Now;
            user.LoginLastIp = Web.IP;

            user.LoginRecords.Add(new LoginRecordEntity
            {
                AdminId = user.Id,
                LoginIp = user.LoginLastIp,
                LoginTime = user.LoginLastTime,
                LoginWay = loginWay
            });

            db.Update(user);

            return mapper.Map<AdminUserDto>(user);
        }

        public async Task<AdminUserDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            var res = await db.Context.AdminUser
                .Where(c => c.Id == id)
                .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return res;
        }

        public async Task<bool> AnyByAccountAsync(AccountMode accountMode, string account, long notId, CancellationToken cancellationToken)
        {
            if (notId > 0)
            {
                switch (accountMode)
                {
                    case AccountMode.UserName:
                        return await db.Context.AdminUser.AnyAsync(c => c.Id != notId && c.UserName == account, cancellationToken);
                    case AccountMode.Mobile:
                        return await db.Context.AdminUser.AnyAsync(c => c.Id != notId && c.Mobile == account, cancellationToken);
                }
            }
            else
            {
                switch (accountMode)
                {
                    case AccountMode.UserName:
                        return await db.Context.AdminUser.AnyAsync(c => c.UserName == account, cancellationToken);
                    case AccountMode.Mobile:
                        return await db.Context.AdminUser.AnyAsync(c => c.Mobile == account, cancellationToken);
                }
            }

            return false;
        }

        public async Task<AdminUserDto> GetByAccountAsync(AccountMode accountMode, string account, CancellationToken cancellationToken)
        {
            switch (accountMode)
            {
                case AccountMode.UserName:

                    return await db.Context.AdminUser
                        .Where(c => c.UserName.Equals(account))
                        .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);

                case AccountMode.Mobile:

                    return await db.Context.AdminUser
                        .Where(c => c.Mobile.Equals(account))
                        .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);
            }

            return null;
        }

        public async Task<PagedModel<AdminUserDto>> GetPagedListAsync(AdminUserQueryPagedCommand request, CancellationToken cancellationToken)
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

            return res.ToModel();
        }

        public async Task<IList<long>> GetRoleKeysAsync(long adminId, CancellationToken cancellationToken)
        {
            return await db.Context.AdminAuthUserRole
                .Where(c => c.UserId == adminId)
                .Select(c => c.RoleId)
                .ToListAsync(cancellationToken);
        }
    }
}
