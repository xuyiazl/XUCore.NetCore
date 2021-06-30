using AutoMapper;
using XUCore.Net5.Template.Domain.Common.Mappings;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using System;

namespace XUCore.Net5.Template.Domain.Sys.Permission
{
    public class PermissionMenuDto : DtoKeyBase<AdminMenuEntity>
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string OnlyCode { get; set; }
    }
}
