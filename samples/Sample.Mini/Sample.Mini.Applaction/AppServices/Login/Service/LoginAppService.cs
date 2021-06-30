using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.NetCore;
using XUCore.Paging;
using XUCore.Serializer;
using Sample.Mini.Applaction.Authorization;
using Sample.Mini.Applaction.Permission;
using Sample.Mini.Core;
using Sample.Mini.Persistence;

namespace Sample.Mini.Applaction.Login
{
    /// <summary>
    /// 登录接口
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class LoginAppService : AppService, ILoginAppService
    {
        private readonly IPermissionService permissionService;
        private readonly IAuthService authService;

        private readonly INigelDbRepository db;
        private readonly IMapper mapper;

        public LoginAppService(IServiceProvider serviceProvider)
        {
            this.permissionService = serviceProvider.GetService<IPermissionService>();
            this.authService = serviceProvider.GetService<IAuthService>();

            this.db = serviceProvider.GetService<INigelDbRepository>();
            this.mapper = serviceProvider.GetService<IMapper>();
        }

        #region [ 登录 ]

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<Result<LoginTokenDto>> LoginAsync([Required][FromBody] AdminUserLoginCommand request, CancellationToken cancellationToken)
        {
            request.IsVaild();

            (var accessToken, var refreshToken) = await authService.LoginAsync(request, cancellationToken);

            return Success(SubCode.Success, new LoginTokenDto
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
            return Success(SubCode.Success, data: new
            {
                authService.AdminId,
                authService.AdminName
            }.ToJson());
        }

        #endregion

        #region [ 登录后的权限获取 ]

        /// <summary>
        /// 查询是否有权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="onlyCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Permission/Exists")]
        public async Task<Result<bool>> GetPermissionExistsAsync([Required] long adminId, [Required] string onlyCode, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.ExistsAsync(adminId, onlyCode, cancellationToken);

            return Success(SubCode.Success, res);
        }
        /// <summary>
        /// 查询权限导航
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Permission/Menu")]
        public async Task<Result<IList<PermissionMenuTreeDto>>> GetPermissionMenusAsync([Required] long adminId, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.GetMenusAsync(adminId, cancellationToken);

            return Success(SubCode.Success, res);
        }
        /// <summary>
        /// 查询权限导航（快捷导航）
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Permission/Express")]
        public async Task<Result<IList<PermissionMenuDto>>> GetPermissionMenuExpressAsync([Required] long adminId, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.GetMenuExpressAsync(adminId, cancellationToken);

            return Success(SubCode.Success, res);
        }

        #endregion

        #region [ 登录记录 ]

        /// <summary>
        /// 获取最近登录记录
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Record/List")]
        public async Task<Result<IList<LoginRecordDto>>> GetRecordListAsync([Required] int limit, [Required] long adminId, CancellationToken cancellationToken = default)
        {
            var res = await View.Create(db.Context)

                 .Where(c => c.AdminId == adminId)

                 .OrderByDescending(c => c.LoginTime)
                 .Take(limit)

                 .ProjectTo<LoginRecordDto>(mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);

            return Success(SubCode.Success, res.As<IList<LoginRecordDto>>());
        }
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Record/Page")]
        public async Task<Result<PagedModel<LoginRecordDto>>> GetRecordPageAsync([Required][FromQuery] LoginRecordQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            request.IsVaild();

            var res = await View.Create(db.Context)

                 .WhereIf(c => c.Name.Contains(request.Search) || c.Mobile.Contains(request.Search) || c.UserName.Contains(request.Search), !string.IsNullOrEmpty(request.Search))

                 .OrderByBatch($"{request.Sort} {request.Order}", !request.Sort.IsEmpty() && !request.Order.IsEmpty())

                 .ProjectTo<LoginRecordDto>(mapper.ConfigurationProvider)
                 .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

            return Success(SubCode.Success, res.ToModel());
        }

        #endregion
    }
}
