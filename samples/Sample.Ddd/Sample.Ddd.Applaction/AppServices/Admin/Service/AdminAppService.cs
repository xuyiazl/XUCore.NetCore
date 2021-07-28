using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using Sample.Ddd.Applaction;
using Sample.Ddd.Applaction.Common;
using Sample.Ddd.Domain.Core;
using Sample.Ddd.Domain.Sys.AdminMenu;
using Sample.Ddd.Domain.Sys.AdminRole;
using Sample.Ddd.Domain.Sys.AdminUser;
using XUCore.NetCore;
using XUCore.Paging;

namespace Sample.Ddd.Application.AppServices.Admin
{
    /// <summary>
    /// 管理员管理
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class AdminAppService : AppService, IAdminAppService
    {
        public AdminAppService(IMediatorHandler bus) : base(bus) { }

        #region [ 账号管理 ]

        /// <summary>
        /// 创建管理员账号
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<int>> CreateUserAsync([FromBody] AdminUserCreateCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新账号信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result<int>> UpdateUserAsync([FromBody] AdminUserUpdateInfoCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Password")]
        public async Task<Result<int>> UpdateUserAsync([FromBody] AdminUserUpdatePasswordCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新指定字段内容
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Field")]
        public async Task<Result<int>> UpdateUserAsync([FromQuery] AdminUserUpdateFieldCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Status")]
        public async Task<Result<int>> UpdateUserAsync([FromQuery] AdminUserUpdateStatusCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 删除账号（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Result<int>> DeleteUserAsync([FromQuery] AdminUserDeleteCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result<AdminUserDto>> GetUserAsync([FromQuery] AdminUserQueryDetail command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取账号信息（根据账号或手机号码）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Account")]
        public async Task<Result<AdminUserDto>> GetUserAsync([FromQuery] AdminUserQueryByAccount command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 检查账号或者手机号是否存在
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Any")]
        public async Task<Result<bool>> GetUserAsync([FromQuery] AdminUserAnyByAccount command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取账号分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Page")]
        public async Task<Result<PagedModel<AdminUserDto>>> GetUserAsync([FromQuery] AdminUserQueryPaged command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion

        #region [ 账号&角色 关联操作 ]

        /// <summary>
        /// 账号关联角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("RelevanceRoleId")]
        public async Task<Result<int>> CreateUserAsync([FromBody] AdminUserRelevanceRoleCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取账号关联的角色id集合
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("RelevanceRoleId")]
        public async Task<Result<IList<long>>> GetUserAsync([FromQuery] AdminUserQueryRoleKeys command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion

        #region [ 角色管理 ]

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<int>> CreateRoleAsync([FromBody] AdminRoleCreateCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result<int>> UpdateRoleAsync([FromBody] AdminRoleUpdateCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新角色指定字段内容
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Field")]
        public async Task<Result<int>> UpdateRoleAsync([FromQuery] AdminRoleUpdateFieldCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新角色状态
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Status")]
        public async Task<Result<int>> UpdateRoleAsync([FromQuery] AdminRoleUpdateStatusCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 删除角色（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Result<int>> DeleteRoleAsync([FromQuery] AdminRoleDeleteCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result<AdminRoleDto>> GetRoleAsync([FromQuery] AdminRoleQueryDetail command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("All")]
        public async Task<Result<IList<AdminRoleDto>>> GetRoleAsync([FromQuery] AdminRoleQueryAll command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取角色分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Page")]
        public async Task<Result<PagedModel<AdminRoleDto>>> GetRoleAsync([FromQuery] AdminRoleQueryPaged command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取角色关联的所有导航id集合
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("RelevanceMenu")]
        public async Task<Result<IList<long>>> GetRoleAsync([FromQuery] AdminRoleQueryMenuKeys command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion

        #region [ 权限导航操作 ]

        /// <summary>
        /// 创建导航
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result<int>> CreateMenuAsync([FromBody] AdminMenuCreateCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新导航信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result<int>> UpdateMenuAsync([FromBody] AdminMenuUpdateCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新导航指定字段内容
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Field")]
        public async Task<Result<int>> UpdateMenuAsync([FromQuery] AdminMenuUpdateFieldCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新导航状态
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("Status")]
        public async Task<Result<int>> UpdateMenuAsync([FromQuery] AdminMenuUpdateStatusCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 删除导航（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Result<int>> DeleteMenuAsync([FromQuery] AdminMenuDeleteCommand command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取导航信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result<AdminMenuDto>> GetMenuAsync([FromQuery] AdminMenuQueryDetail command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取导航树形结构
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Tree")]
        public async Task<Result<IList<AdminMenuTreeDto>>> GetMenuAsync(CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(new AdminMenuQueryByTree(), cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取导航分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("List")]
        public async Task<Result<IList<AdminMenuDto>>> GetMenuAsync([FromQuery] AdminMenuQueryByWeight command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion
    }
}
