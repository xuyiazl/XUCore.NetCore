using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Helpers;
using Sample.Ddd.Applaction;
using Sample.Ddd.Applaction.Common;
using Sample.Ddd.Domain.Core;
using Sample.Ddd.Domain.Sys.AdminMenu;
using Sample.Ddd.Domain.Sys.AdminUser;
using Sample.Ddd.Domain.Sys.LoginRecord;
using Sample.Ddd.Domain.Sys.Permission;
using Sample.Ddd.Infrastructure.Authorization;
using XUCore.NetCore;
using XUCore.NetCore.Swagger;
using XUCore.Paging;
using XUCore.Serializer;

namespace Sample.Ddd.Application.AppServices.Login
{
    /// <summary>
    /// 管理员登录接口
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Login)]
    public class AdminLoginAppService : AppService, IAdminLoginAppService
    {
        private readonly IAdminManager _adminManager;

        public AdminLoginAppService(IMediatorHandler bus, IAdminManager adminManager) : base(bus)
        {
            _adminManager = adminManager;
        }

        #region [ 登录 ]

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <remarks>
        /// 初始账号密码：
        ///     <para>username : admin</para>
        ///     <para>password : admin</para>
        /// </remarks>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<Result<LoginTokenDto>> Login([FromBody] AdminUserLoginCommand command, CancellationToken cancellationToken)
        {
            (var accessToken, var refreshToken) = await _adminManager.LoginAsync(command);

            // 设置 Swagger 自动登录
            Web.HttpContext.SigninToSwagger(accessToken);
            // 设置刷新 token
            Web.HttpContext.Response.Headers["x-access-token"] = refreshToken;

            return RestFull.Success(data: new LoginTokenDto
            {
                Token = accessToken
            });
        }
        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result<string>> VerifyTokenAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            return RestFull.Success(SubCode.Success, data: new { _adminManager.AdminId, _adminManager.AdminName }.ToJson());
        }

        #endregion

        #region [ 登录后的权限获取 ]

        /// <summary>
        /// 查询是否有权限
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Exists")]
        public async Task<Result<bool>> GetPermissionAsync([FromQuery] PermissionQueryExists command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 查询权限导航
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Menu")]
        public async Task<Result<IList<PermissionMenuTreeDto>>> GetPermissionAsync([FromQuery] PermissionQueryMenu command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 查询权限导航（快捷导航）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Express")]
        public async Task<Result<IList<PermissionMenuDto>>> GetPermissionAsync([FromQuery] PermissionQueryMenuExpress command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion

        #region [ 登录记录 ]

        /// <summary>
        /// 获取最近登录记录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("List")]
        public async Task<Result<IList<LoginRecordDto>>> GetRecordAsync([FromQuery] LoginRecordQueryList command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Page")]
        public async Task<Result<PagedModel<LoginRecordDto>>> GetRecordAsync([FromQuery] LoginRecordQueryPaged command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion
    }
}
