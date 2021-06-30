using AutoMapper;
using Sample.Ddd.Domain.Common.Mappings;
using Sample.Ddd.Domain.Core.Entities.Sys.Admin;
using System;
using System.Collections.Generic;

namespace Sample.Ddd.Domain.Sys.AdminMenu
{
    public class PermissionMenuTreeDto : DtoKeyBase<AdminMenuEntity>
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string OnlyCode { get; set; }

        public IList<PermissionMenuTreeDto> Child { get; set; }
    }
}
