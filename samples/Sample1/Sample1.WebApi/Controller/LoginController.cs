using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;
using Sample1.Applaction.Login;
using Sample1.DbService.Sys.Admin.AdminUser;
using Sample1.DbService.Sys.Admin.LoginRecord;
using Sample1.DbService.Sys.Admin.Permission;

namespace Sample1.WebApi.Controller
{
    /// <summary>
    /// 管理员登录接口
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Login)]
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
        [HttpPost]
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

        #region [ 登录记录 ]

        /// <summary>
        /// 获取最近登录记录
        /// </summary>
        /// <param name="limit">记录数</param>
        /// <param name="adminId">管理员id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Record/List")]
        public async Task<Result<IList<LoginRecordDto>>> GetRecordListAsync([Required] int limit, [Required] long adminId, CancellationToken cancellationToken = default)
        {
            return await loginAppService.GetRecordListAsync(limit, adminId, cancellationToken);
        }
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Record/Page")]
        public async Task<Result<PagedModel<LoginRecordDto>>> GetRecordPageAsync([Required][FromQuery] LoginRecordQueryPagedCommand command, CancellationToken cancellationToken = default)
        {
            return await loginAppService.GetRecordPageAsync(command, cancellationToken);
        }

        #endregion
    }
}
