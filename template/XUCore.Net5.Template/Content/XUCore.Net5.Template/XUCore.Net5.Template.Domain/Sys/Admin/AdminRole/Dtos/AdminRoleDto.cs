using AutoMapper;
using XUCore.Net5.Template.Domain.Common.Mappings;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using System;

namespace XUCore.Net5.Template.Domain.Sys.AdminRole
{
    public class AdminRoleDto : DtoBase<AdminRoleEntity>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
    }
}
