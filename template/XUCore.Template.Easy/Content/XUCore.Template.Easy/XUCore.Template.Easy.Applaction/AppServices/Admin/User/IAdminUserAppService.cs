using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;
using XUCore.Template.Easy.Applaction.Login;
using XUCore.Template.Easy.Core.Enums;
using XUCore.Template.Easy.Persistence.Entities.Admin;

namespace XUCore.Template.Easy.Applaction.Admin
{
    /// <summary>
    /// 管理员
    /// </summary>
    public interface IAdminUserAppService : ICurdAppService<long, AdminUserEntity, AdminUserDto, AdminUserCreateCommand, AdminUserUpdateInfoCommand, AdminUserQueryCommand, AdminUserQueryPagedCommand>
    {
        #region [ 账号管理 ]

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdatePasswordAsync(AdminUserUpdatePasswordCommand request, CancellationToken cancellationToken);
        /// <summary>
        /// 更新指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateFieldAsync(long id, string field, string value, CancellationToken cancellationToken);
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<AdminUserDto> LoginAsync(AdminUserLoginCommand request, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取账号信息（根据账号或手机号码）
        /// </summary>
        /// <param name="accountMode"></param>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<AdminUserDto>> GetAccountAsync(AccountMode accountMode, string account, CancellationToken cancellationToken);
        /// <summary>
        /// 检查账号或者手机号是否存在
        /// </summary>
        /// <param name="accountMode"></param>
        /// <param name="account"></param>
        /// <param name="notId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<bool>> GetAnyAsync(AccountMode accountMode, string account, long notId, CancellationToken cancellationToken);

        #endregion

        #region [ 账号&角色 关联操作 ]

        /// <summary>
        /// 账号关联角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> CreateRelevanceRoleAsync(AdminUserRelevanceRoleCommand request, CancellationToken cancellationToken);
        /// <summary>
        /// 获取账号关联的角色id集合
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<long>>> GetRelevanceRoleAsync(long adminId, CancellationToken cancellationToken);

        #endregion

        #region [ 登录记录 ]

        /// <summary>
        /// 获取最近登录记录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<AdminUserLoginRecordDto>> GetRecordListAsync(AdminUserLoginRecordQueryCommand request, CancellationToken cancellationToken);
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PagedModel<AdminUserLoginRecordDto>> GetRecordPageListAsync(AdminUserLoginRecordQueryPagedCommand request, CancellationToken cancellationToken);

        #endregion
    }
}
