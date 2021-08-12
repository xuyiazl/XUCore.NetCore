using AutoMapper;
using XUCore.Net5.Template.Domain.Core.Mappings;
using XUCore.Net5.Template.Domain.Core.Entities.Auth;
using System;

namespace XUCore.Net5.Template.Domain.Auth.Permission
{
    public class PermissionMenuDto : DtoKeyBase<MenuEntity>
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string OnlyCode { get; set; }
    }
}
