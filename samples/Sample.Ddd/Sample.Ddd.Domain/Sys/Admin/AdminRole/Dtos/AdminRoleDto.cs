using AutoMapper;
using Sample.Ddd.Domain.Common.Mappings;
using Sample.Ddd.Domain.Core.Entities.Sys.Admin;
using System;

namespace Sample.Ddd.Domain.Sys.AdminRole
{
    public class AdminRoleDto : DtoBase<AdminRoleEntity>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
    }
}
