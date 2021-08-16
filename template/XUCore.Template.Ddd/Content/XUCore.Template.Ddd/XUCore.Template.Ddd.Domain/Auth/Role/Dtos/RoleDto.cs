using AutoMapper;
using XUCore.Template.Ddd.Domain.Core.Mappings;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using System;

namespace XUCore.Template.Ddd.Domain.Auth.Role
{
    public class RoleDto : DtoBase<RoleEntity>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
    }
}
