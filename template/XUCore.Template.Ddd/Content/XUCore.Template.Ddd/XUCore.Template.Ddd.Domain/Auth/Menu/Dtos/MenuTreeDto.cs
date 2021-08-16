using AutoMapper;
using XUCore.Template.Ddd.Domain.Core.Mappings;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using System;
using System.Collections.Generic;

namespace XUCore.Template.Ddd.Domain.Auth.Menu
{
    public class MenuTreeDto : MenuDto
    {
        public IList<MenuTreeDto> Child { get; set; }
    }
}
