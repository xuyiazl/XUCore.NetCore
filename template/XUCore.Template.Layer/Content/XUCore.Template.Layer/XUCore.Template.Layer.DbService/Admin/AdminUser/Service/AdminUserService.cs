using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.Paging;
using XUCore.Template.Layer.Core.Enums;
using XUCore.Template.Layer.DbService.Events;
using XUCore.Template.Layer.Persistence;
using XUCore.Template.Layer.Persistence.Entities.Admin;

namespace XUCore.Template.Layer.DbService.Admin.AdminUser
{
    public class AdminUserService : CurdService<long, AdminUserEntity, AdminUserDto, AdminUserCreateCommand, AdminUserUpdateInfoCommand, AdminUserQueryCommand, AdminUserQueryPagedCommand>,
        IAdminUserService
    {
        private readonly IDefaultDbRepository<AdminUserRoleEntity> userRole;
        private readonly IDefaultDbRepository<AdminUserLoginRecordEntity> userLoginRecord;
        public AdminUserService(IServiceProvider serviceProvider, IDefaultDbRepository<AdminUserEntity> db, IMapper mapper, IMediator mediator) : base(db, mapper)
        {
            userRole = serviceProvider.GetService<IDefaultDbRepository<AdminUserRoleEntity>>();
            userLoginRecord = serviceProvider.GetService<IDefaultDbRepository<AdminUserLoginRecordEntity>>();

            CreatedAction = async (entity) =>
            {
                await mediator.Publish(new AdminUserCreateEvent(entity.Id, entity));
            };
        }

        public override async Task<long> CreateAsync(AdminUserCreateCommand request, CancellationToken cancellationToken)
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
                CreatedAction?.Invoke(entity);

                return entity.Id;
            }
            else
                return 0;
        }

        public async Task<int> UpdateAsync(AdminUserUpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var admin = await db.GetByIdAsync(request.Id, cancellationToken);

            request.NewPassword = Encrypt.Md5By32(request.NewPassword);
            request.OldPassword = Encrypt.Md5By32(request.OldPassword);

            if (!admin.Password.Equals(request.OldPassword))
                Failure.Error("旧密码错误");

            return await db.UpdateAsync(c => c.Id == request.Id, c => new AdminUserEntity { Password = request.NewPassword }, cancellationToken);
        }

        public async Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken)
        {
            switch (field.ToLower())
            {
                case "name":
                    return await db.UpdateAsync(c => c.Id == id, c => new AdminUserEntity() { Name = value, UpdatedAt = DateTime.Now }, cancellationToken);
                case "position":
                    return await db.UpdateAsync(c => c.Id == id, c => new AdminUserEntity() { Position = value, UpdatedAt = DateTime.Now }, cancellationToken);
                case "location":
                    return await db.UpdateAsync(c => c.Id == id, c => new AdminUserEntity() { Location = value, UpdatedAt = DateTime.Now }, cancellationToken);
                case "company":
                    return await db.UpdateAsync(c => c.Id == id, c => new AdminUserEntity() { Company = value, UpdatedAt = DateTime.Now }, cancellationToken);
                case "picture":
                    return await db.UpdateAsync(c => c.Id == id, c => new AdminUserEntity() { Picture = value, UpdatedAt = DateTime.Now }, cancellationToken);
                default:
                    return 0;
            }
        }

        public override async Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken)
        {
            var res = await db.DeleteAsync(c => ids.Contains(c.Id), cancellationToken);

            if (res > 0)
            {
                //删除登录记录
                await userLoginRecord.DeleteAsync(c => ids.Contains(c.AdminId), cancellationToken);
                //删除关联的角色
                await userRole.DeleteAsync(c => ids.Contains(c.AdminId), cancellationToken);

                DeletedAction?.Invoke(ids);
            }

            return res;
        }

        public async Task<AdminUserDto> LoginAsync(AdminUserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = default(AdminUserEntity);

            request.Password = Encrypt.Md5By32(request.Password);

            var loginWay = "";

            if (!Valid.IsMobileNumberSimple(request.Account))
            {
                user = await db.GetFirstAsync(c => c.UserName.Equals(request.Account), cancellationToken: cancellationToken);
                if (user == null)
                    Failure.Error("账号不存在");

                loginWay = "UserName";
            }
            else
            {
                user = await db.GetFirstAsync(c => c.Mobile.Equals(request.Account), cancellationToken: cancellationToken);
                if (user == null)
                    Failure.Error("手机号码不存在");

                loginWay = "Mobile";
            }

            if (!user.Password.Equals(request.Password))
                Failure.Error("密码错误");
            if (user.Status != Status.Show)
                Failure.Error("您的帐号禁止登录,请与管理员联系!");


            user.LoginCount += 1;
            user.LoginLastTime = DateTime.Now;
            user.LoginLastIp = Web.IP;

            user.LoginRecords.Add(new AdminUserLoginRecordEntity
            {
                AdminId = user.Id,
                LoginIp = user.LoginLastIp,
                LoginTime = user.LoginLastTime,
                LoginWay = loginWay
            });

            db.Update(user);

            return mapper.Map<AdminUserDto>(user);
        }

        public async Task<bool> AnyByAccountAsync(AccountMode accountMode, string account, long notId, CancellationToken cancellationToken)
        {
            if (notId > 0)
            {
                switch (accountMode)
                {
                    case AccountMode.UserName:
                        return await db.AnyAsync(c => c.Id != notId && c.UserName == account, cancellationToken);
                    case AccountMode.Mobile:
                        return await db.AnyAsync(c => c.Id != notId && c.Mobile == account, cancellationToken);
                }
            }
            else
            {
                switch (accountMode)
                {
                    case AccountMode.UserName:
                        return await db.AnyAsync(c => c.UserName == account, cancellationToken);
                    case AccountMode.Mobile:
                        return await db.AnyAsync(c => c.Mobile == account, cancellationToken);
                }
            }

            return false;
        }

        public async Task<AdminUserDto> GetByAccountAsync(AccountMode accountMode, string account, CancellationToken cancellationToken)
        {
            switch (accountMode)
            {
                case AccountMode.UserName:
                    return await db.GetFirstAsync<AdminUserDto>(c => c.UserName.Equals(account), cancellationToken: cancellationToken);

                case AccountMode.Mobile:
                    return await db.GetFirstAsync<AdminUserDto>(c => c.Mobile.Equals(account), cancellationToken: cancellationToken);
            }

            return null;
        }

        public override async Task<IList<AdminUserDto>> GetListAsync(AdminUserQueryCommand request, CancellationToken cancellationToken)
        {
            var selector = db.BuildFilter()

                .And(c => c.Status == request.Status, request.Status != Status.Default)
                .And(c =>
                            c.Name.Contains(request.Keyword) ||
                            c.Mobile.Contains(request.Keyword) ||
                            c.UserName.Contains(request.Keyword), !request.Keyword.IsEmpty());

            var res = await db.GetListAsync<AdminUserDto>(selector: selector, orderby: $"{nameof(AdminUserEntity.Id)} asc", limit: request.Limit, cancellationToken: cancellationToken);

            return res;
        }

        public override async Task<PagedModel<AdminUserDto>> GetPagedListAsync(AdminUserQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var selector = db.BuildFilter()

                .And(c => c.Status == request.Status, request.Status != Status.Default)
                .And(c =>
                            c.Name.Contains(request.Keyword) ||
                            c.Mobile.Contains(request.Keyword) ||
                            c.UserName.Contains(request.Keyword), !request.Keyword.IsEmpty());

            var res = await db.GetPagedListAsync<AdminUserDto>(selector, $"{nameof(AdminUserEntity.Id)} asc", request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }

        public async Task<int> CreateRelevanceRoleAsync(AdminUserRelevanceRoleCommand request, CancellationToken cancellationToken)
        {
            //先清空用户的角色，确保没有冗余的数据
            await userRole.DeleteAsync(c => c.AdminId == request.AdminId, cancellationToken);

            var userRoles = Array.ConvertAll(request.RoleIds, roleid => new AdminUserRoleEntity
            {
                RoleId = roleid,
                AdminId = request.AdminId
            });

            //添加角色
            if (userRoles.Length > 0)
                return await userRole.AddAsync(userRoles, cancellationToken: cancellationToken);

            return 1;
        }

        public async Task<IList<long>> GetRoleKeysAsync(long adminId, CancellationToken cancellationToken)
        {
            return await userRole.Table.Where(c => c.AdminId == adminId).Select(c => c.RoleId).ToListAsync(cancellationToken);
        }

        public async Task<IList<AdminUserLoginRecordDto>> GetRecordListAsync(AdminUserLoginRecordQueryCommand request, CancellationToken cancellationToken)
        {
            var res = await View.Create(db.Context)

                .Where(c => c.AdminId == request.AdminId)

                .OrderByDescending(c => c.LoginTime)
                .Take(request.Limit)

                .ProjectTo<AdminUserLoginRecordDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return res;
        }

        public async Task<PagedModel<AdminUserLoginRecordDto>> GetRecordPagedListAsync(AdminUserLoginRecordQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await View.Create(db.Context)

                .WhereIf(c => c.Name.Contains(request.Keyword) || c.Mobile.Contains(request.Keyword) || c.UserName.Contains(request.Keyword), request.Keyword.NotEmpty())

                .OrderByBatch($"{nameof(AdminUserLoginRecordEntity.Id)} asc")

                .ProjectTo<AdminUserLoginRecordDto>(mapper.ConfigurationProvider)
                .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }
    }
}
