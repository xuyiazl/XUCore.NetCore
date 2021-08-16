using AutoMapper;
using XUCore.Template.Ddd.Domain.Core.Mappings;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using System;

namespace XUCore.Template.Ddd.Domain.Auth.Menu
{
    public class MenuDto : DtoBase<MenuEntity>
    {
        /// <summary>
        /// 导航父级id
        /// </summary>
        public string FatherId { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图标样式
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 唯一代码（权限使用）
        /// </summary>
        public string OnlyCode { get; set; }
        /// <summary>
        /// 是否是导航
        /// </summary>
        public bool IsMenu { get; set; }
        /// <summary>
        /// 排序权重
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// 是否是快捷导航
        /// </summary>
        public bool IsExpress { get; set; }
    }
}
