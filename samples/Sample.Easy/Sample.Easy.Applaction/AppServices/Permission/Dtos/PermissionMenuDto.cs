﻿using Sample.Easy.Core;
using Sample.Easy.Persistence.Entities.Sys.Admin;

namespace Sample.Easy.Applaction.Permission
{
    /// <summary>
    /// 权限导航
    /// </summary>
    public class PermissionMenuDto : DtoKeyBase<AdminMenuEntity>
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图标样式
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 连接地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 唯一代码
        /// </summary>
        public string OnlyCode { get; set; }
    }
}