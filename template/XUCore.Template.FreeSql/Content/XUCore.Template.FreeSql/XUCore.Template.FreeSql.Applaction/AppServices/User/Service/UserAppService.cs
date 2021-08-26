using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;
using XUCore.Template.FreeSql.Core;
using XUCore.Template.FreeSql.Core.Enums;
using XUCore.Template.FreeSql.DbService.Auth.Menu;
using XUCore.Template.FreeSql.DbService.Auth.Role;
using XUCore.Template.FreeSql.DbService.User.User;

namespace XUCore.Template.FreeSql.Applaction.User
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class UserAppService : AppService, IUserAppService
    {
        private readonly IMenuService menuService;
        private readonly IRoleService roleService;
        private readonly IUserService userService;

        public UserAppService(IServiceProvider serviceProvider)
        {
            this.menuService = serviceProvider.GetService<IMenuService>();
            this.roleService = serviceProvider.GetService<IRoleService>();
            this.userService = serviceProvider.GetService<IUserService>();
        }

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
        public async Task<Result<long>> CreateInitAccountAsync(CancellationToken cancellationToken = default)
        {
            var command = new UserCreateCommand
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

            return await CreateUserAsync(command, cancellationToken);
        }

        #region [ 账号管理 ]

        /// <summary>
        /// 创建用户账号
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<long>> CreateUserAsync([Required][FromBody] UserCreateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await userService.CreateAsync(request, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
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
        public async Task<Result<int>> UpdateUserAsync([Required][FromBody] UserUpdateInfoCommand request, CancellationToken cancellationToken = default)
        {
            var res = await userService.UpdateAsync(request, cancellationToken);

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
        public async Task<Result<int>> UpdateUserAsync([Required][FromBody] UserUpdatePasswordCommand request, CancellationToken cancellationToken = default)
        {
            var res = await userService.UpdateAsync(request, cancellationToken);

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
        public async Task<Result<int>> UpdateUserAsync([Required][FromQuery] long id, [Required][FromQuery] string field, [FromQuery] string value, CancellationToken cancellationToken = default)
        {
            var res = await userService.UpdateAsync(id, field, value, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="enabled"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("/api/[controller]/User/Enabled")]
        public async Task<Result<int>> UpdateUserAsync([Required][FromQuery] long[] ids, [Required][FromQuery] bool enabled, CancellationToken cancellationToken = default)
        {
            var res = await userService.UpdateAsync(ids, enabled, cancellationToken);

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
        public async Task<Result<int>> DeleteUserAsync([Required][FromQuery] long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await userService.DeleteAsync(ids, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
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
        public async Task<Result<UserDto>> GetUserAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            var res = await userService.GetByIdAsync(id, cancellationToken);

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
        public async Task<Result<UserDto>> GetUserByAccountAsync([Required] AccountMode accountMode, [Required] string account, CancellationToken cancellationToken = default)
        {
            var res = await userService.GetByAccountAsync(accountMode, account, cancellationToken);

            return RestFull.Success(data: res);
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
        public async Task<Result<bool>> GetUserAnyAsync([Required] AccountMode accountMode, [Required] string account, [Required] long notId, CancellationToken cancellationToken = default)
        {
            var res = await userService.AnyByAccountAsync(accountMode, account, notId, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取账号分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/User/Page")]
        public async Task<Result<PagedModel<UserDto>>> GetUserPagedAsync([Required][FromQuery] UserQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            var res = await userService.GetPagedListAsync(request, cancellationToken);

            return RestFull.Success(data: res);
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
        public async Task<Result<int>> CreateUserRelevanceRoleAsync([Required][FromBody] UserRelevanceRoleCommand request, CancellationToken cancellationToken = default)
        {
            var res = await userService.CreateRelevanceRoleAsync(request, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取账号关联的角色id集合
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/User/RelevancRole")]
        public async Task<Result<IList<long>>> GetUserRelevanceRoleKeysAsync([Required] long userId, CancellationToken cancellationToken = default)
        {
            var res = await userService.GetRoleKeysAsync(userId, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion

        #region [ 登录记录 ]

        /// <summary>
        /// 获取最近登录记录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Record/List")]
        public async Task<Result<IList<UserLoginRecordDto>>> GetRecordListAsync([Required][FromQuery] UserLoginRecordQueryCommand request, CancellationToken cancellationToken = default)
        {
            var res = await userService.GetRecordListAsync(request, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Record/Page")]
        public async Task<Result<PagedModel<UserLoginRecordDto>>> GetRecordPagedListAsync([Required][FromQuery] UserLoginRecordQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            var res = await userService.GetRecordPagedListAsync(request, cancellationToken);

            return RestFull.Success(data: res);
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
        public async Task<Result<long>> CreateRoleAsync([Required][FromBody] RoleCreateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await roleService.CreateAsync(request, cancellationToken);

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
        public async Task<Result<int>> UpdateRoleAsync([Required][FromBody] RoleUpdateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await roleService.UpdateAsync(request, cancellationToken);

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
        public async Task<Result<int>> UpdateRoleAsync([Required][FromQuery] long id, [Required][FromQuery] string field, [FromQuery] string value, CancellationToken cancellationToken = default)
        {
            var res = await roleService.UpdateAsync(id, field, value, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="enabled"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("/api/[controller]/Role/Enabled")]
        public async Task<Result<int>> UpdateRoleAsync([Required][FromQuery] long[] ids, [Required][FromQuery] bool enabled, CancellationToken cancellationToken = default)
        {
            var res = await roleService.UpdateAsync(ids, enabled, cancellationToken);

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
        public async Task<Result<int>> DeleteRoleAsync([Required][FromQuery] long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await roleService.DeleteAsync(ids, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
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
        public async Task<Result<RoleDto>> GetRoleAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            var res = await roleService.GetByIdAsync(id, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Role/List")]
        public async Task<Result<IList<RoleDto>>> GetRoleListAsync([Required][FromQuery] RoleQueryCommand request, CancellationToken cancellationToken = default)
        {
            var res = await roleService.GetListAsync(request, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取角色分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Role/Page")]
        public async Task<Result<PagedModel<RoleDto>>> GetRolePagedAsync([Required][FromQuery] RoleQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            var res = await roleService.GetPagedListAsync(request, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取角色关联的所有导航id集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Role/RelevanceMenu")]
        public async Task<Result<IList<long>>> GetRoleRelevanceMenuAsync([Required] int roleId, CancellationToken cancellationToken = default)
        {
            var res = await roleService.GetRelevanceMenuAsync(roleId, cancellationToken);

            return RestFull.Success(data: res);
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
        public async Task<Result<long>> CreateMenuAsync([Required][FromBody] MenuCreateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await menuService.CreateAsync(request, cancellationToken);

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
        public async Task<Result<int>> UpdateMenuAsync([Required][FromBody] MenuUpdateCommand request, CancellationToken cancellationToken = default)
        {
            var res = await menuService.UpdateAsync(request, cancellationToken);

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
        public async Task<Result<int>> UpdateMenuAsync([Required][FromQuery] long id, [Required][FromQuery] string field, [FromQuery] string value, CancellationToken cancellationToken = default)
        {
            var res = await menuService.UpdateAsync(id, field, value, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="enabled"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("/api/[controller]/Menu/Enabled")]
        public async Task<Result<int>> UpdateMenuAsync([Required][FromQuery] long[] ids, [Required][FromQuery] bool enabled, CancellationToken cancellationToken = default)
        {
            var res = await menuService.UpdateAsync(ids, enabled, cancellationToken);

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
        public async Task<Result<int>> DeleteMenuAsync([Required][FromQuery] long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await menuService.DeleteAsync(ids, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
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
        public async Task<Result<MenuDto>> GetMenuAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            var res = await menuService.GetByIdAsync(id, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取导航树形结构
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Menu/Tree")]
        public async Task<Result<IList<MenuTreeDto>>> GetMenuByTreeAsync(CancellationToken cancellationToken = default)
        {
            var res = await menuService.GetListByTreeAsync(cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Menu/List")]
        public async Task<Result<IList<MenuDto>>> GetMenuListAsync([Required][FromQuery] MenuQueryCommand request, CancellationToken cancellationToken = default)
        {
            var res = await menuService.GetListAsync(request, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion
    }
}
