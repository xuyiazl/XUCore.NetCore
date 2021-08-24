﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using Sample.Layer.Applaction;
using Sample.Layer.Applaction.Admin;
using Sample.Layer.Applaction.Login;
using Sample.Layer.DbService.Sys.Admin.AdminUser;
using Sample.Layer.DbService.Sys.Admin.Permission;

namespace Sample.Layer.WebApi.Controller
{
    /// <summary>
    /// 管理员登录接口
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class LoginController : ApiControllerBase
    {
        private readonly ILoginAppService loginAppService;
        private readonly IAdminAppService adminAppService;  
        public LoginController(ILogger<LoginController> logger, ILoginAppService loginAppService, IAdminAppService adminAppService) : base(logger)
        {
            this.loginAppService = loginAppService;
            this.adminAppService = adminAppService;
        }

        #region [ 登录 ]

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
