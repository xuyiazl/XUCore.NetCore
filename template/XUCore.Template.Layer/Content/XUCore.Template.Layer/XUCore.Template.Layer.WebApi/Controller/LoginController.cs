using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Template.Layer.Applaction;
using XUCore.Template.Layer.Applaction.Login;
using XUCore.Template.Layer.DbService.Admin.AdminUser;
using XUCore.Template.Layer.DbService.Admin.Permission;

namespace XUCore.Template.Layer.WebApi.Controller
{
    /// <summary>
    /// 管理员登录接口
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class LoginController : ApiControllerBase
    {
        private readonly ILoginAppService loginAppService;
        public LoginController(ILogger<LoginController> logger, ILoginAppService loginAppService) : base(logger)
        {
            this.loginAppService = loginAppService;
        }

        #region [ 登录 ]

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/api/[controller]")]
        [AllowAnonymous]
        public async Task<Result<LoginTokenDto>> LoginAsync([Required][FromBody] AdminUserLoginCommand command, CancellationToken cancellationToken)
        {
            return await loginAppService.LoginAsync(command, cancellationToken);
        }
        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("VerifyToken")]
        public async Task<Result<string>> VerifyTokenAsync(CancellationToken cancellationToken)
        {
            return await loginAppService.VerifyTokenAsync(cancellationToken);
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/api/[controller]/Out")]
        public async Task LoginOutAsync(CancellationToken cancellationToken)
        {
            await loginAppService.LoginOutAsync(cancellationToken);
        }

        #endregion

        #region [ 登录后的权限获取 ]

        /// <summary>
        /// 查询是否有权限
        /// </summary>
        /// <param name="adminId">管理员id</param>
        /// <param name="onlyCode">唯一代码</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Permission/Exists")]
        public async Task<Result<bool>> GetPermissionExistsAsync([Required] long adminId, [Required] string onlyCode, CancellationToken cancellationToken = default)
        {
            return await loginAppService.GetPermissionExistsAsync(adminId, onlyCode, cancellationToken);
        }
        /// <summary>
        /// 查询权限导航
        /// </summary>
        /// <param name="adminId">管理员id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Permission/Menu")]
        //[HiddenApi]
        public async Task<Result<IList<PermissionMenuTreeDto>>> GetPermissionMenusAsync([Required] long adminId, CancellationToken cancellationToken = default)
        {
            return await loginAppService.GetPermissionMenusAsync(adminId, cancellationToken);
        }
        /// <summary>
        /// 查询权限导航（快捷导航）
        /// </summary>
        /// <param name="adminId">管理员id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Permission/MenuExpress")]
        public async Task<Result<IList<PermissionMenuDto>>> GetPermissionMenuExpressAsync([Required] long adminId, CancellationToken cancellationToken = default)
        {
            return await loginAppService.GetPermissionMenuExpressAsync(adminId, cancellationToken);
        }

        #endregion
    }
}
