using AutoMapper;
using Sample.Ddd.Domain.Core.Mappings;
using Sample.Ddd.Domain.Core.Entities.Sys.Admin;
using System;

namespace Sample.Ddd.Domain.Sys.Permission
{
    public class PermissionMenuDto : DtoKeyBase<AdminMenuEntity>
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string OnlyCode { get; set; }
    }
}
