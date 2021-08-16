using Sample.Ddd.Applaction;
using Sample.Ddd.Applaction.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Helpers;
using Sample.Ddd.Domain.Auth.Permission;
using Sample.Ddd.Domain.Core;
using Sample.Ddd.Domain.User.LoginRecord;
using Sample.Ddd.Domain.User.User;
using Sample.Ddd.Infrastructure.Authorization;
using XUCore.NetCore;
using XUCore.NetCore.Swagger;
using XUCore.Paging;
using XUCore.Serializer;

namespace Sample.Ddd.Applaction.AppServices.Login
{
    /// <summary>
    /// 用户登录接口
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.User)]
    public class LoginAppService : AppService, ILoginAppService
    {
        private readonly IAuthService authService;

        public LoginAppService(IMediatorHandler bus, IAuthService authService) : base(bus)
        {
            this.authService = authService;
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
        [HttpPost]
        [AllowAnonymous]
        public async Task<Result<int>> CreateInitAccountAsync(CancellationToken cancellationToken = default)
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
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/api/[controller]")]
        [AllowAnonymous]
        public async Task<Result<LoginTokenDto>> Login([FromBody] UserLoginCommand command, CancellationToken cancellationToken)
        {
            (var accessToken, var refreshToken) = await authService.LoginAsync(command);

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

            return RestFull.Success(SubCode.Success, data: new { authService.UserId, authService.UserName }.ToJson());
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/api/[controller]/Out")]
        public async Task LoginOutAsync(CancellationToken cancellationToken)
        {
            await authService.LoginOutAsync(cancellationToken);
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
        public async Task<Result<IList<UserLoginRecordDto>>> GetRecordAsync([FromQuery] UserLoginRecordQueryList command, CancellationToken cancellationToken = default)
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
        public async Task<Result<PagedModel<UserLoginRecordDto>>> GetRecordAsync([FromQuery] UserLoginRecordQueryPaged command, CancellationToken cancellationToken = default)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion
    }
}
