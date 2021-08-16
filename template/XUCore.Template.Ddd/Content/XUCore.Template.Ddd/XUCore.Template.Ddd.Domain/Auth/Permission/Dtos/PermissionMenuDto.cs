using AutoMapper;
using XUCore.Template.Ddd.Domain.Core.Mappings;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using System;

namespace XUCore.Template.Ddd.Domain.Auth.Permission
{
    public class PermissionMenuDto : DtoKeyBase<MenuEntity>
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string OnlyCode { get; set; }
    }
}
