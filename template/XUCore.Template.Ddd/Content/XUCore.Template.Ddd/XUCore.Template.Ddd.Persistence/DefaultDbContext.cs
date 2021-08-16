using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using XUCore.Template.Ddd.Domain.Core.Entities.User;
using XUCore.Template.Ddd.Domain.Core.Events;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using XUCore.NetCore.Data;

namespace XUCore.Template.Ddd.Persistence
{
    public class DefaultDbContext : DBContextFactory, IDefaultDbContext
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {

        }

        public override Assembly[] Assemblies => new Assembly[] { Assembly.GetExecutingAssembly() };
        /// <summary>
        /// 事件存储模型
        /// </summary>
        public DbSet<StoredEvent> StoredEvent => Set<StoredEvent>();
        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<UserEntity> User => Set<UserEntity>();
        /// <summary>
        /// 用户角色关联
        /// </summary>
        public DbSet<UserRoleEntity> UserRole => Set<UserRoleEntity>();
        /// <summary>
        /// 用户登录记录
        /// </summary>
        public DbSet<UserLoginRecordEntity> UserLoginRecord => Set<UserLoginRecordEntity>();
        /// <summary>
        /// 角色菜单关联
        /// </summary>
        public DbSet<RoleMenuEntity> RoleMenu => Set<RoleMenuEntity>();
        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<RoleEntity> Role => Set<RoleEntity>();
        /// <summary>
        /// 菜单
        /// </summary>
        public DbSet<MenuEntity> Menu => Set<MenuEntity>();
    }
}