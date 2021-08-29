using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
using XUCore.Paging;
using XUCore.Template.Easy.Applaction.Login;
using XUCore.Template.Easy.Core;
using XUCore.Template.Easy.Core.Enums;
using XUCore.Template.Easy.Persistence;
using XUCore.Template.Easy.Persistence.Entities.Admin;

namespace XUCore.Template.Easy.Applaction.Admin
{
    /// <summary>
    /// 管理员
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class AdminUserAppService : CurdAppService<long, AdminUserEntity, AdminUserDto, AdminUserCreateCommand, AdminUserUpdateInfoCommand, AdminUserQueryCommand, AdminUserQueryPagedCommand>
        , IAdminUserAppService
    {
        private readonly IMediator mediator;
        private readonly IDefaultDbRepository<AdminUserRoleEntity> userRole;
        private readonly IDefaultDbRepository<AdminUserLoginRecordEntity> userLoginRecord;
        public AdminUserAppService(IServiceProvider serviceProvider, IDefaultDbRepository<AdminUserEntity> db, IMapper mapper) : base(db, mapper)
        {
            userRole = serviceProvider.GetService<IDefaultDbRepository<AdminUserRoleEntity>>();
            userLoginRecord = serviceProvider.GetService<IDefaultDbRepository<AdminUserLoginRecordEntity>>();

            CreatedAction = async (entity) =>
            {
                await mediator.Publish(new AdminUserCreateEvent(entity.Id, entity));
            };
        }

        #region [ 账号管理 ]

        /// <summary>
        /// 创建初始账号
        /// </summary>
        /// <remarks>
        /// 初始账号密码：
        ///     <para>username : admin</para>
        ///     <para>password : admin</para>
        /// </remarks>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<Result<int>> CreateInitAccountAsync(CancellationToken cancellationToken = default)
        {
            var command = new AdminUserCreateCommand
            {
                UserName = "admin",
                Password = "admin",
                Company = "",
                Location = "",
                Mobile = "13500000000",
                Name = "admin",
                Position = ""
            };

            command.IsVaild();

            return await CreateAsync(command, cancellationToken);
        }
        /// <summary>
        /// 创建管理员账号
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<Result<int>> CreateAsync([Required][FromBody] AdminUserCreateCommand request, CancellationToken cancellationToken)
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

                return RestFull.Success(data: res);
            }
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdatePasswordAsync([Required][FromBody] AdminUserUpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var admin = await db.GetByIdAsync<AdminUserEntity>(request.Id, cancellationToken);

            request.NewPassword = Encrypt.Md5By32(request.NewPassword);
            request.OldPassword = Encrypt.Md5By32(request.OldPassword);

            if (!admin.Password.Equals(request.OldPassword))
                Failure.Error("旧密码错误");

            var res = await db.UpdateAsync(c => c.Id == request.Id, c => new AdminUserEntity { Password = request.NewPassword }, cancellationToken);

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
        public async Task<Result<int>> UpdateFieldAsync([Required] long id, [Required] string field, string value, CancellationToken cancellationToken)
        {
            var res = 0;
            switch (field.ToLower())
            {
                case "name":
                    res = await db.UpdateAsync(c => c.Id == id, c => new AdminUserEntity() { Name = value, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "position":
                    res = await db.UpdateAsync(c => c.Id == id, c => new AdminUserEntity() { Position = value, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "location":
                    res = await db.UpdateAsync(c => c.Id == id, c => new AdminUserEntity() { Location = value, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "company":
                    res = await db.UpdateAsync(c => c.Id == id, c => new AdminUserEntity() { Company = value, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case "picture":
                    res = await db.UpdateAsync(c => c.Id == id, c => new AdminUserEntity() { Picture = value, UpdatedAt = DateTime.Now }, cancellationToken);
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
        public override async Task<Result<int>> DeleteAsync([Required] long[] ids, CancellationToken cancellationToken)
        {
            var res = await db.DeleteAsync(c => ids.Contains(c.Id), cancellationToken);

            if (res > 0)
            {
                //删除登录记录
                await userLoginRecord.DeleteAsync(c => ids.Contains(c.AdminId), cancellationToken);
                //删除关联的角色
                await userRole.DeleteAsync(c => ids.Contains(c.AdminId), cancellationToken);

                DeletedAction?.Invoke(ids);

                return RestFull.Success(data: res);
            }
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<AdminUserDto> LoginAsync(AdminUserLoginCommand request, CancellationToken cancellationToken = default)
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
        /// <summary>
        /// 获取账号信息（根据账号或手机号码）
        /// </summary>
        /// <param name="accountMode"></param>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<AdminUserDto>> GetAccountAsync([Required] AccountMode accountMode, [Required] string account, CancellationToken cancellationToken)
        {
            switch (accountMode)
            {
                case AccountMode.UserName:

                    var res = await db.GetFirstAsync<AdminUserDto>(c => c.UserName.Equals(account), cancellationToken: cancellationToken);

                    return RestFull.Success(data: res);

                case AccountMode.Mobile:

                    res = await db.GetFirstAsync<AdminUserDto>(c => c.Mobile.Equals(account), cancellationToken: cancellationToken);

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
        public async Task<Result<bool>> GetAnyAsync([Required] AccountMode accountMode, [Required] string account, [Required] long notId, CancellationToken cancellationToken)
        {
            var res = false;

            if (notId > 0)
            {
                switch (accountMode)
                {
                    case AccountMode.UserName:
                        res = await db.AnyAsync(c => c.Id != notId && c.UserName == account, cancellationToken);
                        break;
                    case AccountMode.Mobile:
                        res = await db.AnyAsync(c => c.Id != notId && c.Mobile == account, cancellationToken);
                        break;
                }
            }
            else
            {
                switch (accountMode)
                {
                    case AccountMode.UserName:
                        res = await db.AnyAsync(c => c.UserName == account, cancellationToken);
                        break;
                    case AccountMode.Mobile:
                        res = await db.AnyAsync(c => c.Mobile == account, cancellationToken);
                        break;
                }
            }

            return RestFull.Success(data: res);
        }
        public override async Task<Result<IList<AdminUserDto>>> GetListAsync([FromQuery, Required] AdminUserQueryCommand request, CancellationToken cancellationToken)
        {
            var selector = db.BuildFilter()

                   .And(c => c.Status == request.Status, request.Status != Status.Default)
                   .And(c =>
                               c.Name.Contains(request.Keyword) ||
                               c.Mobile.Contains(request.Keyword) ||
                               c.UserName.Contains(request.Keyword), !request.Keyword.IsEmpty());

            var res = await db.GetListAsync<AdminUserDto>(selector: selector, orderby: $"{nameof(AdminUserEntity.Id)} asc", limit: request.Limit, cancellationToken: cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取账号分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<Result<PagedModel<AdminUserDto>>> GetPagedListAsync([Required][FromQuery] AdminUserQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var selector = db.BuildFilter()

                .And(c => c.Status == request.Status, request.Status != Status.Default)
                .And(c =>
                            c.Name.Contains(request.Keyword) ||
                            c.Mobile.Contains(request.Keyword) ||
                            c.UserName.Contains(request.Keyword), !request.Keyword.IsEmpty());

            var res = await db.GetPagedListAsync<AdminUserDto>(selector, $"{nameof(AdminUserEntity.Id)} asc", request.CurrentPage, request.PageSize, cancellationToken);

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
        public async Task<Result<int>> CreateRelevanceRoleAsync([Required][FromBody] AdminUserRelevanceRoleCommand request, CancellationToken cancellationToken)
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
            {
                var res = await userRole.AddAsync(userRoles, cancellationToken: cancellationToken);
            }

            return RestFull.Success(data: 1);
        }
        /// <summary>
        /// 获取账号关联的角色id集合
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<long>>> GetRelevanceRoleAsync([Required] long adminId, CancellationToken cancellationToken)
        {
            var res = await userRole.Table.Where(c => c.AdminId == adminId).Select(c => c.RoleId).ToListAsync(cancellationToken);

            return RestFull.Success(data: res.As<IList<long>>());
        }

        #endregion

        #region [ 登录记录 ]

        /// <summary>
        /// 获取最近登录记录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedModel<AdminUserLoginRecordDto>> GetRecordPageListAsync(AdminUserLoginRecordQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await View.Create(db.Context)

                .WhereIf(c => c.Name.Contains(request.Keyword) || c.Mobile.Contains(request.Keyword) || c.UserName.Contains(request.Keyword), request.Keyword.NotEmpty())

                .OrderByBatch($"{nameof(AdminUserLoginRecordEntity.Id)} asc")

                .ProjectTo<AdminUserLoginRecordDto>(mapper.ConfigurationProvider)
                .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }

        #endregion
    }
}
