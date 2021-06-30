using AutoMapper;
using XUCore.Net5.Template.Domain.Common.Mappings;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using System;
using System.Collections.Generic;

namespace XUCore.Net5.Template.Domain.Sys.AdminMenu
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
