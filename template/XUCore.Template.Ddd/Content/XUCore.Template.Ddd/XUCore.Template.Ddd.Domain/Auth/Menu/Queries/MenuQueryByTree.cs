using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;

namespace XUCore.Template.Ddd.Domain.Auth.Menu
{
    /// <summary>
    /// 查询导航树命令
    /// </summary>
    public class MenuQueryByTree : Command<IList<MenuTreeDto>>
    {
        public class Validator : CommandValidator<MenuQueryByTree>
        {
            public Validator()
            {

            }
        }

        public class Handler : CommandHandler<MenuQueryByTree, IList<MenuTreeDto>>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<MenuTreeDto>> Handle(MenuQueryByTree request, CancellationToken cancellationToken)
            {
                var res = await db.GetListAsync<MenuEntity>(orderby: "Weight desc", cancellationToken: cancellationToken);

                return AuthMenuTree(res, "");
            }

            private IList<MenuTreeDto> AuthMenuTree(IList<MenuEntity> entities, string parentId)
            {
                IList<MenuTreeDto> menus = new List<MenuTreeDto>();

                entities.Where(c => c.FatherId == parentId).ToList().ForEach(entity =>
                {
                    var dto = mapper.Map<MenuEntity, MenuTreeDto>(entity);

                    dto.Child = AuthMenuTree(entities, dto.Id);

                    menus.Add(dto);
                });

                return menus;
            }
        }
    }
}
