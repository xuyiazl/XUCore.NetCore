using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Net5.Template.Applaction.Common.Interfaces;
using XUCore.Net5.Template.Domain.Sys.AdminMenu;
using XUCore.Net5.Template.Domain.Sys.AdminUser;
using XUCore.Net5.Template.Domain.Sys.LoginRecord;
using XUCore.Net5.Template.Domain.Sys.Permission;
using XUCore.NetCore;
using XUCore.Paging;

namespace XUCore.Net5.Template.Application.AppServices.Login
{
    /// <summary>
    /// 管理员登录接口
    /// </summary>
    public interface IAdminLoginAppService : IAppService
    {
        #region [ 登录 ]

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<LoginTokenDto>> Login([FromBody] AdminUserLoginCommand command, CancellationToken cancellationToken);
        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<string>> VerifyTokenAsync(CancellationToken cancellationToken);

        #endregion

        #region [ 登录后的权限获取 ]

        /// <summary>
        /// 查询是否有权限
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<bool>> GetPermissionAsync(PermissionQueryExists command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 查询权限导航
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<PermissionMenuTreeDto>>> GetPermissionAsync(PermissionQueryMenu command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 查询权限导航（快捷导航）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<PermissionMenuDto>>> GetPermissionAsync(PermissionQueryMenuExpress command, CancellationToken cancellationToken = default);

        #endregion

        #region [ 登录记录 ]

        /// <summary>
        /// 获取最近登录记录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<LoginRecordDto>>> GetRecordAsync(LoginRecordQueryList command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PagedModel<LoginRecordDto>>> GetRecordAsync(LoginRecordQueryPaged command, CancellationToken cancellationToken = default);

        #endregion
    }
}