using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.Ddd.Applaction.Common.Interfaces;
using XUCore.Template.Ddd.Domain.User.User;
using XUCore.NetCore;
using XUCore.Paging;

namespace XUCore.Template.Ddd.Applaction.AppServices.User
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public interface IUserAppService : IAppService
    {
        /// <summary>
        /// 创建用户账号
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> CreateAsync(UserCreateCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新账号信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateAsync(UserUpdateInfoCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateAsync(UserUpdatePasswordCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新指定字段内容
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateAsync(UserUpdateFieldCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateAsync(UserUpdateStatusCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除账号（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> DeleteAsync(UserDeleteCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<UserDto>> GetAsync(UserQueryDetail command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取账号信息（根据账号或手机号码）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<UserDto>> GetAsync(UserQueryByAccount command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 检查账号或者手机号是否存在
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<bool>> GetAsync(UserAnyByAccount command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取账号分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PagedModel<UserDto>>> GetAsync(UserQueryPaged command, CancellationToken cancellationToken = default);
    }
}