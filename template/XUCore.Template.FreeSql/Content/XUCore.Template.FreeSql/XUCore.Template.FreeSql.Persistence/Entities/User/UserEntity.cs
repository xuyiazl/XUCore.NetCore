
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using XUCore.NetCore.FreeSql.Entity;

namespace XUCore.Template.FreeSql.Persistence.Entities.User
{
    /// <summary>
    /// 用户表
    /// </summary>
	[Table(Name = "sys_user")]
    [Index("idx_{tablename}_01", nameof(UserName), true)]
    [Index("idx_{tablename}_02", nameof(Mobile), true)]
    public partial class UserEntity : EntityFull
    {
        public UserEntity()
        {
            UserRoles = new List<UserRoleEntity>();
            LoginRecords = new List<UserLoginRecordEntity>();
        }
        /// <summary>
        /// 用户名
        /// </summary>
        [Column(StringLength = 60)]
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Column(StringLength = 11)]
        public string Mobile { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Column(StringLength = 60)]
        public string Password { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        [Column(StringLength = 60)]
        public string Name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [Column(StringLength = 100)]
        public string Picture { get; set; }
        /// <summary>
        /// 所在位置
        /// </summary>
        [Column(StringLength = 60)]
        public string Location { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        [Column(StringLength = 60)]
        public string Position { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        [Column(StringLength = 60)]
        public string Company { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginCount { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LoginLastTime { get; set; }
        /// <summary>
        /// 最后登录id
        /// </summary>
        public string LoginLastIp { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; } = true;
        /// <summary>
        /// 对应用户角色关联
        /// </summary>
        [Navigate(ManyToMany = typeof(UserRoleEntity))]
        public ICollection<UserRoleEntity> UserRoles;
        /// <summary>
        /// 对应登录记录关联
        /// </summary>
        [Navigate(ManyToMany = typeof(UserLoginRecordEntity))]
        public ICollection<UserLoginRecordEntity> LoginRecords;
    }
}
