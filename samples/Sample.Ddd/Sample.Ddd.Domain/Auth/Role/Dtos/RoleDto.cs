using AutoMapper;
using Sample.Ddd.Domain.Core.Mappings;
using Sample.Ddd.Domain.Core.Entities.Auth;
using System;

namespace Sample.Ddd.Domain.Auth.Role
{
    public class RoleDto : DtoBase<RoleEntity>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
    }
}
