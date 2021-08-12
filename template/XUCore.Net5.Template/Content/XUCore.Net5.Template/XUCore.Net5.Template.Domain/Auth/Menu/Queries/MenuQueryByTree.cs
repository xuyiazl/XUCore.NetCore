using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Auth;

namespace XUCore.Net5.Template.Domain.Auth.Menu
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
            private readonly ITaxDbRepository db;
            private readonly IMapper mapper;

            public Handler(ITaxDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<MenuTreeDto>> Handle(MenuQueryByTree request, CancellationToken cancellationToken)
            {
                var res = await db.Context.Menu
                    .OrderByDescending(c => c.Weight)
                    .ToListAsync(cancellationToken);

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
