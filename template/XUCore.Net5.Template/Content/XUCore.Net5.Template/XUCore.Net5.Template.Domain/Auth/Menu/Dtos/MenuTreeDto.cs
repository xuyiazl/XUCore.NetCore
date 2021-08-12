using AutoMapper;
using XUCore.Net5.Template.Domain.Core.Mappings;
using XUCore.Net5.Template.Domain.Core.Entities.Auth;
using System;
using System.Collections.Generic;

namespace XUCore.Net5.Template.Domain.Auth.Menu
{
    public class MenuTreeDto : MenuDto
    {
        public IList<MenuTreeDto> Child { get; set; }
    }
}
