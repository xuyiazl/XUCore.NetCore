using AutoMapper;
using Sample.Ddd.Domain.Core.Mappings;
using Sample.Ddd.Domain.Core.Entities.Auth;
using System;

namespace Sample.Ddd.Domain.Auth.Permission
{
    public class PermissionMenuDto : DtoKeyBase<MenuEntity>
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string OnlyCode { get; set; }
    }
}
