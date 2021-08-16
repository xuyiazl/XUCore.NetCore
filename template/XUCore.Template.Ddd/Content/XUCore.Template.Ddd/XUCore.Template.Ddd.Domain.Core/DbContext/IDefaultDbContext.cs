using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using XUCore.Template.Ddd.Domain.Core.Entities.User;
using XUCore.Template.Ddd.Domain.Core.Events;
using Microsoft.EntityFrameworkCore;
using XUCore.NetCore.Data;

namespace XUCore.Template.Ddd.Domain.Core
{
    public interface IDefaultDbContext : IDbContext
    {
        /// <summary>
        /// 事件存储模型
        /// </summary>
        DbSet<StoredEvent> StoredEvent { get; }
        /// <summary>
        /// 用户
        /// </summary>
        DbSet<UserEntity> User { get; }
        /// <summary>
        /// 用户角色关联
        /// </summary>
        DbSet<UserRoleEntity> UserRole { get; }
        /// <summary>
        /// 用户登录记录
        /// </summary>
        DbSet<UserLoginRecordEntity> UserLoginRecord { get; }
        /// <summary>
        /// 角色菜单关联
        /// </summary>
        DbSet<RoleMenuEntity> RoleMenu { get; }
        /// <summary>
        /// 角色
        /// </summary>
        DbSet<RoleEntity> Role { get; }
        /// <summary>
        /// 菜单
        /// </summary>
        DbSet<MenuEntity> Menu { get; }
    }
}
