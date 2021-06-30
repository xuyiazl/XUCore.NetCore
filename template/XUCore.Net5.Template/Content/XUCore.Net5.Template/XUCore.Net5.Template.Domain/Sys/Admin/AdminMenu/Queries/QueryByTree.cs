using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;

namespace XUCore.Net5.Template.Domain.Sys.AdminMenu
{
    /// <summary>
    /// 查询导航树命令
    /// </summary>
    public class AdminMenuQueryByTree : Command<IList<AdminMenuTreeDto>>
    {
        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandValidator<AdminMenuQueryByTree>
        {
            public Validator()
            {

            }
        }

        public class Handler : CommandHandler<AdminMenuQueryByTree, IList<AdminMenuTreeDto>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<AdminMenuTreeDto>> Handle(AdminMenuQueryByTree request, CancellationToken cancellationToken)
            {
                var res = await db.Context.AdminAuthMenus
                    .OrderByDescending(c => c.Weight)
                    .ToListAsync(cancellationToken);

                return AuthMenuTree(res, 0);
            }

            private IList<AdminMenuTreeDto> AuthMenuTree(IList<AdminMenuEntity> entities, long parentId)
            {
                IList<AdminMenuTreeDto> menus = new List<AdminMenuTreeDto>();

                entities.Where(c => c.FatherId == parentId).ToList().ForEach(entity =>
                {
                    var dto = mapper.Map<AdminMenuEntity, AdminMenuTreeDto>(entity);

                    dto.Child = AuthMenuTree(entities, dto.Id);

                    menus.Add(dto);
                });

                return menus;
            }
        }
    }
}
