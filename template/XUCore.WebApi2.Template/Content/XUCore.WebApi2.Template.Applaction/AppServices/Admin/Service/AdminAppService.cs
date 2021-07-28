using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;
using XUCore.WebApi2.Template.Core;
using XUCore.WebApi2.Template.Core.Enums;
using XUCore.WebApi2.Template.DbService.Sys.Admin.AdminMenu;
using XUCore.WebApi2.Template.DbService.Sys.Admin.AdminRole;
using XUCore.WebApi2.Template.DbService.Sys.Admin.AdminUser;

namespace XUCore.WebApi2.Template.Applaction.Admin
{
    /// <summary>
    /// 管理员管理
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class AdminAppService : AppService, IAdminAppService
    {
        private readonly IAdminMenuService adminMenuService;
        private readonly IAdminRoleService adminRoleService;
        private readonly IAdminUserService adminUserService;

        public AdminAppService(IServiceProvider serviceProvider)
        {
            this.adminMenuService = serviceProvider.GetService<IAdminMenuService>();
            this.adminRoleService = serviceProvider.GetService<IAdminRoleService>();
            this.adminUserService = serviceProvider.GetService<IAdminUserService>();
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
            request.IsVaild();

            var res = await adminUserService.CreateAsync(request, cancellationToken);

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
        public async Task<Result<int>> UpdateUserAsync([Required][FromBody] AdminUserUpdateInfoCommand request, CancellationToken cancellationToken = default)
        {
            request.IsVaild();

            var res = await adminUserService.UpdateAsync(request, cancellationToken);

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
            request.IsVaild();

            var res = await adminUserService.UpdateAsync(request, cancellationToken);

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
        public async Task<Result<int>> UpdateUserAsync([Required] long id, [Required] string field, string value, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.UpdateAsync(id, field, value, cancellationToken);

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
        public async Task<Result<int>> UpdateUserAsync([Required] long[] ids, [Required] Status status, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.UpdateAsync(ids, status, cancellationToken);

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
            var res = await adminUserService.DeleteAsync(ids, cancellationToken);

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
        public async Task<Result<AdminUserDto>> GetUserAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.GetByIdAsync(id, cancellationToken);

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
            var res = await adminUserService.GetByAccountAsync(accountMode, account, cancellationToken);

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
            var res = await adminUserService.AnyByAccountAsync(accountMode, account, notId, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取账号分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/User/Page")]
        public async Task<Result<PagedModel<AdminUserDto>>> GetUserPagedAsync([Required][FromQuery] AdminUserQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            request.IsVaild();

            var res = await adminUserService.GetPagedListAsync(request, cancellationToken);

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
        public async Task<Result<int>> CreateUserRelevanceRoleIdAsync([Required][FromBody] AdminUserRelevanceRoleCommand request, CancellationToken cancellationToken = default)
        {
            request.IsVaild();

            var res = await adminUserService.RelevanceRoleAsync(request, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取账号关联的角色id集合
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/User/RelevancRole/{adminId}")]
        public async Task<Result<IList<long>>> GetUserRelevanceRoleIdsAsync([Required] long adminId, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.GetRoleKeysAsync(adminId, cancellationToken);

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
        public async Task<Result<int>> CreateRoleAsync([Required][FromBody] AdminRoleCreateCommand request, CancellationToken cancellationToken = default)
        {
            request.IsVaild();

            var res = await adminRoleService.CreateAsync(request, cancellationToken);

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
            request.IsVaild();

            var res = await adminRoleService.UpdateAsync(request, cancellationToken);

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
        public async Task<Result<int>> UpdateRoleAsync([Required] long id, [Required] string field, string value, CancellationToken cancellationToken = default)
        {
            var res = await adminRoleService.UpdateAsync(id, field, value, cancellationToken);

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
        public async Task<Result<int>> UpdateRoleAsync([Required] long[] ids, [Required] Status status, CancellationToken cancellationToken = default)
        {
            var res = await adminRoleService.UpdateAsync(ids, status, cancellationToken);

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
            var res = await adminRoleService.DeleteAsync(ids, cancellationToken);

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
        public async Task<Result<AdminRoleDto>> GetRoleAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            var res = await adminRoleService.GetByIdAsync(id, cancellationToken);

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
            var res = await adminRoleService.GetAllAsync(cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取角色分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Role/Page")]
        public async Task<Result<PagedModel<AdminRoleDto>>> GetRolePagedAsync([Required][FromQuery] AdminRoleQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            request.IsVaild();

            var res = await adminRoleService.GetPageListAsync(request, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取角色关联的所有导航id集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Role/RelevanceMenu/{roleId}")]
        public async Task<Result<IList<long>>> GetRoleRelevanceMenuIdsAsync([Required] int roleId, CancellationToken cancellationToken = default)
        {
            var res = await adminRoleService.GetRelevanceMenuIdsAsync(roleId, cancellationToken);

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
        public async Task<Result<int>> CreateMenuAsync([Required][FromBody] AdminMenuCreateCommand request, CancellationToken cancellationToken = default)
        {
            request.IsVaild();

            var res = await adminMenuService.CreateAsync(request, cancellationToken);

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
            request.IsVaild();

            var res = await adminMenuService.UpdateAsync(request, cancellationToken);

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
        public async Task<Result<int>> UpdateMenuAsync([Required] long id, [Required] string field, string value, CancellationToken cancellationToken = default)
        {
            var res = await adminMenuService.UpdateAsync(id, field, value, cancellationToken);

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
        public async Task<Result<int>> UpdateMenuAsync([Required] long[] ids, [Required] Status status, CancellationToken cancellationToken = default)
        {
            var res = await adminMenuService.UpdateAsync(ids, status, cancellationToken);

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
            var res = await adminMenuService.DeleteAsync(ids, cancellationToken);

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
        public async Task<Result<AdminMenuDto>> GetMenuAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            var res = await adminMenuService.GetByIdAsync(id, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取导航树形结构
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Menu/Tree")]
        public async Task<Result<IList<AdminMenuTreeDto>>> GetMenuByTreeAsync(CancellationToken cancellationToken = default)
        {
            var res = await adminMenuService.GetListByTreeAsync(cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <param name="isMenu"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Menu/List")]
        public async Task<Result<IList<AdminMenuDto>>> GetMenuByWeightAsync([Required] bool isMenu = true, CancellationToken cancellationToken = default)
        {
            var res = await adminMenuService.GetListByWeightAsync(isMenu, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion
    }
}
