using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;
using XUCore.Template.FreeSql.DbService.User.User;
using XUCore.Template.FreeSql.DbService.Auth.Permission;

namespace XUCore.Template.FreeSql.Applaction.Login
{
    /// <summary>
    /// 用户登录接口
    /// </summary>
    public interface ILoginAppService : IAppService
    {

        #region [ 登录 ]

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<LoginTokenDto>> LoginAsync(UserLoginCommand request, CancellationToken cancellationToken);
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
        /// <param name="userId"></param>
        /// <param name="onlyCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<bool>> GetPermissionExistsAsync(long userId, string onlyCode, CancellationToken cancellationToken = default);
        /// <summary>
        /// 查询权限导航
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<PermissionMenuTreeDto>>> GetPermissionMenusAsync(long userId, CancellationToken cancellationToken = default);
        /// <summary>
        /// 查询权限导航（快捷导航）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<PermissionMenuDto>>> GetPermissionMenuExpressAsync(long userId, CancellationToken cancellationToken = default);

        #endregion
    }
}
