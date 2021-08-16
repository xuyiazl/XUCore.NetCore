using AutoMapper;
using Sample.Ddd.Domain.Core.Mappings;
using Sample.Ddd.Domain.Core.Entities.Auth;
using System;
using System.Collections.Generic;

namespace Sample.Ddd.Domain.Auth.Menu
{
    public class MenuTreeDto : MenuDto
    {
        public IList<MenuTreeDto> Child { get; set; }
    }
}
