﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Helpers;
using XUCore.NetCore;
using XUCore.NetCore.Authorization.JwtBearer;
using XUCore.NetCore.Swagger;
using XUCore.Serializer;
using XUCore.Template.FreeSql.Core;
using XUCore.Template.FreeSql.Core.Auth;
using XUCore.Template.FreeSql.DbService.Auth.Permission;
using XUCore.Template.FreeSql.DbService.User.User;

namespace XUCore.Template.FreeSql.Applaction.Login
{
    /// <summary>
    /// 用户登录接口
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class LoginAppService : AppService, ILoginAppService
    {
        private readonly IPermissionService permissionService;
        private readonly IUserService userService;
        private readonly IUser user;

        public LoginAppService(IServiceProvider serviceProvider)
        {
            this.permissionService = serviceProvider.GetService<IPermissionService>();
            this.userService = serviceProvider.GetService<IUserService>();
            this.user = serviceProvider.GetService<IUser>();
        }

        #region [ 登录 ]

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/api/[controller]")]
        [AllowAnonymous]
        public async Task<Result<LoginTokenDto>> LoginAsync([Required][FromBody] UserLoginCommand request, CancellationToken cancellationToken)
        {
            var userDto = await userService.LoginAsync(request, cancellationToken);

            // 生成 token
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { ClaimAttributes.UserId , userDto.Id },
                { ClaimAttributes.UserName ,userDto.UserName }
            });

            // 生成 刷新token
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);

            // 设置 Swagger 自动登录
            Web.HttpContext.SigninToSwagger(accessToken);
            // 设置刷新 token
            Web.HttpContext.Response.Headers["x-access-token"] = refreshToken;

            user.SetLoginToken(userDto.Id, accessToken);

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
            return RestFull.Success(data: new
            {
                user.Id,
                user.UserName
            }.ToJson());
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/api/[controller]/Out")]
        public async Task LoginOutAsync(CancellationToken cancellationToken)
        {
            user.RemoveLoginToken();

            await Task.CompletedTask;
        }

        #endregion

        #region [ 登录后的权限获取 ]

        /// <summary>
        /// 查询是否有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="onlyCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Permission/Exists")]
        public async Task<Result<bool>> GetPermissionExistsAsync([Required] long userId, [Required] string onlyCode, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.ExistsAsync(userId, onlyCode, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 查询权限导航
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Permission/Menu")]
        public async Task<Result<IList<PermissionMenuTreeDto>>> GetPermissionMenusAsync([Required] long userId, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.GetMenusAsync(userId, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 查询权限导航（快捷导航）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Permission/Express")]
        public async Task<Result<IList<PermissionMenuDto>>> GetPermissionMenuExpressAsync([Required] long userId, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.GetMenuExpressAsync(userId, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion
    }
}