using XUCore.Template.Ddd.Applaction.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.Ddd.Domain.Auth.Permission;
using XUCore.Template.Ddd.Domain.User.LoginRecord;
using XUCore.Template.Ddd.Domain.User.User;
using XUCore.NetCore;
using XUCore.Paging;

namespace XUCore.Template.Ddd.Applaction.AppServices.Login
{
    /// <summary>
    /// 用户登录接口
    /// </summary>
    public interface ILoginAppService : IAppService
    {
        #region [ 登录 ]

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<LoginTokenDto>> Login([FromBody] UserLoginCommand command, CancellationToken cancellationToken);
        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<string>> VerifyTokenAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task LoginOutAsync(CancellationToken cancellationToken);

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
        Task<Result<IList<UserLoginRecordDto>>> GetRecordAsync(UserLoginRecordQueryList command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PagedModel<UserLoginRecordDto>>> GetRecordAsync(UserLoginRecordQueryPaged command, CancellationToken cancellationToken = default);

        #endregion
    }
}