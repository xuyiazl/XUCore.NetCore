
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using XUCore.NetCore.FreeSql.Entity;

namespace XUCore.Template.FreeSql.Persistence.Entities.User
{
    /// <summary>
    /// 权限导航表
    /// </summary>
	[Table(Name = "sys_menu")]
    [Index("idx_{tablename}_01", nameof(ParentId) + "," + nameof(Name), true)]
    public partial class MenuEntity : EntityFull
    {
        public MenuEntity()
        {
            RoleMenus = new List<RoleMenuEntity>();
        }
        /// <summary>
        /// 导航父级id
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// 上级
        /// </summary>
        [Navigate(nameof(ParentId))]
        public MenuEntity Parent { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        [Navigate(nameof(ParentId))]
        public List<MenuEntity> Childs { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        [Column(StringLength = 30)]
        public string Name { get; set; }
        /// <summary>
        /// 图标样式
        /// </summary>
        [Column(StringLength = 50)]
        public string Icon { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        [Column(StringLength = 500)]
        public string Url { get; set; }
        /// <summary>
        /// 唯一代码（权限使用）
        /// </summary>
        [Column(StringLength = 100)]
        public string OnlyCode { get; set; }
        /// <summary>
        /// 是否是导航
        /// </summary>
        public bool IsMenu { get; set; }
        /// <summary>
        /// 排序权重
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否是快捷导航
        /// </summary>
        public bool IsExpress { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; } = true;
        /// <summary>
        /// 角色导航关联列表
        /// </summary>
        [Navigate(ManyToMany = typeof(RoleMenuEntity))]
        public ICollection<RoleMenuEntity> RoleMenus { get; set; }
    }
}
