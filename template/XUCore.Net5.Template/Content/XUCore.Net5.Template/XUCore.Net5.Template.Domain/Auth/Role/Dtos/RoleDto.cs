using AutoMapper;
using XUCore.Net5.Template.Domain.Core.Mappings;
using XUCore.Net5.Template.Domain.Core.Entities.Auth;
using System;

namespace XUCore.Net5.Template.Domain.Auth.Role
{
    public class RoleDto : DtoBase<RoleEntity>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
    }
}
