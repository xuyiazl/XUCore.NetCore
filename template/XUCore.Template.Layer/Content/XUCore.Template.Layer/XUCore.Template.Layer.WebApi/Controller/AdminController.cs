using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;
using XUCore.Template.Layer.Applaction;
using XUCore.Template.Layer.Applaction.Admin;
using XUCore.Template.Layer.Core.Enums;
using XUCore.Template.Layer.DbService.Admin.AdminMenu;
using XUCore.Template.Layer.DbService.Admin.AdminRole;
using XUCore.Template.Layer.DbService.Admin.AdminUser;

namespace XUCore.Template.Layer.WebApi.Controller
{
    /// <summary>
    /// 管理员管理
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class AdminController : ApiControllerBase
    {
        private readonly IAdminAppService adminAppService;
        public AdminController(ILogger<AdminController> logger, IAdminAppService adminAppService) : base(logger)
        {
            this.adminAppService = adminAppService;
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
        [HttpPost("/api/[controller]/InitAccount")]
        [AllowAnonymous]
        public async Task<Result<long>> CreateInitAccountAsync(CancellationToken cancellationToken = default)
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

            return await adminAppService.CreateUserAsync(command, cancellationToken);
        }
        /// <summary>
        /// 创建管理员账号
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<long>> CreateUserAsync([Required][FromBody] AdminUserCreateCommand command, CancellationToken cancellationToken = default)
        {
            return await adminAppService.CreateUserAsync(command, cancellationToken);
        }
        /// <summary>
        /// 更新账号信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result<int>> UpdateUserAsync([Required][FromBody] AdminUserUpdateInfoCommand command, CancellationToken cancellationToken = default)
        {
            return await adminAppService.UpdateUserAsync(command, cancellationToken);
        }
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Password")]
        public async Task<Result<int>> UpdateUserAsync([Required][FromBody] AdminUserUpdatePasswordCommand command, CancellationToken cancellationToken = default)
        {
            return await adminAppService.UpdateUserAsync(command, cancellationToken);
        }
        /// <summary>
        /// 更新指定字段内容
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="field">字段名</param>
        /// <param name="value">新值</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Field")]
        public async Task<Result<int>> UpdateUserAsync([Required][FromQuery] long id, [Required][FromQuery] string field, [FromQuery] string value, CancellationToken cancellationToken = default)
        {
            return await adminAppService.UpdateUserAsync(id, field, value, cancellationToken);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <param name="status">数据状态</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Status")]
        public async Task<Result<int>> UpdateUserAsync([Required][FromQuery] long[] ids, [Required][FromQuery] Status status, CancellationToken cancellationToken = default)
        {
            return await adminAppService.UpdateUserAsync(ids, status, cancellationToken);
        }
        /// <summary>
        /// 删除账号（物理删除）
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Result<int>> DeleteUserAsync([Required][FromQuery] long[] ids, CancellationToken cancellationToken = default)
        {
            return await adminAppService.DeleteUserAsync(ids, cancellationToken);
        }
        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        public async Task<Result<AdminUserDto>> GetUserAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            return await adminAppService.GetUserAsync(id, cancellationToken);
        }
        /// <summary>
        /// 获取账号信息（根据账号或手机号码）
        /// </summary>
        /// <param name="accountMode">账号类型（1、账号，2、手机）</param>
        /// <param name="account">账号</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Account")]
        public async Task<Result<AdminUserDto>> GetUserByAccountAsync([Required] AccountMode accountMode, [Required] string account, CancellationToken cancellationToken = default)
        {
            return await adminAppService.GetUserByAccountAsync(accountMode, account, cancellationToken);
        }
        /// <summary>
        /// 检查账号或者手机号是否存在
        /// </summary>
        /// <param name="accountMode">账号类型（1、账号，2、手机）</param>
        /// <param name="account">账号</param>
        /// <param name="notId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Any")]
        public async Task<Result<bool>> GetUserAnyAsync([Required] AccountMode accountMode, [Required] string account, [Required] long notId, CancellationToken cancellationToken = default)
        {
            return await adminAppService.GetUserAnyAsync(accountMode, account, notId, cancellationToken);
        }
        /// <summary>
        /// 获取账号分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Page")]
        public async Task<Result<PagedModel<AdminUserDto>>> GetUserPagedAsync([Required][FromQuery] AdminUserQueryPagedCommand command, CancellationToken cancellationToken = default)
        {
            return await adminAppService.GetUserPagedAsync(command, cancellationToken);
        }

        #endregion

        #region [ 账号&角色 关联操作 ]

        /// <summary>
        /// 账号关联角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("RelevanceRole")]
        public async Task<Result<int>> CreateUserRelevanceRoleIdAsync([Required][FromBody] AdminUserRelevanceRoleCommand command, CancellationToken cancellationToken = default)
        {
            return await adminAppService.CreateUserRelevanceRoleIdAsync(command, cancellationToken);
        }
        /// <summary>
        /// 获取账号关联的角色id集合
        /// </summary>
        /// <param name="adminId">管理员id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("RelevanceRole")]
        public async Task<Result<IList<long>>> GetUserRelevanceRoleIdsAsync([Required] long adminId, CancellationToken cancellationToken = default)
        {
            return await adminAppService.GetUserRelevanceRoleIdsAsync(adminId, cancellationToken);
        }

        #endregion

        #region [ 登录记录 ]

        /// <summary>
        /// 获取最近登录记录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Record/List")]
        public async Task<Result<IList<AdminUserLoginRecordDto>>> GetRecordListAsync([Required][FromQuery] AdminUserLoginRecordQueryCommand command, CancellationToken cancellationToken = default)
        {
            return await adminAppService.GetRecordListAsync(command, cancellationToken);
        }
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Record/Page")]
        public async Task<Result<PagedModel<AdminUserLoginRecordDto>>> GetRecordPageAsync([Required][FromQuery] AdminUserLoginRecordQueryPagedCommand command, CancellationToken cancellationToken = default)
        {
            return await adminAppService.GetRecordPageAsync(command, cancellationToken);
        }

        #endregion

        #region [ 角色管理 ]

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("Role")]
        public async Task<Result<long>> CreateRoleAsync([Required][FromBody] AdminRoleCreateCommand command, CancellationToken cancellationToken = default)
        {
            return await adminAppService.CreateRoleAsync(command, cancellationToken);
        }
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Role")]
        public async Task<Result<int>> UpdateRoleAsync([Required][FromBody] AdminRoleUpdateCommand command, CancellationToken cancellationToken = default)
        {
            return await adminAppService.UpdateRoleAsync(command, cancellationToken);
        }
        /// <summary>
        /// 更新角色指定字段内容
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="field">字段名</param>
        /// <param name="value">新值</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Role/Field")]
        public async Task<Result<int>> UpdateRoleAsync([Required][FromQuery] long id, [Required][FromQuery] string field, [FromQuery] string value, CancellationToken cancellationToken = default)
        {
            return await adminAppService.UpdateRoleAsync(id, field, value, cancellationToken);
        }
        /// <summary>
        /// 更新角色状态
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <param name="status">数据状态</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Role/Status")]
        public async Task<Result<int>> UpdateRoleAsync([Required][FromQuery] long[] ids, [Required][FromQuery] Status status, CancellationToken cancellationToken = default)
        {
            return await adminAppService.UpdateRoleAsync(ids, status, cancellationToken);
        }
        /// <summary>
        /// 删除角色（物理删除）
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("Role")]
        public async Task<Result<int>> DeleteRoleAsync([Required][FromQuery] long[] ids, CancellationToken cancellationToken = default)
        {
            return await adminAppService.DeleteRoleAsync(ids, cancellationToken);
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Role/{id:long}")]
        public async Task<Result<AdminRoleDto>> GetRoleAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            return await adminAppService.GetRoleAsync(id, cancellationToken);
        }
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Role")]
        public async Task<Result<IList<AdminRoleDto>>> GetRoleListAsync([Required][FromQuery] AdminRoleQueryCommand command, CancellationToken cancellationToken = default)
        {
            return await adminAppService.GetRoleListAsync(command, cancellationToken);
        }
        /// <summary>
        /// 获取角色分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Role/Page")]
        public async Task<Result<PagedModel<AdminRoleDto>>> GetRolePagedAsync([Required][FromQuery] AdminRoleQueryPagedCommand command, CancellationToken cancellationToken = default)
        {
            return await adminAppService.GetRolePagedAsync(command, cancellationToken);
        }
        /// <summary>
        /// 获取角色关联的所有导航id集合
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Role/RelevanceMenu")]
        public async Task<Result<IList<long>>> GetRoleRelevanceMenuAsync([Required] int roleId, CancellationToken cancellationToken = default)
        {
            return await adminAppService.GetRoleRelevanceMenuAsync(roleId, cancellationToken);
        }

        #endregion

        #region [ 权限导航操作 ]

        /// <summary>
        /// 创建导航
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("Menu")]
        public async Task<Result<long>> CreateMenuAsync([Required][FromBody] AdminMenuCreateCommand command, CancellationToken cancellationToken = default)
        {
            return await adminAppService.CreateMenuAsync(command, cancellationToken);
        }
        /// <summary>
        /// 更新导航信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Menu")]
        public async Task<Result<int>> UpdateMenuAsync([Required][FromBody] AdminMenuUpdateCommand command, CancellationToken cancellationToken = default)
        {
            return await adminAppService.UpdateMenuAsync(command, cancellationToken);
        }
        /// <summary>
        /// 更新导航指定字段内容
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="field">字段名</param>
        /// <param name="value">新值</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Menu/Field")]
        public async Task<Result<int>> UpdateMenuAsync([Required][FromQuery] long id, [Required][FromQuery] string field, [FromQuery] string value, CancellationToken cancellationToken = default)
        {
            return await adminAppService.UpdateMenuAsync(id, field, value, cancellationToken);
        }
        /// <summary>
        /// 更新导航状态
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <param name="status">数据状态</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Menu/Status")]
        public async Task<Result<int>> UpdateMenuAsync([Required][FromQuery] long[] ids, [Required][FromQuery] Status status, CancellationToken cancellationToken = default)
        {
            return await adminAppService.UpdateMenuAsync(ids, status, cancellationToken);
        }
        /// <summary>
        /// 删除导航（物理删除）
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("Menu")]
        public async Task<Result<int>> DeleteMenuAsync([Required][FromQuery] long[] ids, CancellationToken cancellationToken = default)
        {
            return await adminAppService.DeleteMenuAsync(ids, cancellationToken);
        }
        /// <summary>
        /// 获取导航信息
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Menu/{id:long}")]
        public async Task<Result<AdminMenuDto>> GetMenuAsync([Required] long id, CancellationToken cancellationToken = default)
        {
            return await adminAppService.GetMenuAsync(id, cancellationToken);
        }
        /// <summary>
        /// 获取导航树形结构
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Menu/Tree")]
        public async Task<Result<IList<AdminMenuTreeDto>>> GetMenuByTreeAsync(CancellationToken cancellationToken = default)
        {
            return await adminAppService.GetMenuByTreeAsync(cancellationToken);
        }
        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Menu/List")]
        public async Task<Result<IList<AdminMenuDto>>> GetMenuListAsync([Required][FromQuery] AdminMenuQueryCommand command, CancellationToken cancellationToken = default)
        {
            return await adminAppService.GetMenuListAsync(command, cancellationToken);
        }

        #endregion
    }
}
